using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using LTX.ChanneledProperties;
using RogueLike.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DeadLink.Menus.Implementation
{
    public class SettingsMenu : Menu
    {
        
        public override MenuType MenuType { get; protected set; }
        
        [Header("Sound Settings")]
        
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider voiceSlider;
        private Bus musicBus;
        private Bus sfxBus;
        private Bus voiceBus;
        
        [Header("Screen Resolutions Settings")]
        
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        private Resolution[] resolutions;
        private int currentResolutionIndex = 0;
        
        [Header("Luminosity Settings")]
        [SerializeField] private Slider brightnessSlider;


        public override MenuProperties GetMenuProperties()
        {
            return new MenuProperties(
                gameObject,
                PriorityTags.Default,
                0f,
                CursorLockMode.None,
                true,
                true,
                false);
        }
        
        private void Awake()
        {
            MenuType = MenuType.Settings;
        }
        
        public override void Open()
        {
            base.Open();
            LoadSettings();
        }

        public override void Close()
        {
            SaveSettings();
            base.Close();
        }

        private void LoadSettings()
        {
            SetMusicVolume(GameController.SettingsListener.MusicVolume);
            musicSlider.value = GameController.SettingsListener.MusicVolume;
            SetSfxVolume(GameController.SettingsListener.SfxVolume);
            sfxSlider.value = GameController.SettingsListener.SfxVolume;
            SetVoiceVolume(GameController.SettingsListener.VoiceVolume);
            voiceSlider.value = GameController.SettingsListener.VoiceVolume;
        }

        private void SaveSettings()
        {
            if (musicSlider == null || sfxSlider == null || voiceSlider == null)
            {
                Debug.LogError("Settings sliders are not initialized.");
                return;
            }
            
            GameController.SettingsListener.SetMusicVolume(musicSlider.value);
            GameController.SettingsListener.SetSfxVolume(sfxSlider.value);
            GameController.SettingsListener.SetVoiceVolume(voiceSlider.value);
        }
        
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

            List<string> options = new List<string>();

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                //Debug.Log($"Added : {option}");
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
        
        #region LuminositySettings
        
        public void SetBrightness(float brightness)
        {
            //GlobalVolumeManager.Instance.globalVolume.profile.TryGet(out )
        }
        #endregion
        
        public void Back()
        {
            MenuManager.Instance.CloseMenu();
        }

        public void EnableVibrations(bool state)
        {
            
        }

        public void ChangeLanguage(int index)
        {
            //héhé pas sur 
        }
    }
}