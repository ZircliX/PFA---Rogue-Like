using LTX.ChanneledProperties;
using UnityEngine;

namespace DeadLink.Menus
{
    public struct MenuProperties
    {
        public readonly GameObject GameObject;
        public readonly PriorityTags Priority;
        public readonly float TimeScale;
        public readonly CursorLockMode CursorLockMode;
        public readonly bool CursorVisibility;
        public readonly bool CanClose;
        public readonly bool CanStack;
        
        public MenuProperties(GameObject gameObject, PriorityTags priority, float timeScale, CursorLockMode cursorLockMode, bool cursorVisibility, bool canClose, bool canStack)
        {
            GameObject = gameObject;
            Priority = priority;
            TimeScale = timeScale;
            CursorLockMode = cursorLockMode;
            CursorVisibility = cursorVisibility;
            CanClose = canClose;
            CanStack = canStack;
        }
    }
}