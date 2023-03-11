using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than 1 BuildManager in scene");
            return;
        }
        instance = this;

    }

    public void ClearTurretToBuild()
    {
        turretToBuild = null;
    }

    private TurretBlueprint turretToBuild;
    private Node selectedNode;
    public GameObject upgradeEffect;
    public NodeUI nodeUI;
    public GameObject sellEffect;
    private TurretBlueprint storedTurret;
    public Color buffedBG = new(8,0,255);
    public Color buffedHealthBG = new(192,214,15);


    public bool canBuild { get { return turretToBuild != null && Time.timeScale ==1f; } }
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

}
