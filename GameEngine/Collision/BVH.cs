using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;


namespace GameEngine.Collision
{
    class BVH{

        public Box boundingBox;

        public BVH child1;
        public BVH child2;

        public RigidBody containedRigidBody;

        public BVH(LinkedList<RigidBody> rigidbodies)
        {
            this.boundingBox = buildTightBoundingBox(rigidbodies);
            if (rigidbodies.Count == 1)
            {
                this.containedRigidBody = rigidbodies.First.Value;
            }
            else
            {
                LinkedList<RigidBody>[] splited = split(rigidbodies, this.boundingBox);
                this.child1 = new BVH(splited[0]);
                this.child2 = new BVH(splited[1]);
            }
        }

        /// <summary>
        /// Split the rigidbody list into two balanced ones
        /// </summary>
        /// <param name="rigidBodies"></param>
        /// <param name="boundingBox"></param>
        /// <returns></returns>
        public static LinkedList<RigidBody>[] split(LinkedList<RigidBody> rigidBodies, Box boundingBox)
        {
            LinkedList<RigidBody> left = new LinkedList<RigidBody>();
            LinkedList<RigidBody> right = new LinkedList<RigidBody>();
            if (boundingBox.getWidth() >= boundingBox.getHeight())
            {
                double mid = boundingBox.getMidX();
                foreach (RigidBody rigidBody in rigidBodies)
                {
                    if (rigidBody.Position.X <= mid)
                    {
                        left.AddLast(rigidBody);
                    }
                    if (rigidBody.Position.X > mid)
                    {
                        right.AddLast(rigidBody);
                    }
                }
            }
            if (boundingBox.getWidth() < boundingBox.getHeight())
            {
                double mid = boundingBox.getMidY();
                foreach (RigidBody rigidBody in rigidBodies)
                {
                    if (rigidBody.Position.Y <= mid)
                    {
                        left.AddLast(rigidBody);
                    }
                    if (rigidBody.Position.Y > mid)
                    {
                        right.AddLast(rigidBody);
                    }
                }
            }
            return new LinkedList<RigidBody>[] { left, right };
        }

        /// <summary>
        /// Returns the smallest possible box which fully encloses every circle in circles
        /// </summary>
        public static Box buildTightBoundingBox(LinkedList<RigidBody> rigidBodies)
        {
            Vector2 bottomLeft = new Vector2(float.PositiveInfinity);
            Vector2 topRight = new Vector2(float.NegativeInfinity);

            foreach (RigidBody r in rigidBodies)
            {
                bottomLeft = Vector2.Min(bottomLeft, r.getBoundingBox().bottomLeft);
                topRight = Vector2.Max(topRight, r.getBoundingBox().topRight);
            }

            return new Box(bottomLeft, topRight);
        }
        public override String ToString()
        {
            return "Not implemented yet";
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                if (this.containedRigidBody != null && ((BVH)obj).containedRigidBody == null)
                    return false;
                else if (this.containedRigidBody == null && ((BVH)obj).containedRigidBody != null)
                    return false;
                else if (this.containedRigidBody == null && ((BVH)obj).containedRigidBody == null)
                    return (this.child1.Equals(((BVH)obj).child1)) && (this.child2.Equals(((BVH)obj).child2));
                else if (this.containedRigidBody != null && ((BVH)obj).containedRigidBody != null)
                    return this.containedRigidBody.Equals(((BVH)obj).containedRigidBody);
                else return false;
            }
        }
    }
}
