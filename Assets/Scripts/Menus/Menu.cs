using UnityEngine;

namespace DeadLink.Menus.New
{
    public abstract class Menu : MonoBehaviour, IMenu
    {
        [field : SerializeField] public bool BaseState { get; set; }

        public abstract MenuType MenuType { get; protected set; }

        public virtual void Initialize()
        {
            gameObject.SetActive(BaseState);
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
        
        public abstract MenuProperties GetMenuProperties();
    }
}