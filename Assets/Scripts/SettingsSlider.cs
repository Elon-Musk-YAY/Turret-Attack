using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsSlider : MonoBehaviour
{

    public enum SettingTypes
    {
        Volume,
        ScrollSensitivity
    }

    public Slider slider;
    public SettingTypes type;
    bool set;

    public void Update()
    {
        if (set || !SettingsManager.settingsImported) return;
        if (type == SettingTypes.Volume)
        {
            //if (slider.value == SettingsManager.volume) return;
            slider.value = SettingsManager.volume;
            if (SceneManager.GetActiveScene().name == "TowerDefenseMenu" || SceneManager.GetActiveScene().name == "TowerDefenseMenuWEBGL")
            {
                AudioPlayer.Instance.GetCurrentTrack().volume = SettingsManager.volume;
            }
        } else if (type == SettingTypes.ScrollSensitivity)
        {
            //if (slider.value == SettingsManager.scrollSensitivity) return;
            slider.value = SettingsManager.scrollSensitivity;
        }
        print("imported a slider");
        set = true;
    }

    public void OnChangeSliderValue()
    {
        if (type == SettingTypes.Volume)
        {
            SettingsManager.volume = slider.value;
            if (SceneManager.GetActiveScene().name == "TowerDefenseMenu" || SceneManager.GetActiveScene().name == "TowerDefenseMenuWEBGL")
            {
                AudioPlayer.Instance.GetCurrentTrack().volume = slider.value;
            }
            SettingsManager.SaveSettings();
        } else if (type == SettingTypes.ScrollSensitivity)
        {
            SettingsManager.scrollSensitivity = slider.value;
            print(slider.value);
            SettingsManager.SaveSettings();
        }
    }
}
