using System;
using EditorAttributes;
using UnityEngine;
using Void = EditorAttributes.Void;

namespace DeadLink.Menus.Extensions
{
    public class ContextWindowManager : MonoBehaviour
    {
        [SerializeField] private ContextWindowLink[] links;
        [ReadOnly, SerializeField] private ContextWindowLink currentLink;

        #region Editor
        [SerializeField, ValueButtons(nameof(windowNames))] private string selectedWindow;
        private string[] windowNames = new string[]
        {
            "Game", "Inputs", "Audio", "Graphics"
        };
        
        [ButtonField(nameof(ResetValues), "Refresh Windows")]
        [SerializeField] private Void RefreshWindows;
        
        private void ResetValues()
        {
            int index = Array.IndexOf(windowNames, selectedWindow);
            
            for (int i = 0; i < links.Length; i++)
            {
                ContextWindowLink contextWindowLink = links[i];
                
                if (i == index)
                {
                    currentLink = contextWindowLink;
                    currentLink.Enter();
                }
                else
                {
                    contextWindowLink.Exit();
                }
            }
        }
        #endregion
        
        private void Awake()
        {
            currentLink.Enter();
            
            for (int i = 0; i < links.Length; i++)
            {
                links[i].Button.OnButtonClick += ButtonClick;
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < links.Length; i++)
            {
                links[i].Button.OnButtonClick -= ButtonClick;
            }
        }

        private void ManageButtonClick(ContextWindowButton button)
        {
            if (!currentLink.IsNull) currentLink.Exit();
            
            for (int i = 0; i < links.Length; i++)
            {
                if (links[i].Button == button)
                {
                    currentLink = links[i];
                    currentLink.Enter();
                }
            }
        }

        private void ButtonClick(ContextWindowButton button)
        {
            if (button.Equals(currentLink.Button)) return;
            ManageButtonClick(button);
        }
    }
}