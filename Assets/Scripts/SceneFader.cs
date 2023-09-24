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
        if (img.color.a != 1.0f && img.color.a != 0.0f) {
            return;
        }
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
        SceneManager.LoadScene(scene);
    }
}
