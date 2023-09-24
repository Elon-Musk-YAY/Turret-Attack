
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    

    //public string levelToLoad = "TowerDefenseMain";
    public SceneFader sceneFader;
    public CanvasGroup titleAlpha;
    public CanvasGroup settingsAlpha;
    public GameObject postProcessVolume;
    public UIScreen settingScreenUI;
    public void Play()
    {
        StartCoroutine(AudioFade.FadeOut(AudioPlayer.Instance.GetCurrentTrack(), 0.5f, Mathf.SmoothStep));
#if UNITY_WEBGL
        sceneFader.FadeTo("TowerDefenseMainWEBGL");
#else
sceneFader.FadeTo("TowerDefenseMain");
#endif

    }

    private void CopyFilesRecursively(string sourcePath, string targetPath)
    {
        //Now Create all of the directories
        foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
        }

        //Copy all the files & Replaces any files with the same name
        foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
        {
            File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
        }
    }



    public void Quit()
    {
        Application.Quit();
    }

    private void FadeInOnStart()
    {
        titleAlpha.LeanAlpha(1, 1.5f).setEaseInOutQuart();
    }

    private void Start()
    {
        Invoke(nameof(FadeInOnStart), 1f);
        settingScreenUI.SetAllTo(0f);
        settingsAlpha.alpha = 0;
    }


    public void Settings()
    {
        titleAlpha.LeanAlpha(0, 0.5f).setEaseInOutQuart().setOnComplete(() =>
        {
            titleAlpha.gameObject.SetActive(false); settingsAlpha.gameObject.SetActive(true);
            settingsAlpha.LeanAlpha(1, 0.5f).setEaseInOutQuart().setOnComplete(() =>
            {
                settingScreenUI.FadeIn();
            });
        });

        //settingsScreen.SetActive(true);
        StartCoroutine(ToggleBlur());

    }
    IEnumerator ToggleBlur()
    {
        postProcessVolume.GetComponent<Volume>().profile.TryGet<DepthOfField>(out DepthOfField d);
        yield return new WaitForSeconds(0.4f);
        d.active = true;
    }
    public void Back()
    {
        settingScreenUI.FadeOut();
        settingsAlpha.LeanAlpha(0, 0.5f).setOnComplete(() =>
        {
            titleAlpha.gameObject.SetActive(true);
            titleAlpha.LeanAlpha(1, 0.5f).setEaseInOutQuart().setOnComplete(() => settingsAlpha.gameObject.SetActive(false));
        }).setDelay(0.5f);

        postProcessVolume.GetComponent<Volume>().profile.TryGet<DepthOfField>(out DepthOfField d);
        d.active = false;
    }
}
