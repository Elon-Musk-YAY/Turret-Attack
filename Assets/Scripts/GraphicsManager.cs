using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GraphicsManager : MonoBehaviour
{
    public static bool glow = true;
    public static bool particles = true;
    public static GameSettings settings = new();


    public static void SaveSettings()
    {
        BinaryFormatter formatter = new();
        settings.glow = glow;
        settings.particles = particles;
        if (!File.Exists(SaveSystem.sPath)) {
            return;
        }
        FileStream settingsStream = new FileStream(SaveSystem.sPath, FileMode.Create);
        formatter.Serialize(settingsStream,settings);
        settingsStream.Close();
        settingsStream.Dispose();
        Debug.Log("Settings Saved");
    }

    private void Start()
    {
        GameSettings fileSettings;
        BinaryFormatter formatter = new();
        if (File.Exists(SaveSystem.sPath))
        {
            FileStream stream = new FileStream(SaveSystem.sPath, FileMode.Open);
            try
            {
                if (stream.Length == 0)
                {
                    SaveSettings();
                    stream.Dispose();
                    return;
                }
                object data = formatter.Deserialize(stream);
                fileSettings = data as GameSettings;
                glow = fileSettings.glow;
                particles = fileSettings.particles;
                Debug.LogError(particles);
            }
            catch (System.Exception e)
            {
                Debug.LogError("file is curroperted\n" + e + "\n" + stream);
            }
            stream.Dispose();
        }
        else
        {
            SaveSettings();
        }
        Debug.Log("Settings Loaded");
    }

        // Update is called once per frame
        void Update()
	{
        gameObject.GetComponent<Volume>().profile.TryGet<Bloom>(out Bloom bloomEffect);
        if (glow == false)
        {
            bloomEffect.active = false;
        } else
        {
            bloomEffect.active = true;
        }
        foreach (GameObject go in FindObjectsOfType<GameObject>())
        {
            if (go.activeInHierarchy)
            {
                if (go.GetComponent<VisualEffect>() != null && !particles)
                {
                    go.GetComponent<VisualEffect>().enabled = false;
                }
                else if (go.GetComponent<VisualEffect>() != null && particles)
                {
                    go.GetComponent<VisualEffect>().enabled = true;
                }
            } 
        }

    }
}

