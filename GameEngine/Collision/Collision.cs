﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine.Collision
{
    class Collision
    {
        /// <summary>
        /// Simulate all loaded colisions
        /// </summary>
        /// <param name="gameObjects"></param>
        /// <param name="_graphics"></param>
        public static void simulate(LinkedList<GameObject> gameObjects, GraphicsDeviceManager _graphics)
        {
            LinkedList<RigidBody> _rigidBody = new LinkedList<RigidBody>();
            foreach (GameObject gameObject in gameObjects)
            {
                _rigidBody.AddLast(gameObject.Rigidbody);
            }

            BVH bvh = new BVH(_rigidBody);
            
            foreach (RigidBody r in _rigidBody)
            {
                Vector2 screenSize = new Vector2(_graphics.PreferredBackBufferWidth/64, _graphics.PreferredBackBufferHeight/64);
                if (r.leftMostPoint().X < 0)
                {
                    resolveBoundaryContact(r, new Vector2(1, 0), screenSize);
                }
                if (r.rightMostPoint().X > screenSize.X)
                {
                    resolveBoundaryContact(r, new Vector2(-1, 0), screenSize);
                }
                if (r.downMostPoint().Y > screenSize.Y)
                {
                    resolveBoundaryContact(r, new Vector2(0, -1), screenSize);
                }
                if (r.upMostPoint().Y < 0)
                {
                    resolveBoundaryContact(r, new Vector2(0, 1), screenSize);
                }
            }
            
            HashSet<ContactResult> contacts = getContactsBVH(_rigidBody, bvh);
            if (contacts == null)
            {
                return;
            }

            foreach (ContactResult cr in contacts)
            {
                resolveContact(cr);
            }            
        }

        /// <summary>
        /// Finds all colisions in the BVH
        /// </summary>
        /// <param name="rigidBodies"></param>
        /// <param name="bvh"></param>
        /// <returns></returns>
        public static HashSet<ContactResult> getContactsBVH(LinkedList<RigidBody> rigidBodies, BVH bvh)
        {
            HashSet<ContactResult> result = new HashSet<ContactResult>();
            foreach (RigidBody rb in rigidBodies)
            {
                HashSet<ContactResult> contact = getContactBVH(rb, bvh);
                if (contact.Count != 0)
                {
                    result.UnionWith(contact);
                }
            }
            return result;
        }

        public static HashSet<ContactResult> getContactBVH(RigidBody r, BVH bvh)
        {
            HashSet<ContactResult> result = new HashSet<ContactResult>();
            if (!r.getBoundingBox().intersectBox(bvh.boundingBox))
            {
                return result;
            }
            else if (bvh.child1 == null && bvh.child2 == null)
            {
                if (bvh.containedRigidBody.Id != r.Id)
                {
                    ContactResult contact = r.isContacting(bvh.containedRigidBody);
                    if (contact != null)
                    {
                        result.Add(contact);
                    }
                }
            }
            else if (bvh.child1 != null || bvh.child2 != null)
            {
                result.UnionWith(getContactBVH(r, bvh.child1));
                result.UnionWith(getContactBVH(r, bvh.child2));
            }
            return result;
        }

        /// <summary>
        /// Resolve contact between rigidbodies
        /// </summary>
        /// <param name="cr"></param>
        public static void resolveContact(ContactResult cr)
        {
            Vector2 relativeVelocity = Vector2.Subtract(cr.a.Velocity, cr.b.Velocity);
            double normalVelocity = Vector2.Dot(relativeVelocity, cr.contactNormal);

            if (normalVelocity > 0)
            {
                return;
            }

            double restitution = 0.99f;
            Vector2 impulse = Vector2.Multiply(cr.contactNormal, (float)restitution * (float)normalVelocity / 2f);
            cr.a.Velocity = Vector2.Subtract(cr.a.Velocity, impulse);
            cr.b.Velocity = Vector2.Add(cr.b.Velocity, impulse);

            cr.a.Position = Vector2.Subtract(cr.a.Position, Vector2.Multiply(cr.contactNormal, 0.5f * (float)cr.penetrationDepth));
            cr.b.Position = Vector2.Add(cr.b.Position, Vector2.Multiply(cr.contactNormal, 0.5f * (float)cr.penetrationDepth));
        }

        /// <summary>
        /// Resolve contact with the screen border
        /// </summary>
        /// <param name="r"></param>
        /// <param name="collisionNormal"></param>
        /// <param name="screenSize"></param>
        public static void resolveBoundaryContact(RigidBody r, Vector2 collisionNormal, Vector2 screenSize)
        {
            double normalVelocity = Vector2.Dot(r.Velocity, collisionNormal);
            if (normalVelocity > 0)
            {
                return;
            }
            double restitution = 1f;
            Vector2 impulse = Vector2.Multiply(collisionNormal, (float)restitution * (float)normalVelocity);
            r.Velocity = Vector2.Subtract(r.Velocity, impulse);
            r.Position = new Vector2(
                CustomMath.Min
                (
                    CustomMath.Max
                    (
                        r.Position.X,
                        3 * CustomMath.Abs(r.leftMostPoint().X - r.Position.X)
                    ),
                    screenSize.X - Math.Abs(r.rightMostPoint().X - r.Position.X)
                ),
                CustomMath.Min
                (
                    CustomMath.Max
                    (
                        r.Position.Y,
                        3 * CustomMath.Abs(r.upMostPoint().Y - r.Position.Y)
                    ),
                    screenSize.Y - Math.Abs(r.downMostPoint().Y - r.Position.Y)
                )
            );
        }
    }
}