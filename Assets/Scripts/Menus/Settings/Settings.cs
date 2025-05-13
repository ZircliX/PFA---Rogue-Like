using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

namespace DeadLink.Menus.Settings
{
    public class Settings : MonoBehaviour
    {
        [Header("Sound Settings")]
        
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider voiceSlider;
        private Bus musicBus;
        private Bus sfxBus;
        private Bus voiceBus;
        
        [Header("Screen Resolutions Settings")]
        
        [SerializeField] private Dropdown resolutionDropdown;
        private Resolution[] resolutions;
        private int currentResolutionIndex = 0;
        
        private void Start()
        {
            musicBus = RuntimeManager.GetBus("bus:/Music");
            sfxBus = RuntimeManager.GetBus("bus:/SFX");
            voiceBus = RuntimeManager.GetBus("bus:/Voices");

            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
            voiceSlider.onValueChanged.AddListener(SetVoiceVolume);
            
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            var options = new System.Collections.Generic.List<string>();

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();

            resolutionDropdown.onValueChanged.AddListener(SetResolution);
        }
        #region ScreenResolutionSettings
        
        public void SetResolution(int resolutionIndex)
        {
            Resolution res = resolutions[resolutionIndex];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        }
        #endregion
        
        
        #region SoundsSettings
        public void SetMusicVolume(float volume)
        {
            musicBus.setVolume(volume); // 0.0 = muet, 1.0 = volume max
        }

        public void SetSfxVolume(float volume)
        {
            sfxBus.setVolume(volume);
        }

        public void SetVoiceVolume(float volume)
        {
            voiceBus.setVolume(volume);
        }

        #endregion
    }
}