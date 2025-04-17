using DeadLink.Menus.Interfaces;
using LTX.ChanneledProperties;
using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public struct UpgradeMenuContext : IMenuContext
    {
        public GameObject GameObject { get; set; }
        public MenuType MenuType { get; set; }
        public PriorityTags Priority { get; set; }
        public CursorLockMode CursorLockMode { get; set; }
        public bool CursorVisibility { get; set; }
        public float TimeScale { get; set; }
        public bool CanClose { get; set; }
        public bool CanStack { get; set; }
        public UpgradeMenuHandler handler { get; set; }

        public Transform[] PowerUps { get; set; }
    }
}