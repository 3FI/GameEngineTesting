using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace GameEngine.Collision
{
    /// <summary>
    /// Binary tree structure on a set of geometric objects. All geometric objects, that form the leaf nodes of the tree, are wrapped in bounding volumes. These nodes are then grouped as small sets and enclosed within larger bounding volumes.
    /// </summary>
    class BVH
    {

        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The bounding volume of the current node
        /// </summary>
        public Box boundingBox;

        /// <summary>
        /// One of the two child node of the tree (Nullable if the node contains a rigidbody instead)
        /// </summary>
        public BVH child1;
        /// <summary>
        /// One of the two child node of the tree (Nullable if the node contains a rigidbody instead)
        /// </summary>
        public BVH child2;

        /// <summary>
        /// The contained rigidbody of the leaf. (Nullable if 
        /// </summary>
        public RigidBody containedRigidBody;

        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rigidbodies"></param>
        public BVH(LinkedList<RigidBody> rigidbodies)
        {
            this.boundingBox = buildTightBoundingBox(rigidbodies);
            if (rigidbodies.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("ERROR : BVH COULDN'T BE CREATED, RIGIDBODIES LIST IS EMPTY");
            }
            else if (rigidbodies.Count == 1)
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

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Split the rigidbody list into two balanced ones
        /// </summary>
        /// <param name="rigidBodies">          </param>
        /// <param name="boundingBox">          </param>
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
                    if (rigidBody.Position.X < mid)
                    {
                        left.AddLast(rigidBody);
                    }
                    else if (rigidBody.Position.X > mid)
                    {
                        right.AddLast(rigidBody);
                    }
                    else
                    {
                        if (left.Count < right.Count) left.AddLast(rigidBody);
                        else right.AddLast(rigidBody);
                    }
                }
            }
            else if (boundingBox.getWidth() < boundingBox.getHeight())
            {
                double mid = boundingBox.getMidY();
                foreach (RigidBody rigidBody in rigidBodies)
                {
                    if (rigidBody.Position.Y < mid)
                    {
                        left.AddLast(rigidBody);
                    }
                    else if (rigidBody.Position.Y > mid)
                    {
                        right.AddLast(rigidBody);
                    }
                    else
                    {
                        if (left.Count < right.Count) left.AddLast(rigidBody);
                        else right.AddLast(rigidBody);
                    }
                }
            }

            if (left.Count == 0 || right.Count == 0)
            {
                LinkedList<RigidBody> inverse = new LinkedList<RigidBody>();
                while (rigidBodies.Count != 0)
                {
                    inverse.AddLast(rigidBodies.Last.Value);
                    rigidBodies.RemoveLast();
                }

                return split(inverse, boundingBox);
            }

            return new LinkedList<RigidBody>[] { left, right };
        }

        /// <summary>
        /// Returns the smallest possible box which fully encloses every circle in circles
        /// </summary>
        public static Box buildTightBoundingBox(LinkedList<RigidBody> rigidBodies)
        {
            Vector2 topLeft = new Vector2(float.PositiveInfinity);
            Vector2 bottomRight = new Vector2(float.NegativeInfinity);

            foreach (RigidBody r in rigidBodies)
            {
                topLeft = Vector2.Min(topLeft, r.getBoundingBox().topLeft);
                bottomRight = Vector2.Max(bottomRight, r.getBoundingBox().bottomRight);
            }

            return new Box(topLeft, bottomRight);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            if (boundingBox != null)
            {
                boundingBox.Draw(spritebatch, Color.Purple, 3);
                if (this.child1 != null) this.child1.Draw(spritebatch);
                if (this.child2 != null) this.child2.Draw(spritebatch);
            }
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
