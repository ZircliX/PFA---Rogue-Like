using UnityEngine;

namespace DeadLink.Menus
{
    public abstract class Menu : MonoBehaviour
    {
        public virtual void OnOpen()
        {
            gameObject.SetActive(true);
        }   
    }
}