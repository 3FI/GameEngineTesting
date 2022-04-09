using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine.Collision
{
    class Collision
    {
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

        // Handles the physics simulation for a single contact between a circle and the boundaries of the level
        // You are not responsible for understanding this method.
        // You should not modify this method.
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
                    screenSize.Y - 2*Math.Abs(r.downMostPoint().Y - r.Position.Y)
                )
            );
        }

        // Handles a single tick/frame of the physics simulator.
        // If bvhAccelerated is set, circle/circle contacts will be found using getContactsBVH(), otherwise getContactsNaive().
        // Each circle/circle contact will then be resolved/simulated.
        // Each circle will then be contact checked against the 4 walls and all such contacts will be resolved/simulated.
        // We then step the simulation forward using numerical integration.
        // You are not responsible for understanding this method.
        // You should not modify this method.
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
                Vector2 screenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
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

            HashSet<ContactResult> contacts = ContactFinder.getContactsBVH(_rigidBody, bvh);
            if (contacts == null)
            {
                return;
            }

            foreach (ContactResult cr in contacts)
            {
                resolveContact(cr);
            }

            // resolve contacts with walls;
            
        }
    }
}
