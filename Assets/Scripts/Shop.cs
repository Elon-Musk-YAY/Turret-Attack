using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour

{
    public TurretBlueprint standardTurret;
    public TurretBlueprint missleLauncher;
    public TurretBlueprint laserBeamer;
    public TurretBlueprint forceFieldLauncher;
    public TurretBlueprint hardcoreTower;
    public TurretBlueprint spiralTurret;
    private BuildManager buildManager;
    public Text standardCostText;
    public Text missleCostText;
    public Text laserCostText;
    public Text auraCostText;
    public Text bufferCostText;
    public Text spiralCostText;
    public Text standardAmtText;
    public Text missleAmtText;
    public Text laserAmtText;
    public Text auraAmtText;
    public Text bufferAmtText;
    public Text spiralAmtText;
    public static long standardBaseCost;
    public static long missleBaseCost;
    public static long laserBaseCost;
    public static long auraBaseCost;
    public static long bufferBaseCost;
    public static long spiralBaseCost;
    public Image standardTurretImage;
    public Image missleLauncherImage;
    public Image laserBeamerImage;
    public Image forceFieldLauncherImage;
    public Image hardcoreTowerImage;
    public Image spiralTurretImage;
    public Color selectedColor;
    public Color normalColor = new Color(255, 255, 255);

    public static Shop Instance;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than 1 Shop in scene");
            return;
        }
        Instance = this;
        standardTurret.baseCost = standardTurret.cost;
        missleLauncher.baseCost = missleLauncher.cost;
        laserBeamer.baseCost = laserBeamer.cost;
        forceFieldLauncher.baseCost = forceFieldLauncher.cost;
        hardcoreTower.baseCost = hardcoreTower.cost;
        spiralTurret.baseCost = spiralTurret.cost;

    }
    public void SelectStandardTurret()
    {
        if (buildManager.GetTurretToBuild() == standardTurret)
        {
            buildManager.ClearTurretToBuild();
            standardTurretImage.color = normalColor;
            return;
        }
        buildManager.SelectTurretToBuild(standardTurret);
        standardTurretImage.color = selectedColor;
        missleLauncherImage.color = normalColor;
        laserBeamerImage.color = normalColor;
        forceFieldLauncherImage.color = normalColor;
        hardcoreTowerImage.color = normalColor;
        spiralTurretImage.color = normalColor;
    }

    public void SelectMissleTurret()
    {
        if (buildManager.GetTurretToBuild() == missleLauncher)
        {
            buildManager.ClearTurretToBuild();
            missleLauncherImage.color = normalColor;
            return;
        }
        buildManager.SelectTurretToBuild(missleLauncher);
        missleLauncherImage.color = selectedColor;
        standardTurretImage.color = normalColor;
        hardcoreTowerImage.color = normalColor;
        laserBeamerImage.color = normalColor;
        forceFieldLauncherImage.color = normalColor;
        spiralTurretImage.color = normalColor;
    }

    public void SelectLaserTurret()
    {
        if (buildManager.GetTurretToBuild() == laserBeamer)
        {
            buildManager.ClearTurretToBuild();
            laserBeamerImage.color = normalColor;
            return;
        }
        buildManager.SelectTurretToBuild(laserBeamer);
        laserBeamerImage.color = selectedColor;
        missleLauncherImage.color = normalColor;
        hardcoreTowerImage.color = normalColor;
        standardTurretImage.color = normalColor;
        forceFieldLauncherImage.color = normalColor;
        spiralTurretImage.color = normalColor;
    }

    public void SelectForceFieldLauncher()
    {
        if (buildManager.GetTurretToBuild() == forceFieldLauncher)
        {
            buildManager.ClearTurretToBuild();
            forceFieldLauncherImage.color = normalColor;
            return;
        }
        buildManager.SelectTurretToBuild(forceFieldLauncher);
        forceFieldLauncherImage.color = selectedColor;
        missleLauncherImage.color = normalColor;
        hardcoreTowerImage.color = normalColor;
        laserBeamerImage.color = normalColor;
        standardTurretImage.color = normalColor;
        spiralTurretImage.color = normalColor;
    }
    public void SelectHardcoreTower()
    {
        if (buildManager.GetTurretToBuild() == hardcoreTower)
        {
            buildManager.ClearTurretToBuild();
            hardcoreTowerImage.color = normalColor;
            return;
        }
        buildManager.SelectTurretToBuild(hardcoreTower);
        hardcoreTowerImage.color = selectedColor;
        missleLauncherImage.color = normalColor;
        laserBeamerImage.color = normalColor;
        forceFieldLauncherImage.color = normalColor;
        standardTurretImage.color = normalColor;
        spiralTurretImage.color = normalColor;
    }
    public void SelectSpiralTurret()
    {
        if (buildManager.GetTurretToBuild() == spiralTurret)
        {
            buildManager.ClearTurretToBuild();
            spiralTurretImage.color = normalColor;
            return;
        }
        buildManager.SelectTurretToBuild(spiralTurret);
        spiralTurretImage.color = selectedColor;
        missleLauncherImage.color = normalColor;
        laserBeamerImage.color = normalColor;
        forceFieldLauncherImage.color = normalColor;
        standardTurretImage.color = normalColor;
        hardcoreTowerImage.color = normalColor;
    }
    private void Update()
    {

        standardBaseCost = (long)(standardTurret.baseCost * StatsManager.standardTurretMultiplier);
        standardCostText.text = $"${GameManager.ShortenNumL(standardBaseCost)}";
        standardTurret.cost = standardBaseCost;

        missleBaseCost = (long)(missleLauncher.baseCost * StatsManager.missleLauncherMultiplier);
        missleCostText.text = $"${GameManager.ShortenNumL(missleBaseCost)}";
        missleLauncher.cost = missleBaseCost;

        laserBaseCost = (long)(laserBeamer.baseCost * StatsManager.laserBeamerMultiplier);
        laserCostText.text = $"${GameManager.ShortenNumL(laserBaseCost)}";
        laserBeamer.cost = laserBaseCost;

        auraBaseCost = (long)(forceFieldLauncher.baseCost * StatsManager.auraLauncherMultiplier);
        auraCostText.text = $"${GameManager.ShortenNumL(auraBaseCost)}";
        forceFieldLauncher.cost = auraBaseCost;

        bufferBaseCost = (long)(hardcoreTower.baseCost * StatsManager.bufferMultiplier);
        bufferCostText.text = $"${GameManager.ShortenNumL(bufferBaseCost)}";
        hardcoreTower.cost = bufferBaseCost;

        spiralBaseCost = (long)(spiralTurret.baseCost * StatsManager.spiralMultiplier);
        spiralCostText.text = $"${GameManager.ShortenNumL(spiralBaseCost)}";
        spiralTurret.cost = spiralBaseCost;

        standardAmtText.text = $"{StatsManager.remainingStandardTurretsAvailible}";
        missleAmtText.text = $"{StatsManager.remainingMissleLaunchersAvailible}";
        laserAmtText.text = $"{StatsManager.remainingLaserBeamersAvailible}";
        auraAmtText.text = $"{StatsManager.remainingAuraLaunchersAvailible}";
        bufferAmtText.text = $"{StatsManager.remainingBuffersAvailible}";
        spiralAmtText.text = $"{StatsManager.remainingSpiralTurretsAvailible}";

        if (StatsManager.remainingStandardTurretsAvailible == 0)
        {
            standardAmtText.transform.parent.parent.GetComponent<Button>().interactable = false;
            standardTurretImage.color = normalColor;
        }
        else
        {
            standardAmtText.transform.parent.parent.GetComponent<Button>().interactable = true;
        }
        if (StatsManager.remainingMissleLaunchersAvailible == 0)
        {
            missleAmtText.transform.parent.parent.GetComponent<Button>().interactable = false;
            missleLauncherImage.color = normalColor;
        }
        else
        {
            missleAmtText.transform.parent.parent.GetComponent<Button>().interactable = true;
        }
        if (StatsManager.remainingLaserBeamersAvailible == 0)
        {
            laserAmtText.transform.parent.parent.GetComponent<Button>().interactable = false;
            forceFieldLauncherImage.color = normalColor;
        }
        else
        {
            laserAmtText.transform.parent.parent.GetComponent<Button>().interactable = true;
        }
        if (StatsManager.remainingAuraLaunchersAvailible == 0)
        {
            auraAmtText.transform.parent.parent.GetComponent<Button>().interactable = false;
            forceFieldLauncherImage.color = normalColor;
        }
        else
        {
            auraAmtText.transform.parent.parent.GetComponent<Button>().interactable = true;
        }
        if (StatsManager.remainingBuffersAvailible == 0)
        {
            bufferAmtText.transform.parent.parent.GetComponent<Button>().interactable = false;
            hardcoreTowerImage.color = normalColor;
        }
        else
        {
            bufferAmtText.transform.parent.parent.GetComponent<Button>().interactable = true;
        }
        if (StatsManager.remainingSpiralTurretsAvailible == 0)
        {
            spiralAmtText.transform.parent.parent.GetComponent<Button>().interactable = false;
            spiralTurretImage.color = normalColor;
        }
        else
        {
            spiralAmtText.transform.parent.parent.GetComponent<Button>().interactable = true;
        }
    }

    private void Start()
    {
        buildManager = BuildManager.Instance;
    }

    public TurretBlueprint GetBlueprintByID(int id)
    {
        if (standardTurret.id == id)
        {
            return standardTurret;
        }
        else if (missleLauncher.id == id)
        {
            return missleLauncher;
        }
        else if (laserBeamer.id == id)
        {
            return laserBeamer;
        }
        else if (forceFieldLauncher.id == id)
        {
            return forceFieldLauncher;
        }
        else if (hardcoreTower.id == id)
        {
            return hardcoreTower;
        }
        else if (spiralTurret.id == id)
        {
            return spiralTurret;
        }
        return null;
    }
}

