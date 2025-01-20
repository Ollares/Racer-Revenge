using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsScreen : UIScreen
{
    [SerializeField] RectTransform mainPanel;
    [SerializeField] Button closeButton;
    [SerializeField] Button backgroundButton;
    [SerializeField] SwitchToggle hapticSwitchToggle;
    [SerializeField] SwitchToggle soundSwitchToggle;
    [SerializeField] SwitchToggle musicSwitchToggle;
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider musicSlider;

    private void Awake()
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => { Close(); });
        backgroundButton.onClick.RemoveAllListeners();
        backgroundButton.onClick.AddListener(() => { Close(); });
        hapticSwitchToggle.OnToggleSwitch = HapticToggle;
        soundSwitchToggle.OnToggleSwitch = SoundToggle;
        musicSwitchToggle.OnToggleSwitch = MusicToggle;
        soundSlider.onValueChanged.AddListener(SoundVolume);
        musicSlider.onValueChanged.AddListener(MusicVolume);
    }
    private void SoundVolume(float value)
    {
        GameData.Instance.SoundVolume = value;
        //update sound mixer
        //AudioPlayer.Instance.SetSoundVolume(value);

    }
    private void MusicVolume(float value)
    {
        GameData.Instance.MusicVolume = value;
        //update music mixer
        //AudioPlayer.Instance.SetMusicVolume(value);
    }

    public void HapticToggle(bool isOn)
    {
        GameData.Instance.Vibration = isOn;
        if (isOn)
        {
            HapticManager.HapticLight();
        }
    }
    public void SoundToggle(bool isOn)
    {
        GameData.Instance.Sound = isOn;
        //AudioPlayer.Instance.SoundEnable(isOn);
        HapticManager.HapticLight();
    }
    public void MusicToggle(bool isOn)
    {
        GameData.Instance.Music = isOn;
        //AudioPlayer.Instance.MusicEnable(isOn);
        HapticManager.HapticLight();
    }
    public override void Open()
    {
        hapticSwitchToggle.SetState(GameData.Instance.Vibration);
        soundSwitchToggle.SetState(GameData.Instance.Sound);
        musicSwitchToggle.SetState(GameData.Instance.Music);
        soundSlider.value = GameData.Instance.SoundVolume;
        musicSlider.value = GameData.Instance.MusicVolume;

        base.Open();
    }
    public override void Close()
    {
        base.Close();
    }
}
