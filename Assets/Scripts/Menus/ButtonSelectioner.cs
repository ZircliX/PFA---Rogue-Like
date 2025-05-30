using System;
using LTX.Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace DeadLink.Menus
{
    public class ButtonSelectioner : MonoSingleton<ButtonSelectioner>
    {
        [SerializeField] private ButtonHandler currentSelectedLevelButton;
        [SerializeField] private ButtonHandler currentSelectedDifficultyButton;
        [SerializeField] private ButtonHandler[] levelButtons;
        [SerializeField] private ButtonHandler[] difficultyButtons;


        public void Select(ButtonHandler button)
        {
            if (Array.Exists(levelButtons, x => x == button))
            {
                if (currentSelectedLevelButton != null)
                {
                    Deselect(currentSelectedLevelButton);
                    
                }
                
                currentSelectedLevelButton = button;
                button.IsSelected = true;
            }

            if (Array.Exists(difficultyButtons, x => x == button))
            {
                if (currentSelectedDifficultyButton != null)
                {
                    Deselect(currentSelectedDifficultyButton);
                }
                currentSelectedDifficultyButton = button;
                button.IsSelected = true;
            }
            
            
        }

        public void Deselect(ButtonHandler button)
        {
            if (button.OriginalImage != null)
            {
                button.OriginalImage.material = null;  
            }
            
            if (button.HighlightedImage != null)
            {
                button.OriginalImage.sprite = button._image;
            }
            
            if (button.Text != null)
            {
                button.Text.color = button.OriginalColor;
            }
            button.IsSelected = false;
        }

    }
}