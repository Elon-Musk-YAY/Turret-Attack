using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField]private AudioSource[] tracks;
    
    private int currentIndex = 0;
    [HideInInspector]
    public bool coroDone = false;
    public static AudioPlayer Instance;
    public AudioSource bossMusic;
    // Start is called before the first frame update
    private void Awake()
    {
        currentIndex = Random.Range(0, tracks.Length);
        GetCurrentTrack().volume = SettingsManager.volume;
        GetCurrentTrack().Play();
        Instance = this;
    }
    

    public AudioSource GetCurrentTrack()
    {
        //if (SceneManager.GetActiveScene().name == "TowerDefenseMain") {
        //    if (PlayerStats.Rounds == WaveSpawner.Instance.finalRoundNum)
        //    {
        //        return tracks[currentIndex];
        //    } else
        //    {
        //        return bossMusic;
        //    }
        //}
        //else
        //{
            return tracks[currentIndex];
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "TowerDefenseMain" || SceneManager.GetActiveScene().name == "TowerDefenseMainWEBGL")
        {
            //if (PlayerStats.Rounds == WaveSpawner.Instance.finalRoundNum && !bossMusic.isPlaying && Time.timeScale != 0f)
            //{
            //    StartCoroutine(AudioFade.FadeOut(GetCurrentTrack(), 0.5f, Mathf.SmoothStep));
            //    StartCoroutine(FadeInBossMusic());
            //}
            //else if ((PlayerStats.Rounds == WaveSpawner.Instance.finalRoundNum + 1 || !GameManager.win) && bossMusic.isPlaying && Time.timeScale != 0f)
            //{
            //    StartCoroutine(AudioFade.FadeOut(bossMusic, 0.5f, Mathf.SmoothStep));
            //    currentIndex++;
            //    if (tracks.Length == currentIndex)
            //    {
            //        currentIndex = 0;
            //    }
            //    StartCoroutine(FadeInNormalMusic());

            //}
        }
        if (GetCurrentTrack().volume != 0f && !GetCurrentTrack().isPlaying)
        {
            GetCurrentTrack().UnPause();
        }
        if (GetCurrentTrack().volume != SettingsManager.volume && !GetCurrentTrack().isPlaying && Time.timeScale != 0 && coroDone)
        {
            GetCurrentTrack().volume = SettingsManager.volume;
        }
        //if (Time.timeScale != 0f && (GetCurrentTrack().volume == 0f || !GetCurrentTrack().isPlaying) && !coroDone)
        //{
        //    coroDone = true;
        //    GetCurrentTrack().UnPause();
        //    GetCurrentTrack().volume = SettingsManager.volume;
        //    StartCoroutine(AudioFade.FadeIn(GetCurrentTrack(), 0.75f, Mathf.SmoothStep));
            
        //}
        if (GetCurrentTrack().time == GetCurrentTrack().clip.length)
        {
            print("new song");
            currentIndex++;
            if (tracks.Length == currentIndex)
            {
                currentIndex = 0;
            }
            GetCurrentTrack().volume = SettingsManager.volume;
            GetCurrentTrack().Play();
        }
    }
}
