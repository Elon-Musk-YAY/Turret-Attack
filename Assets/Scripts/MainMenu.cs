using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad = "TowerDefenseMain";
    public SceneFader sceneFader;
    public GameObject titleScreen;
    public GameObject settingsScreen;
    public GameObject howToPlayScreen;
    public GameObject postProcessVolume;


    public void Play()
    {
        sceneFader.FadeTo(levelToLoad);
    }



    public void Quit()
    {
        Application.Quit();
    }

    public void Settings()
    {
        settingsScreen.SetActive(true);
        StartCoroutine(ToggleBlur());
        titleScreen.SetActive(false);
    }

    public void HowToPlay()
    {
        howToPlayScreen.SetActive(true);
        StartCoroutine(ToggleBlur());
        titleScreen.SetActive(false);
    }
    IEnumerator ToggleBlur()
    {
        postProcessVolume.GetComponent<Volume>().profile.TryGet<DepthOfField>(out DepthOfField d);
        yield return new WaitForSeconds(0.4f);
        d.active = true;
    }
    public void Back()
    {
        settingsScreen.SetActive(false);
        howToPlayScreen.SetActive(false);
        postProcessVolume.GetComponent<Volume>().profile.TryGet<DepthOfField>(out DepthOfField d);
        d.active = false;
        titleScreen.SetActive(true);
    }
}
