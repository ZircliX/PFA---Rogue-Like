using System;
using System.Data.SqlTypes;

namespace DeadLink.Menus.Implementation.Extensions
{
    [System.Serializable]
    public struct ContextWindowLink : INullable, IEquatable<ContextWindowLink>
    {
        public ContextWindowButton Button;
        public ContextWindow Window;

        public void ChangeState()
        {
            Button.ChangeState();
            Window.ChangeState();
        }

        public bool IsNull => Button == null && Window == null;

        public bool Equals(ContextWindowLink other) => Equals(Button, other.Button) && Equals(Window, other.Window);
        public override bool Equals(object obj) => obj is ContextWindowLink other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Button, Window);
    }
}