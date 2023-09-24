using System.Collections;
using UnityEngine;

[System.Serializable]
public class TurretBlueprint
{
    // SET VALUES IN THE SHOP
    public GameObject prefab;
    public GameObject buildEffect;
    public GameObject upgradedPrefab;
    public int upgradeCost;
    public long cost;
    [HideInInspector]
    public long baseCost;
    public int id = 0;
}
