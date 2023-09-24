using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class EnemyInfo : MonoBehaviour
{
    [Header("Main")]
    public GameObject postProcessVolume;
    public CanvasGroup uiAlpha;

    [Header("Info GameObjects")]
    public Text infoTitle;
    public Text infoText;
    public Text statsText;
    public Text boosterText;
    public RawImage infoImage;

    public ScrollRect view;


    [SerializeField]
    private List<EnemySetup>  blocks = new();

    public UIScreen enemyInfoUIScreen;

    public void OpenEnemyInfoMenu()
    {
        StartCoroutine(ToggleBlur());
        uiAlpha.gameObject.SetActive(true);
        uiAlpha.LeanAlpha(1, 0.5f).setEaseInOutQuart();
        enemyInfoUIScreen.FadeIn(delay: 0.5f);
        foreach (Node node in GameManager.nodes.GetComponentsInChildren<Node>())
        {
            node.enabled = false;
        }
        GameManager.Instance.gameCamera.GetComponent<CameraController>().enabled = false;
    }



    public void CloseEnemyInfoMenu()
    {
        enemyInfoUIScreen.FadeOut(delay: 0f);
        uiAlpha.LeanAlpha(0, 0.5f).setEaseInOutQuart().setDelay(0.5f).setOnComplete(() =>
        {

            uiAlpha.gameObject.SetActive(false);
            foreach (Node node in GameManager.nodes.GetComponentsInChildren<Node>())
            {
                node.enabled = true;
            }
            GameManager.Instance.gameCamera.GetComponent<CameraController>().enabled = true;
            //enemyInfoShallow.SetActive(false);
        });
        StartCoroutine(ToggleBlur());
        
    }
    IEnumerator ToggleBlur()
    {
        postProcessVolume.GetComponent<Volume>().profile.TryGet(out DepthOfField d);
        print(d);
        d.active = !d.active;
        yield return null;
    }
    private void Start()
    {
        foreach (EnemySetup ei in WaveSpawner.Instance.enemies)
        {
            if (ei.waveToStartSpawning != 0)
            {
                blocks.Add(ei);
            }
        }

        enemyInfoUIScreen.SetAllTo(0f);
        uiAlpha.alpha = 0;
        //enemyInfoShallow.SetActive(false);

        //WaveSpawner.Instance.enemies[0].texture.
    }

    private void Update()
    {

        view.scrollSensitivity = SettingsManager.scrollSensitivity * 20;
        foreach (EnemySetup block in blocks)
        {
            if (WaveSpawner.Instance.waveIndex /2 < block.waveToStartSpawning)
            {
                block.referenceBlock.SetActive(false);
            } else
            {
                block.referenceBlock.SetActive(true);
            }
        }
        if (!uiAlpha.gameObject.activeSelf && statsText != null)
        {
            OpenInfo(0);
        }
        boosterText.text = $"Enemy Health Multiplier: {GameManager.ShortenNum(WaveSpawner.Instance.enemyHealth):0.00}x\n" +
            $"Enemy Speed Multiplier: {GameManager.ShortenNum(WaveSpawner.Instance.enemySpeed):0.00}x\n" +
            $"Enemy Gain Multiplier: {GameManager.ShortenNum(WaveSpawner.Instance.enemyWorth):0.00}x";

    }

    int current = 0;

    public void OpenInfo(int index)
    {
        current = index;
        infoImage.texture = WaveSpawner.Instance.enemies[index].texture;
        Transform prefab = WaveSpawner.Instance.enemies[index].transform;
        Enemy pE = prefab.GetComponent<Enemy>();
        infoTitle.text = WaveSpawner.Instance.enemies[index].name;
        infoText.text = WaveSpawner.Instance.enemies[index].info;
        statsText.text =
            $"Start Health: {GameManager.ShortenNumD(System.Math.Round(pE.startHealth * WaveSpawner.Instance.enemyHealth))}\n" +
            $"Start Speed: {(int)(pE.startSpeed * WaveSpawner.Instance.enemySpeed)}\n" +
            $"Gain From Death: ${GameManager.ShortenNumD(System.Math.Round(pE.worth * WaveSpawner.Instance.enemyWorth))}\n"+
            $"Enemy Type: {pE.enemyType}";
    }
}
