using System;
using System.Data.SqlTypes;

namespace DeadLink.Menus.Extensions
{
    [System.Serializable]
    public struct ContextWindowLink : INullable, IEquatable<ContextWindowLink>
    {
        public ContextWindowButton Button;
        public ContextWindow Window;

        public void Enter()
        {
            Button.Enter();
            Window.Enter();
        }

        public void Exit()
        {
            Button.Exit();
            Window.Exit();
        }

        public bool IsNull => Button == null && Window == null;

        public bool Equals(ContextWindowLink other) => Equals(Button, other.Button) && Equals(Window, other.Window);
        public override bool Equals(object obj) => obj is ContextWindowLink other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Button, Window);
    }
}