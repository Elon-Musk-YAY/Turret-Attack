using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsToggle : MonoBehaviour
{

    public enum SettingTypes {
        Particles,
        Glowing,
        FPSCounter
    }

    public Text onBtn;
    public Text offBtn;
    public Color disabledColor;
    public Color normalColor;
    public SettingTypes type;

    public void Update()
    {
        if (type == SettingTypes.Glowing) {
            if (SettingsManager.glow) {
                offBtn.color = disabledColor;
                onBtn.color = normalColor;
            }
            else {
                onBtn.color = disabledColor;
                offBtn.color = normalColor;
            }
        } else if (type == SettingTypes.Particles) {
            if (SettingsManager.particles == ParticleSettingTypes.OFF)
            {
                onBtn.color = disabledColor;
                offBtn.color = normalColor;
                
            }
            else if (SettingsManager.particles == ParticleSettingTypes.ALL)
            {
                offBtn.color = disabledColor;
                onBtn.color = normalColor;
            }
        } else if (type == SettingTypes.FPSCounter)
        {
            if (SettingsManager.showFPS)
            {
                offBtn.color = disabledColor;
                onBtn.color = normalColor;
                if (SceneManager.GetActiveScene().name != "TowerDefenseMenu" && SceneManager.GetActiveScene().name != "TowerDefenseMenuWEBGL")
                {
                    FPSCounter.Instance.enabled = true;
                }
            }
            else
            {
                onBtn.color = disabledColor;
                offBtn.color = normalColor;
                if (SceneManager.GetActiveScene().name != "TowerDefenseMenu" && SceneManager.GetActiveScene().name != "TowerDefenseMenuWEBGL")
                    FPSCounter.Instance.enabled = false;
            }
        }

    }

    public void ToggleSetting()
    {
        if (type == SettingTypes.Glowing) {
            SettingsManager.glow = !SettingsManager.glow;
            if (SettingsManager.glow)
            {
                offBtn.color = disabledColor;
                onBtn.color = normalColor;
            }
            else
            {
                onBtn.color = disabledColor;
                offBtn.color = normalColor;
            }
        }
        else if (type == SettingTypes.FPSCounter)
        {
            SettingsManager.showFPS = !SettingsManager.showFPS;
            if (SettingsManager.showFPS)
            {
                offBtn.color = disabledColor;
                onBtn.color = normalColor;
                if (SceneManager.GetActiveScene().name != "TowerDefenseMenu" && SceneManager.GetActiveScene().name!= "TowerDefenseMenuWEBGL")
                    FPSCounter.Instance.enabled = true;
            }
            else
            {
                onBtn.color = disabledColor;
                offBtn.color = normalColor;
                if (SceneManager.GetActiveScene().name != "TowerDefenseMenu" && SceneManager.GetActiveScene().name != "TowerDefenseMenuWEBGL")
                    FPSCounter.Instance.enabled = false;
            }
        }
        //Debug.Log("saving settings");
        SettingsManager.SaveSettings();
    }

    public void ChangeToOff() {
        SettingsManager.particles = ParticleSettingTypes.OFF;
        //Debug.Log("saving settings");
        SettingsManager.SaveSettings();
    }

    public void ChangeToAll() {
        SettingsManager.particles = ParticleSettingTypes.ALL;
        //Debug.Log("saving settings");
        SettingsManager.SaveSettings();
    }
}
