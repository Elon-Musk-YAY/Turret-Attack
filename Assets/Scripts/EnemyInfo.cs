using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class EnemyInfo : MonoBehaviour
{
    [Header("Main")]
    public GameObject postProcessVolume;
    public GameObject ui;
    [Header("Descriptions")]
    [TextArea(3,10)]
    public string NormalInfo;
    [TextArea(3, 10)]
    public string ToughInfo;
    [TextArea(3, 10)]
    public string FastInfo;
    [TextArea(3, 10)]
    public string MiniBossInfo;
    [TextArea(3, 10)]
    public string MegaBossInfo;

    [Header("Info GameObjects")]
    public Text infoTitle;
    public Text infoText;
    public Text statsText;
    public Text boosterText;
    public Image infoImage;

    [Header("Enemy Images")]
    public Sprite normalImage;
    public Sprite toughImage;
    public Sprite fastImage;
    public Sprite miniBossImage;
    public Sprite megaBossImage;

    private bool hasChangedOriginalStats = false;
    public void OpenEnemyInfoMenu()
    {
        ui.SetActive(true);
        StartCoroutine(ToggleBlur());
    }
    public void CloseEnemyInfoMenu()
    {
        ui.SetActive(false);
        StartCoroutine(ToggleBlur());
    }
    IEnumerator ToggleBlur()
    {
        postProcessVolume.GetComponent<Volume>().profile.TryGet(out DepthOfField d);
        d.active = !d.active;
        yield return null;
    }
    private void Update()
    {
        if (!hasChangedOriginalStats && statsText != null)
        {
            Transform prefab = WaveSpawner.instance.standardEnemyPrefab;
            statsText.text =
            $"Start Health: {(int)(prefab.GetComponent<Enemy>().startHealth * WaveSpawner.instance.enemyHealth)}\n" +
            $"Start Speed: {(int)(prefab.GetComponent<Enemy>().startSpeed * WaveSpawner.instance.enemySpeed)}\n" +
            $"Gain From Death: ${(int)(prefab.GetComponent<Enemy>().worth * WaveSpawner.instance.enemyWorth)}";
            hasChangedOriginalStats = true;
        }
        boosterText.text = $"Enemy Health Multiplier: {WaveSpawner.instance.enemyHealth:0.00}x\n" +
            $"Enemy Speed Multiplier: {WaveSpawner.instance.enemySpeed:0.00}x\n" +
            $"Enemy Gain Multiplier: {WaveSpawner.instance.enemyWorth:0.00}x";
        
    }
    public void OpenNormalInfo()
    {
        infoImage.sprite = normalImage;
        Transform prefab = WaveSpawner.instance.standardEnemyPrefab;
        infoTitle.text = "The Normal Enemy";
        infoText.text = NormalInfo;
        statsText.text =
            $"Start Health: {(int)(prefab.GetComponent<Enemy>().startHealth * WaveSpawner.instance.enemyHealth)}\n" +
            $"Start Speed: {(int)(prefab.GetComponent<Enemy>().startSpeed * WaveSpawner.instance.enemySpeed)}\n" +
            $"Gain From Death: ${(int)(prefab.GetComponent<Enemy>().worth * WaveSpawner.instance.enemyWorth)}";
    }
    public void OpenToughInfo()
    {
        infoImage.sprite = toughImage;
        Transform prefab = WaveSpawner.instance.toughEnemyPrefab;
        infoTitle.text = "The Tough Enemy";
        infoText.text = ToughInfo;
        statsText.text =
            $"Start Health: {(int)(prefab.GetComponent<Enemy>().startHealth * WaveSpawner.instance.enemyHealth)}\n" +
            $"Start Speed: {(int)(prefab.GetComponent<Enemy>().startSpeed * WaveSpawner.instance.enemySpeed)}\n" +
            $"Gain From Death: ${(int)(prefab.GetComponent<Enemy>().worth * WaveSpawner.instance.enemyWorth)}";
    }
    public void OpenFastInfo()
    {
        infoImage.sprite = fastImage;
        Transform prefab = WaveSpawner.instance.fastEnemyPrefab;
        infoTitle.text = "The Speedy Enemy";
        infoText.text = FastInfo;
        statsText.text =
            $"Start Health: {(int)(prefab.GetComponent<Enemy>().startHealth * WaveSpawner.instance.enemyHealth)}\n" +
            $"Start Speed: {(int)(prefab.GetComponent<Enemy>().startSpeed * WaveSpawner.instance.enemySpeed)}\n" +
            $"Gain From Death: ${(int)(prefab.GetComponent<Enemy>().worth * WaveSpawner.instance.enemyWorth)}";
    }
    public void OpenMiniBossInfo()
    {
        infoImage.sprite = miniBossImage;
        Transform prefab = WaveSpawner.instance.miniBossPrefab;
        infoTitle.text = "The Mini Boss";
        infoText.text = MiniBossInfo;
        statsText.text =
            $"Start Health: {(int)(prefab.GetComponent<Enemy>().startHealth * WaveSpawner.instance.enemyHealth)}\n" +
            $"Start Speed: {(int)(prefab.GetComponent<Enemy>().startSpeed * WaveSpawner.instance.enemySpeed)}\n" +
            $"Gain From Death: ${(int)(prefab.GetComponent<Enemy>().worth * WaveSpawner.instance.enemyWorth)}";
    }
    public void OpenMegaBossInfo()
    {
        infoImage.sprite = megaBossImage;
        Transform prefab = WaveSpawner.instance.megaBossPrefab;
        infoTitle.text = "The Mega Boss";
        infoText.text = MegaBossInfo;
        statsText.text =
            $"Start Health: {(int)(prefab.GetComponent<Enemy>().startHealth * WaveSpawner.instance.enemyHealth)}\n" +
            $"Start Speed: {(int)(prefab.GetComponent<Enemy>().startSpeed * WaveSpawner.instance.enemySpeed)}\n" +
            $"Gain From Death: ${(int)(prefab.GetComponent<Enemy>().worth * WaveSpawner.instance.enemyWorth)}";
    }
}
