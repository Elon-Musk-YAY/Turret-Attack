using UnityEngine;

[System.Serializable]
public class UIScreen
{
    public CanvasGroup[] canvasesToAnimate;

    public void FadeIn(float duration = 0.5f, float delay = 0.3f, System.Action onComplete=null)
    {
        bool hasSetOnComplete = false;
        foreach (CanvasGroup g in this.canvasesToAnimate)
        {
            if (!hasSetOnComplete)
            {
                hasSetOnComplete = true;
                g.LeanAlpha(1, duration).setEaseInOutQuart().setDelay(delay).setIgnoreTimeScale(true).setOnComplete(onComplete);
            }
            else
            {
                g.LeanAlpha(1, duration).setEaseInOutQuart().setDelay(delay).setIgnoreTimeScale(true);

            }
        }
    }

    public void FadeOut(float duration = 0.5f, float delay = 0f, System.Action onComplete=null)
    {
        bool hasSetOnComplete = false;
        foreach (CanvasGroup g in this.canvasesToAnimate)
        {
            if (!hasSetOnComplete)
            {
                hasSetOnComplete = true;
                g.LeanAlpha(0, duration).setEaseInOutQuart().setDelay(delay).setIgnoreTimeScale(true).setOnComplete(onComplete);
            }
            else
            {
                g.LeanAlpha(0, duration).setEaseInOutQuart().setDelay(delay).setIgnoreTimeScale(true);

            }
        }
    }

    public void SetAllTo(float a)
    {
        foreach (CanvasGroup g in this.canvasesToAnimate)
        {
            g.alpha = a;
        }
    }
}