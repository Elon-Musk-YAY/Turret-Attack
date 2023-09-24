using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class AudioFade
{
    public static IEnumerator FadeOut(AudioSource sound, float fadingTime, Func<float, float, float, float> Interpolate)
    {
        sound.volume = SettingsManager.volume;
        float startVolume = sound.volume;
        float frameCount = fadingTime / Time.deltaTime;
        float framesPassed = 0;
        while (framesPassed <= frameCount)
        {
            var t = framesPassed++ / frameCount;
            sound.volume = Interpolate(startVolume, 0, t);
            yield return null;
        }

        sound.volume = 0;
        sound.Pause();
        if (SceneManager.GetActiveScene().name == "TowerDefenseMain" || SceneManager.GetActiveScene().name == "TowerDefenseMainWEBGL")
        {
            AudioPlayer.Instance.coroDone = false;
        }
    }
    public static IEnumerator FadeIn(AudioSource sound, float fadingTime, Func<float, float, float, float> Interpolate)
    {
        sound.volume = 0;
        sound.UnPause();

        float resultVolume = SettingsManager.volume;
        float frameCount = fadingTime / Time.deltaTime;
        float framesPassed = 0;

        while (framesPassed <= frameCount)
        {
            var t = framesPassed++ / frameCount;
            sound.volume = Interpolate(0, resultVolume, t);
            yield return null;
        }

        sound.volume = resultVolume;
        if (sound.volume != resultVolume || !sound.isPlaying)
        {
            sound.Play();
            sound.volume = SettingsManager.volume;
        }
        if (SceneManager.GetActiveScene().name == "TowerDefenseMain" || SceneManager.GetActiveScene().name == "TowerDefenseMainWEBGL")
        {
            AudioPlayer.Instance.coroDone = true;
        }
    }
}