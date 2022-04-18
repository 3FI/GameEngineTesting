using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine.Collision
{
    public class ContactResult
    {
        public double penetrationDepth;
        public Vector2 contactNormal;
        public RigidBody a;
        public RigidBody b;

        public ContactResult()
        {
            this.penetrationDepth = 0f;
            this.contactNormal = new Vector2();
            this.a = null;
            this.b = null;
        }

        public ContactResult(RigidBody a, RigidBody b, double penetrationDepth, Vector2 contactNormal)
        {
            this.penetrationDepth = penetrationDepth;
            this.contactNormal = contactNormal;
            this.a = a;
            this.b = b;
        }
        public override String ToString()
        {
            return "ContactResult(\n\tPenetration Depth: " + penetrationDepth + ", \n\tContact Normal: " + contactNormal + ", \n\ta: " + a + ", \n\tb: " + b + "\n)";
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return (penetrationDepth == ((ContactResult)obj).penetrationDepth) && (contactNormal == ((ContactResult)obj).contactNormal) && (a == ((ContactResult)obj).a) && (b == ((ContactResult)obj).b);
            }
        }
    }
}
