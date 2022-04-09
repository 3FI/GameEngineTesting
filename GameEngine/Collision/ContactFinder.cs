using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.Collision
{
    class ContactFinder
    {

        // todo for students
        // Returns a HashSet of ContactResult objects representing all the contacts between circles in the scene.
        // The runtime of this method should be O(n*log(n)) where n is the number of circles.
        public static HashSet<ContactResult> getContactsBVH(LinkedList<RigidBody> rigidBodies, BVH bvh)
        {
            HashSet<ContactResult> result = new HashSet<ContactResult>();
            foreach (RigidBody rb in rigidBodies)
            {
                HashSet<ContactResult> contact = getContactBVH(rb, bvh);
                if (contact.Count!=0)
                {
                    result.UnionWith(contact);
                }
            }
            return result;
        }

        // todo for students
        // Takes a single circle c and a BVH bvh.
        // Returns a HashSet of ContactResult objects representing contacts between c
        // and the circles contained in the leaves of the bvh.
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
    }
}
