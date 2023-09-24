using UnityEngine;

[System.Serializable]
public class EnemySetup
{
    [Tooltip("The transform of the enemy prefab")]
    public Transform transform;
    [Range(0f, 100f)]
    public float chanceToSpawn = 10f;

    public double _weight;

    [Tooltip("What wave can it start spawning at?")]
    public int waveToStartSpawning;
    [Tooltip("When will this enemy become a rare spawn?")]
    public int waveToStopSpawningFrequently;

    [Header("Enemy info stuff")]
    [Tooltip("THE DETAIL ONE")]
    public Texture texture;
    [Tooltip("The .... enemy")]
    public string name;

    [TextArea(3, 10)]
    public string info;

    [Tooltip("The block in the enemy info menu (only needed if waveToStartSpawning isn't 0)")]
    public GameObject referenceBlock;

    //public Camera deepCamera;
}