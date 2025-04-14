using UnityEngine;

namespace DeadLink.Menus
{
    public abstract class Menu : MonoBehaviour
    {
        public string MenuName => gameObject.name;
        public abstract bool CanClose { get; protected set; }
        
        public virtual void OnOpen()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnClose()
        {
            gameObject.SetActive(false);
        }
    }
}