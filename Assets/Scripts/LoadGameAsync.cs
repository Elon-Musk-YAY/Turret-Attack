using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadGameAsync : MonoBehaviour
{
    public string gameSceneName = "TowerDefenseMain";
    public static bool loadNewScene = false;
    AsyncOperation asyncLoad;
    public static LoadGameAsync instance;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    private void Awake()
    {
        instance = this;
    }


    public IEnumerator LoadGameScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        Debug.LogWarning("loading game scene asynchrnously");
        asyncLoad = SceneManager.LoadSceneAsync(gameSceneName);
        asyncLoad.allowSceneActivation = false;
        while (asyncLoad.progress < 0.9f) {
            Debug.LogError(asyncLoad.progress);
            yield return null;
        }
        Debug.LogWarning("done loadig");


    }

    public void OpenScene()
    {
        asyncLoad.allowSceneActivation = true;
    }



}

