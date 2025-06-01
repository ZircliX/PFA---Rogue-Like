using DeadLink.VoiceLines;
using FMOD.Studio;
using FMODUnity;
using LTX.ChanneledProperties;
using RogueLike;
using RogueLike.Controllers;
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

        private bool enableVoicelines = true;

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
            MenuType = GameMetrics.Global.SettingsMenu;
            
            musicBus = RuntimeManager.GetBus("bus:/Music");
            sfxBus = RuntimeManager.GetBus("bus:/SFX");
            voiceBus = RuntimeManager.GetBus("bus:/Voices");

            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
            voiceSlider.onValueChanged.AddListener(SetVoiceVolume);
            
            LoadSettings();
        }

        public override void Initialize()
        {
            LoadSettings();
            base.Initialize();
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
        
        #region SoundsSettings
        public void SetMusicVolume(float volume)
        {
            musicBus.setVolume(volume);
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

        public void EnableVoicelines()
        {
            enableVoicelines = !enableVoicelines;
            VoiceLinesManager.Instance.SetActiveState(enableVoicelines);
        }

        public void ChangeLanguage(int index)
        {
            //héhé pas sur 
        }
    }
}