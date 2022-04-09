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

        public ContactResult(Circle a, Circle b)
        {
            this.penetrationDepth = 0.0;
            this.a = a;
            this.b = b;
        }

        public override String ToString()
        {
            RigidBody first = a.Id < b.Id ? a : b;
            RigidBody second = a.Id >= b.Id ? a : b;
            return "Contact between circles: " + first + " and " + second;
        }

        /*
        public override bool Equals(Object o)
        {
                if (this == o) return true;
                if (o == null || GetType() != o.GetType()) return false;
                ContactResult that = (ContactResult)o;
                return this.hashCode() == that.hashCode();
        }

        public override int hashCode()
        {
                Circle first = a.Id < b.Id ? a : b;
                Circle second = a.Id >= b.Id ? a : b;
                return Objects.hash(first, second);
        }
        */
    }
}
