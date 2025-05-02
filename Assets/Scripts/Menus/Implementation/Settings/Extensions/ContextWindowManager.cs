using System;
using EditorAttributes;
using UnityEngine;
using Void = EditorAttributes.Void;

namespace DeadLink.Menus.Implementation.Extensions
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
                links[i].SetValues(i == index);
                currentLink = i == index ? links[i] : currentLink;
            }
        }
        #endregion
        
        private void Awake()
        {
            currentLink.ChangeState();
            
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

        private void ButtonClick(ContextWindowButton button)
        {
            if (!currentLink.IsNull) currentLink.ChangeState();
            
            for (int i = 0; i < links.Length; i++)
            {
                if (links[i].Button == button)
                {
                    currentLink = links[i];
                    currentLink.ChangeState();
                }
            }
        }
    }
}