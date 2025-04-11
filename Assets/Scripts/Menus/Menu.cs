using UnityEngine;

namespace DeadLink.Menus
{
    public abstract class Menu : MonoBehaviour
    {
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