using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DeadLink.Menus
{
    public class ResolutionCoach : MonoBehaviour
    {
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private string[] texts;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI ResolutionText;
        private int index;


        private void OnEnable()
        {
            LoadFullScreenMode();
        }

        private void OnDisable()
        {
            SaveFullScreenMode();
        }

        #region SaveScreenMode
        public void SaveFullScreenMode()
        {
            PlayerPrefs.SetInt("FullScreenMode", (int)Screen.fullScreenMode);
            PlayerPrefs.Save();
        }

        public void LoadFullScreenMode()
        {
            if (PlayerPrefs.HasKey("FullScreenMode"))
            {
                Screen.fullScreenMode = (FullScreenMode)PlayerPrefs.GetInt("FullScreenMode");
            }
        }
        #endregion
        
        private void Awake()
        {
            FullScreenMode currentMode = Screen.fullScreenMode;
            Screen.SetResolution(1920, 1080, currentMode);

        }
        
        public void NextSprite()
        {
            if (index + 1 > 2)
            {
                index = 0;
                ResolutionText.text = texts[index];
                image.sprite = sprites[index];
                return;
            }
            index++;
            ResolutionText.text = texts[index];
            image.sprite = sprites[index];
        }

        public void PreviousSprite()
        {
            if (index - 1 < 0)
            {
                index = 2;
                ResolutionText.text = texts[index];
                image.sprite = sprites[index];
                return;
            }
            index--;
            ResolutionText.text = texts[index];
            image.sprite = sprites[index];
        }

        public void AppliedResolution()
        {

            if (index == 0)
            {
                Screen.SetResolution(1920, 1080, true);
                Debug.Log($"{Screen.width}x{Screen.height}");

            }
            
            if (index == 1)
            {
                Screen.SetResolution(2560, 1440, true);
                Debug.Log($"{Screen.width}x{Screen.height}");

            }
            
            if (index == 2)
            {
                Screen.SetResolution(3840, 2160, true);
                Debug.Log($"{Screen.width}x{Screen.height}");

            }
        }
    }
}