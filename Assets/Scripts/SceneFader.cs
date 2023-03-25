using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    public Image img;
    public float fadeSpeed = 1f;
    public AnimationCurve fadeCurve;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }
    public IEnumerator FadeIn()
    {
        float t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime * fadeSpeed;
            float a = fadeCurve.Evaluate(t);
            img.color = new Color(0, 0, 0, a);
            yield return 0;
        }
    }

    public IEnumerator FadeOut( string scene)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            float a = fadeCurve.Evaluate(t);
            img.color = new Color(0, 0, 0, a);
            yield return 0;
        }
        if (scene == LoadGameAsync.instance.gameSceneName)
        {
            LoadGameAsync.instance.OpenScene();
            //SceneManager.LoadScene(scene);
        }
        else
        {
            SceneManager.LoadScene(scene);
        }
    }
}
