using DG.Tweening;
using EditorAttributes;
using KBCore.Refs;
using UnityEngine;

namespace DeadLink.Menus.New
{
    public abstract class Menu : MonoBehaviour, IMenu
    {
        [field : SerializeField] public bool BaseState { get; set; }
        [SerializeField, Self, Required] private CanvasGroup canvasGroup;

        public abstract MenuType MenuType { get; protected set; }

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        public virtual void Initialize()
        {
            canvasGroup.interactable = BaseState;
            canvasGroup.blocksRaycasts = BaseState;
            canvasGroup.DOFade(BaseState ? 1 : 0, 0.25f).SetUpdate(true);
        }

        public virtual void Open()
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.DOFade(1f, 0.25f).SetUpdate(true);
        }

        public virtual void Close()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.DOFade(0f, 0.25f).SetUpdate(true);
        }
        
        public abstract MenuProperties GetMenuProperties();
    }
}