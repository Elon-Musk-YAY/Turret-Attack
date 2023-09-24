using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than 1 BuildManager in scene");
            return;
        }
        Instance = this;

    }

    public void ClearTurretToBuild()
    {
        turretToBuild = null;
        Shop.Instance.standardTurretImage.color = Shop.Instance.normalColor;
        Shop.Instance.missleLauncherImage.color = Shop.Instance.normalColor;
        Shop.Instance.laserBeamerImage.color = Shop.Instance.normalColor;
        Shop.Instance.forceFieldLauncherImage.color = Shop.Instance.normalColor;
        Shop.Instance.hardcoreTowerImage.color = Shop.Instance.normalColor;
        Shop.Instance.spiralTurretImage.color = Shop.Instance.normalColor;
    }
    [System.Serializable]
    public struct TurretSilhoutte
    {
        public GameObject can;
        public GameObject cannot;
    }

    private TurretBlueprint turretToBuild;
    private Node selectedNode;
    public GameObject upgradeEffect;
    public NodeUI nodeUI;
    public GameObject sellEffect;
    private TurretBlueprint storedTurret;
    public BufferHealthBarModifier[] modifiers;
    public TurretSilhoutte standardSilhoutte;
    public TurretSilhoutte missleSilhoutte;
    public TurretSilhoutte laserSilhouette;
    public TurretSilhoutte auraSilhoutte;
    public TurretSilhoutte bufferSilhouette;
    public TurretSilhoutte spiralSilhoette;

    public ParticleSystem rangeEffectForNodes;


    public bool canBuild { get { return turretToBuild != null && Time.timeScale != 0f; } }
    public bool hasMoney { get { return PlayerStats.Money >= turretToBuild.cost; } }


    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
        storedTurret = turret;
        DeselectNode();
    }

    public TurretBlueprint GetTurretToBuild()
    {
        return turretToBuild;
    }

    public GameObject GetSilhouette(bool turret)
    {
        return storedTurret.id switch
        {
            0 => (hasMoney && !turret ? standardSilhoutte.can : standardSilhoutte.cannot),
            1 => (hasMoney && !turret ? missleSilhoutte.can : missleSilhoutte.cannot),
            2 => (hasMoney && !turret ? laserSilhouette.can : laserSilhouette.cannot),
            3 => (hasMoney && !turret ? auraSilhoutte.can : auraSilhoutte.cannot),
            4 => (hasMoney && !turret ? bufferSilhouette.can : bufferSilhouette.cannot),
            5 => (hasMoney && !turret ? spiralSilhoette.can : spiralSilhoette.cannot),
            _ => null,
        };
    }


    public void SelectNode(Node node)
    {
        if (selectedNode == node)
        {
            DeselectNode();
            return;
        }
        selectedNode = node;
        storedTurret = turretToBuild;
        turretToBuild = null;
        nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        selectedNode = null;
        turretToBuild = storedTurret;
        nodeUI.Hide();
    }

    private void Start()
    {
        rangeEffectForNodes.gameObject.SetActive(true);
        rangeEffectForNodes.Stop();
    }

}
