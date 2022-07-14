 using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine.Collision
{
    /// <summary>
    /// Static class which holds multiple core function for collision handling and simulation
    /// </summary>
    static class Collision
    {
        /// <summary>
        /// The current bounding volume hierarchy of the scene. Used to optimize collision to always test between pair of close objects.
        /// </summary>
        public static BVH Bvh;

        /// <summary>
        /// Simulate all colisions in the LinkedList
        /// </summary>
        /// <param name="RigidBody">The list of rigidbody which we want to simulate</param>
        public static void simulate(LinkedList<RigidBody> _rigidBody)
        {
            //Initialize the bouding volume hierarchy of the scene such that we only test for contact between closest pairs.
            Bvh = new BVH(_rigidBody);

            //Get the hashset of all the contacts that has happened
            HashSet<ContactResult> contacts = getContactsBVH(_rigidBody, Bvh);
            if (contacts == null)
            {
                return;
            }

            //Resolve each contact
            foreach (ContactResult cr in contacts)
            {
                resolveContact(cr);
            }
        }

        /// <summary>
        /// Finds all colisions in the BVH
        /// </summary>
        /// <param name="rigidBodies">The list of rigidbody to test if they have collisions.</param>
        /// <param name="bvh">The BVH used to find the closest object.</param>
        /// <returns></returns>
        public static HashSet<ContactResult> getContactsBVH(LinkedList<RigidBody> rigidBodies, BVH bvh)
        {
            HashSet<ContactResult> result = new HashSet<ContactResult>();

            foreach (RigidBody rb in rigidBodies)
            {
                //Gets if there's a contact between the current rigidbody and the BVH.
                HashSet<ContactResult> contact = getContactBVH(rb, bvh);
                //If there's a contact, adds it to the hashset.
                if (contact.Count != 0)
                {
                    result.UnionWith(contact);
                }
            }
            return result;
        }

        /// <summary>
        /// Get's if there's a contact between ONE rigidbody and the BVH
        /// </summary>
        /// <param name="r"></param>
        /// <param name="bvh"></param>
        /// <returns></returns>
        public static HashSet<ContactResult> getContactBVH(RigidBody r, BVH bvh)
        {
            HashSet<ContactResult> result = new HashSet<ContactResult>();

            //If the bounding box don't even intersect, there's no chance there's a collision.
            if (!r.getBoundingBox().intersectBox(bvh.boundingBox))
            {
                return result;
            }

            //If we reached a leaf of the tree, test if the leaf is a contact
            else if (bvh.child1 == null && bvh.child2 == null)
            {
                //Verify if the leaf is the same as the current rigidbody. (we don't want a contact with itself)
                if (bvh.containedRigidBody.Id != r.Id)
                {
                    //Test if there's an actual contact between the two rigidbody and returns it.
                    ContactResult contact = r.isContacting(bvh.containedRigidBody);
                    if (contact != null)
                    {
                        //If there's a contact, adds it to the hashset.
                        result.Add(contact);
                    }
                }
            }

            //If the current node is not a leaf then recursivly search for leaves.
            else if (bvh.child1 != null || bvh.child2 != null)
            {
                //TODO : OPTIMIZE TO ALWAYS GET CLOSEST LEAF
                //if(bvh.child1.boundingBox.intersectBox(r.getBoundingBox()))
                    result.UnionWith(getContactBVH(r, bvh.child1));
                //else if (bvh.child2.boundingBox.intersectBox(r.getBoundingBox()))
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
            //Calculates the relative & normal velocity of the two rigidbody
            Vector2 relativeVelocity = Vector2.Subtract(cr.a.Velocity, cr.b.Velocity);
            double normalVelocity = Vector2.Dot(relativeVelocity, cr.contactNormal);

            //If the normal velocity is above 0, then the 2 objects are already moving away from each others
            if (normalVelocity > 0)
            {
                return;
            }

            //The quantity of the velocity that is lost in the collision. (1f = inelastic, 0f = elastic)
            double restitution = 0.99f;

            //TODO : ADD MASS FACTOR
            //TODO : ADD TORQUE

            //If both rigidbody are fixed there's a problem
            if (cr.a.fix && cr.b.fix) { System.Diagnostics.Debug.WriteLine("Collision Between 2 Fixed RB"); return; }

            //If either rigid body is fixed, the other gets the complete displacement and impulse
            else if (cr.a.fix)
            {
                Vector2 impulse = Vector2.Multiply(cr.contactNormal, (float)restitution * (float)normalVelocity);
                cr.b.Velocity = Vector2.Add(cr.b.Velocity, impulse);
                cr.b.Position = Vector2.Subtract(cr.b.Position, Vector2.Multiply(cr.contactNormal, 1f * (float)cr.penetrationDepth));
            }
            else if (cr.b.fix)
            {
                Vector2 impulse = Vector2.Multiply(cr.contactNormal, (float)restitution * (float)normalVelocity);
                cr.a.Velocity = Vector2.Subtract(cr.a.Velocity, impulse);
                cr.a.Position = Vector2.Add(cr.a.Position, Vector2.Multiply(cr.contactNormal, 1f * (float)cr.penetrationDepth));
            }
            //Else, the impulse and displacement are split in two
            else
            {
                Vector2 impulse = Vector2.Multiply(cr.contactNormal, (float)restitution * (float)normalVelocity / 2f);
                cr.a.Velocity = Vector2.Subtract(cr.a.Velocity, impulse);
                cr.b.Velocity = Vector2.Add(cr.b.Velocity, impulse);
                cr.a.Position = Vector2.Add(cr.a.Position, Vector2.Multiply(cr.contactNormal, 0.5f * (float)cr.penetrationDepth));
                cr.b.Position = Vector2.Subtract(cr.b.Position, Vector2.Multiply(cr.contactNormal, 0.5f * (float)cr.penetrationDepth));
            }
        }

        //LEGACY : code used in simulation to test for collision with the edge of the screen
        /*
        foreach (RigidBody r in _rigidBody)
        {
            Vector2 screenSize = new Vector2(30, 16);
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
        */

        //LEGACY : Collision resolution with the edge of the screen
        /*
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
                Math.Min
                (
                    Math.Max
                    (
                        r.Position.X,
                        Math.Abs(r.leftMostPoint().X - r.Position.X)
                    ),
                    screenSize.X - Math.Abs(r.rightMostPoint().X - r.Position.X)
                ),
                Math.Min
                (
                    Math.Max
                    (
                        r.Position.Y,
                        Math.Abs(r.upMostPoint().Y - r.Position.Y)
                    ),
                    screenSize.Y - Math.Abs(r.downMostPoint().Y - r.Position.Y)
                )
            );
        }
        */
    }
}
