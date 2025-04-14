using LTX.ChanneledProperties;
using UnityEngine;

namespace DeadLink.Menus.Interfaces
{
    public interface IMenuContext
    {
        GameObject GameObject { get; set; }
        
        public PriorityTags Priority { get; set; }
        CursorLockMode CursorLockMode { get; set; }
        bool CursorVisibility { get; set; }
        float TimeScale { get; set; }
        
        bool CanClose { get; set; }
        bool CanStack { get; set; }
    }
}