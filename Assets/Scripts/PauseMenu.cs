using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public string menuSceneName = "TowerDefenseMenu";
    private float originalTimeScale;
    public CanvasGroup uiAlpha;
    public UIScreen mainMenuScreenUI;
    public UIScreen settingScreenUI;
    public UIScreen statsScreenUI;
    public SceneFader sceneFader;
    public CanvasGroup statsAlpha;
    public CanvasGroup mainMenuAlpha;
    public CanvasGroup settingsAlpha;
    public Text statsText;
    private bool menuOpen = false;
    [SerializeField]
    private bool interactable = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && interactable)
        {
            if (!menuOpen)
            {
                OpenPauseMenu();
                menuOpen = true;
            } else
            {
                ClosePauseMenu();
            }
            DisableInteraction();
        }
    }

    IEnumerator g()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(AudioFade.FadeIn(AudioPlayer.Instance.GetCurrentTrack(), 0.75f, Mathf.SmoothStep));
    }

    private void Start()
    {
        uiAlpha.alpha = 0;
        mainMenuScreenUI.SetAllTo(0f);
        settingScreenUI.SetAllTo(0f);
        statsScreenUI.SetAllTo(0f);
    }

    IEnumerator EnableInteraction(float time=1f)
    {
        yield return new WaitForSecondsRealtime(time);
        interactable = true;
    }

    void DisableInteraction () {
        interactable = false;
    }


    public void OpenPauseMenu()
    {
        uiAlpha.gameObject.SetActive(true);
        uiAlpha.LeanAlpha(1, 0.5f).setEaseInOutQuart().setIgnoreTimeScale(true).setOnComplete( () => { StartCoroutine(EnableInteraction()); });
        mainMenuScreenUI.FadeIn(delay: 0.5f);
        StartCoroutine(AudioFade.FadeOut(AudioPlayer.Instance.GetCurrentTrack(), 0.5f, Mathf.SmoothStep));
        foreach (Node node in GameManager.nodes.GetComponentsInChildren<Node>())
        {
            node.enabled = false;
        }
        originalTimeScale = Time.timeScale;
        AudioPlayer.Instance.coroDone = false;
        Time.timeScale = 0f;
    }

    public void ClosePauseMenu(bool isGoingToMenu = false)
    {
        if (!interactable) return;
        interactable = false;
        mainMenuScreenUI.FadeOut(delay: 0f);
        uiAlpha.LeanAlpha(0, 0.5f).setEaseInOutQuart().setDelay(0.5f).setIgnoreTimeScale(true).setOnComplete(() =>
        {
            uiAlpha.gameObject.SetActive(false);
            Time.timeScale = originalTimeScale;
            if (!isGoingToMenu)
            {
                foreach (Node node in GameManager.nodes.GetComponentsInChildren<Node>())
                {
                    node.enabled = true;
                }
            }
            menuOpen = false;
            if (isGoingToMenu)
            {
                StartCoroutine(nameof(h));
                //WaveSpawner.enemiesAlive = 0;
                if (SceneManager.GetActiveScene().name == "TowerDefenseMain") { 
                sceneFader.FadeTo(menuSceneName);
                } else if (SceneManager.GetActiveScene().name == "TowerDefenseMainWEBGL")
                {
                    sceneFader.FadeTo(menuSceneName+"WEBGL");
                }
            }
            else
            {
                StartCoroutine(nameof(g));
                StartCoroutine(EnableInteraction());
            }
        });
    }

    public void Stats()
    {
        if (!interactable) return;
        interactable = false;
        mainMenuScreenUI.FadeOut(delay: 0f, onComplete: () =>
        {
            mainMenuAlpha.gameObject.SetActive(false);
            statsAlpha.gameObject.SetActive(true);
            statsScreenUI.FadeIn(delay: 0f);
            StartCoroutine(EnableInteraction(0.5f));
        });
        statsText.text = $"Damage Dealt: {GameManager.ShortenNumUL(StatsManager.damageDealtThisSession + StatsManager.importedDamageDealt)}\n" +
            $"Money Earned: {GameManager.ShortenNumUL(StatsManager.moneyEarnedThisSession + StatsManager.importedMoneyEarned)}\n" +
            $"Time Played: {StatsManager.GetFormattedPlayTime()}";
    }

    public void Settings()
    {
        if (!interactable) return;
        interactable = false;
        mainMenuScreenUI.FadeOut(delay: 0f,onComplete: () =>
        {
            mainMenuAlpha.gameObject.SetActive(false);
            settingsAlpha.gameObject.SetActive(true);
            settingScreenUI.FadeIn(delay: 0f);
            StartCoroutine(EnableInteraction(0.5f));
        });
    }

    public void BackToPauseMenu()
    {
        if (!interactable) return;
        interactable = false;
        if (settingsAlpha.gameObject.activeSelf)
        {
            // we are in settings screen currently
            settingScreenUI.FadeOut(delay: 0f, onComplete: () =>
            {
                settingsAlpha.gameObject.SetActive(false);
                mainMenuAlpha.gameObject.SetActive(true);
                mainMenuScreenUI.FadeIn(delay: 0f);
                StartCoroutine(EnableInteraction(0.5f));
            });
        } else
        {
            // we are in stats screen
            statsScreenUI.FadeOut(delay: 0f, onComplete: () =>
            {
                statsAlpha.gameObject.SetActive(false);
                mainMenuAlpha.gameObject.SetActive(true);
                mainMenuScreenUI.FadeIn(delay: 0f);
                StartCoroutine(EnableInteraction(0.5f));
            });
        }
    }

    IEnumerator h()
    {
        AudioPlayer.Instance.GetCurrentTrack().Play();
        AudioPlayer.Instance.GetCurrentTrack().volume = SettingsManager.volume;
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(AudioFade.FadeOut(AudioPlayer.Instance.GetCurrentTrack(), 0.6f, Mathf.SmoothStep));
    }
}
