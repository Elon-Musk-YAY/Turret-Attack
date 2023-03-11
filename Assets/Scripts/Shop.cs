using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour

{
    public TurretBlueprint standardTurret;
    public TurretBlueprint missleLauncher;
    public TurretBlueprint laserBeamer;
    public TurretBlueprint forceFieldLauncher;
    public TurretBlueprint hardcoreTower;
    private BuildManager buildManager;
    public Image standardTurretImage;
    public Image missleLauncherImage;
    public Image laserBeamerImage;
    public Image forceFieldLauncherImage;
    public Image hardcoreTowerImage;
    public Color selectedColor;
    public Color normalColor = new Color(255,255,255);

    public static Shop instance;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than 1 Shop in scene");
            return;
        }
        instance = this;

    }
    public void SelectStandardTurret()
    {
        buildManager.SelectTurretToBuild(standardTurret);
        standardTurretImage.color = selectedColor;
        missleLauncherImage.color = normalColor;
        laserBeamerImage.color = normalColor;
        forceFieldLauncherImage.color = normalColor;
    }

    public void SelectMissleTurret()
    {
        buildManager.SelectTurretToBuild(missleLauncher);
        missleLauncherImage.color = selectedColor;
        standardTurretImage.color = normalColor;
        hardcoreTowerImage.color = normalColor;
        laserBeamerImage.color = normalColor;
        forceFieldLauncherImage.color = normalColor;
    }

    public void SelectLaserTurret()
    {
        buildManager.SelectTurretToBuild(laserBeamer);
        laserBeamerImage.color = selectedColor;
        missleLauncherImage.color = normalColor;
        hardcoreTowerImage.color = normalColor;
        standardTurretImage.color = normalColor;
        forceFieldLauncherImage.color = normalColor;
    }

    public void SelectForceFieldLauncher()
    {
        buildManager.SelectTurretToBuild(forceFieldLauncher);
        forceFieldLauncherImage.color = selectedColor;
        missleLauncherImage.color = normalColor;
        hardcoreTowerImage.color = normalColor;
        laserBeamerImage.color = normalColor;
        standardTurretImage.color = normalColor;
    }
    public void SelectHardcoreTower()
    {
        buildManager.SelectTurretToBuild(hardcoreTower);
        hardcoreTowerImage.color = selectedColor;
        missleLauncherImage.color = normalColor;
        laserBeamerImage.color = normalColor;
        forceFieldLauncherImage.color = normalColor;
        standardTurretImage.color = normalColor;
    }

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    public TurretBlueprint GetBlueprintByID(int id)
    {
        if (standardTurret.id == id)
        {
            return standardTurret;
        } else if (missleLauncher.id == id)
        {
            return missleLauncher;
        } else if (laserBeamer.id == id)
        {
            return laserBeamer;
        } else if (forceFieldLauncher.id == id)
        {
            return forceFieldLauncher;
        } else if (hardcoreTower.id == id)
        {
            return hardcoreTower;
        }
        return null;
    }
}

