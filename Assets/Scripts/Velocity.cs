using System;
using UnityEngine;

namespace RogueLike
{
    public struct Velocity : IComparable<Velocity>
    {
        public Vector3 vector;

        public static implicit operator Vector3(Velocity vel)
        {
            return vel.vector;
        }
        
        public static implicit operator Velocity(Vector3 vec)
        {
            return new Velocity()
            {
                vector = vec,
            };
        }
        
        public int CompareTo(Velocity other)
        {
            return 0;
        }
    }
}