using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyHealthBarPool : MonoBehaviour
{

	public GameObject poolObject;

	[System.Serializable]
	public class Pool
	{
		public string tag;
		public GameObject prefab;
	}

	public static EnemyHealthBarPool Instance;


    private void Awake()
    {
		Instance = this;
    }

    public Pool mainPool;
	public Queue<GameObject> objectPool;
    public GameObject finalBossHlth;
    // Use this for initialization
    void Start()
	{
		CreateObjects();
	}


	void CreateObjects()
	{
		objectPool = new Queue<GameObject>();
		for (int i = 0; i< WaveSpawner.Instance.maxEnemiesPerWave; i++)
		{
			GameObject obj = Instantiate(mainPool.prefab);
			obj.SetActive(false);
			obj.transform.SetParent(poolObject.transform,false);
			objectPool.Enqueue(obj);
		} 
	}

	public GameObject SpawnFromPool(Vector3 position, Quaternion rotation,bool withHealthText = false)
	{
		GameObject objToSpawn = objectPool.Dequeue();

		objToSpawn.SetActive(true);
        objToSpawn.transform.SetPositionAndRotation(position, rotation);
		objectPool.Enqueue(objToSpawn);
		return objToSpawn;
	}
}

