using UnityEngine;
using System.IO;
using System.IO.Compression;

public class SaveManager : MonoBehaviour
{
	// Use this for initialization

	public string zipPath = "/Users/akshardesai/Desktop/Turret_Attack_Save.save";
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
			
	}


	public void ExportSave()
    {
		//Debug.LogError("run");
  //      StandaloneFileBrowser.SaveFilePanelAsync("Save File", "", "Turret_Attack_Save", "save", (string path) => {
  //          string savePath = Path.Combine(Application.persistentDataPath, "save0");
  //          ZipFile.CreateFromDirectory(savePath, path);
  //      });
        //string tempPath = Path.Combine(Application.temporaryCachePath, "Turret_Attack_Save.zip");
        
    }

	public void ImportSave()
    {
        //ZipFile.ExtractToDirectory(zipPath, extractPath);
    }
}

