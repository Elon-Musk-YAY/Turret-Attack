using UnityEngine;
using UnityEngine.UI;

public class SettingsToggle : MonoBehaviour
{

    public enum SettingTypes {
        Particles,
        Glowing
    }

    public Text onBtn;
    public Text offBtn;
    public Color disabledColor;
    public Color normalColor;
    public SettingTypes type;

    public void Start()
    {
        if (type == SettingTypes.Glowing) {
            if (GraphicsManager.glow) {
                offBtn.color = disabledColor;
                onBtn.color = normalColor;
            }
            else {
                onBtn.color = disabledColor;
                offBtn.color = normalColor;
            }
        } else if (type == SettingTypes.Particles) {
            if (GraphicsManager.particles)
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

    }

    public void ToggleSetting()
    {
        if (type == SettingTypes.Glowing) {
            GraphicsManager.glow = !GraphicsManager.glow;
            if (GraphicsManager.glow)
            {
                offBtn.color = disabledColor;
                onBtn.color = normalColor;
            }
            else
            {
                onBtn.color = disabledColor;
                offBtn.color = normalColor;
            }
        } else if (type == SettingTypes.Particles) {
            GraphicsManager.particles = !GraphicsManager.particles;
            if (GraphicsManager.particles)
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

        GraphicsManager.SaveSettings();
    }
}
