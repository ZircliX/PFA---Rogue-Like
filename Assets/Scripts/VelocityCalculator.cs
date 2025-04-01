using LTX.ChanneledProperties;
using UnityEngine;

namespace RogueLike
{
    public class VelocityCalculator : IBaseCalculator<Velocity>
    {
        public Velocity Add(Velocity first, Velocity second)
        {
            return new Velocity()
            {
                vector = new Vector3()
                {
                    x = (first.vector.x + second.vector.x),
                    y = (first.vector.y + second.vector.y),
                    z = (first.vector.z + second.vector.z)
                }
            };
        }

        public Velocity Substract(Velocity first, Velocity second)
        {
            return new Velocity()
            {
                vector = new Vector3()
                {
                    x = (first.vector.x - second.vector.x),
                    y = (first.vector.y - second.vector.y),
                    z = (first.vector.z - second.vector.z)
                }
            };
        }

        public Velocity Divide(Velocity first, Velocity second)
        {
            return new Velocity()
            {
                vector = new Vector3()
                {
                    x = (first.vector.x / second.vector.x),
                    y = (first.vector.y / second.vector.y),
                    z = (first.vector.z / second.vector.z)
                }
            };
        }

        public Velocity Multiply(Velocity first, Velocity second)
        {
            return new Velocity()
            {
                vector = new Vector3()
                {
                    x = (first.vector.x * second.vector.x),
                    y = (first.vector.y * second.vector.y),
                    z = (first.vector.z * second.vector.z)
                }
            };
        }
    }
}