using LTX.Singletons;
using UnityEngine;
using ZLinq;

namespace DeadLink.Menus.Implementation.Extensions
{
    public class ContextWindowManager : MonoSingleton<ContextWindowManager>
    {
        [SerializeField] private ContextWindowLink[] links;
        private ContextWindowLink currentLink;
        
        protected override void Awake()
        {
            base.Awake();
            
            for (int i = 0; i < links.Length; i++)
            {
                ContextWindowButton contextWindowButton = links[i].Button;
                
                contextWindowButton.OnButtonClick += ButtonClick;
                if (contextWindowButton.BaseState) currentLink = GetLink(contextWindowButton);
            }
        }

        protected override void OnDestroy()
        {
            for (int i = 0; i < links.Length; i++)
            {
                links[i].Button.OnButtonClick -= ButtonClick;
            }
            
            base.OnDestroy();
        }

        private void ButtonClick(ContextWindowButton button)
        {
            if (GetLink(button).Equals(currentLink)) return;
            if (!currentLink.IsNull) currentLink.ChangeState();

            currentLink = GetLink(button);
            currentLink.ChangeState();
        }

        private ContextWindowLink GetLink(ContextWindowButton button)
        {
            return links.AsValueEnumerable().First(ctx => ctx.Button == button);;
        }
    }
}