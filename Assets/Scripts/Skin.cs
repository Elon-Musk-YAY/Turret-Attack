using UnityEngine;
[System.Serializable]
public class Skin : MonoBehaviour
{
	public string skinName;
	public int skinID;
	[Header("Standard Turret")]
	public GameObject standardTurretPrefab;
	public GameObject standardTurretPrefabUpgraded;
	[Space(10)]
	[Header("Missle Launcher")]
	public GameObject missleLauncherPrefab;
	public GameObject missleLauncherPrefabUpgraded;
	[Space(10)]
	[Header("Laser Beamer")]
	public GameObject laserBeamerPrefab;
	public GameObject laserBeamerPrefabUpgraded;
	[Space(10)]
	[Header("Force Field Launcher")]
	public GameObject forceFieldLauncherPrefab;
	public GameObject forceFieldLauncherPrefabUpgraded;


}

