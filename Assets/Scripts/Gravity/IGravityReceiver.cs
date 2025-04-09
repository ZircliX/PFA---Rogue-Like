using LTX.ChanneledProperties;
using UnityEngine;

namespace DeadLink.Gravity
{
    public interface IGravityReceiver
    {
        Vector3 Position { get; }
        PrioritisedProperty<Vector3> Gravity { get; }
    }
}