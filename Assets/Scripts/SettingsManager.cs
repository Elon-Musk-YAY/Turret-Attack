using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{

    public static bool glow = true;
    public static float volume = 1f;
    public static bool showFPS = false;
    public static ParticleSettingTypes particles = ParticleSettingTypes.ALL;
    public static float scrollSensitivity = 0.5f;
    public static GameSettings settings = new();
    public static bool settingsImported = false;
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void SyncDB();
#endif


    public static void SaveSettings()
    {
        BinaryFormatter formatter = new();
        settings.glow = glow;
        settings.particles = particles;
        settings.volume = volume;
        settings.showFPS = showFPS;
        settings.scrollSensitivity = scrollSensitivity;
        if (File.Exists(SaveSystem.path) && !File.Exists(SaveSystem.sPath))
        {
            Debug.LogWarning("Setting Path Not Found");
            settings.glow = glow;
            settings.particles = particles;
            settings.volume = volume;
            settings.showFPS = showFPS;
            settings.scrollSensitivity = scrollSensitivity;
            FileStream ss = new FileStream(SaveSystem.sPath, FileMode.Create);
            formatter.Serialize(ss, settings);
            ss.Close();
            ss.Dispose();
#if UNITY_WEBGL && !UNITY_EDITOR
        //flush our changes to IndexedDB
        SyncDB();
#endif
            Debug.Log("Settings file created when saving   " + SaveSystem.sPath);
            return;
        }
        else if (!Directory.Exists(SaveSystem.savePath))
        {
            Directory.CreateDirectory(SaveSystem.savePath);
            settings.glow = glow;
            settings.particles = particles;
            settings.volume = volume;
            settings.showFPS = showFPS;
            settings.scrollSensitivity = scrollSensitivity;
            FileStream ss = new FileStream(SaveSystem.sPath, FileMode.Create);
            formatter.Serialize(ss, settings);
            ss.Close();
            ss.Dispose();
#if UNITY_WEBGL && !UNITY_EDITOR
        //flush our changes to IndexedDB
        SyncDB();
#endif
            Debug.Log("Settings file created   " + SaveSystem.sPath);
        }
        FileStream settingsStream = new FileStream(SaveSystem.sPath, FileMode.Create);
        formatter.Serialize(settingsStream, settings);
        settingsStream.Close();
        settingsStream.Dispose();
     
#if UNITY_WEBGL && !UNITY_EDITOR
        //flush our changes to IndexedDB
        SyncDB();
#endif
        Debug.Log("Settings Saved");
    }


    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "TowerDefenseMenu" || SceneManager.GetActiveScene().name == "TowerDefenseMenuWEBGL")
        {
            Import();
        }
    }

    public static bool All()
    {
        return (particles == ParticleSettingTypes.ALL);
    }
    public static void Import()
    {
        //if (gameObject.name == "special PP") return;
        GameSettings fileSettings;
        BinaryFormatter formatter = new();
        if (File.Exists(SaveSystem.sPath))
        {
            print("Settings Path Exists");
            FileStream stream = new FileStream(SaveSystem.sPath, FileMode.Open);
            try
            {
                if (stream.Length == 0)
                {
                    print("bad stream");
                    SaveSettings();
                    stream.Dispose();
                    return;
                }
                object data = formatter.Deserialize(stream);
                fileSettings = data as GameSettings;
                glow = fileSettings.glow;
                particles = fileSettings.particles;
                volume = fileSettings.volume;
                scrollSensitivity = fileSettings.scrollSensitivity;
                showFPS = fileSettings.showFPS;
            }
            catch (System.Exception e)
            {
                Debug.LogError("file is curroperted\n" + e + "\n" + stream);
            }
            stream.Close();
            stream.Dispose();
        }
        else if (File.Exists(SaveSystem.sPath))
        {
            settings.glow = glow;
            settings.particles = particles;
            settings.volume = volume;
            settings.showFPS = showFPS;
            settings.scrollSensitivity = scrollSensitivity;
            FileStream settingsStream = new FileStream(SaveSystem.sPath, FileMode.Create);
            formatter.Serialize(settingsStream, settings);
            settingsStream.Close();
            settingsStream.Dispose();
            Debug.Log("Settings file created   " + SaveSystem.sPath);
        }
        Debug.Log("Settings Loaded");
        settingsImported = true;
#if UNITY_WEBGL && !UNITY_EDITOR
        //flush our changes to IndexedDB
        SyncDB();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        Debug.LogWarning(scrollSensitivity);

        gameObject.GetComponent<Volume>().profile.TryGet<Bloom>(out Bloom bloomEffect);
        if (glow == false)
        {
            bloomEffect.active = false;
        }
        else
        {
            bloomEffect.active = true;
        }
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("VFX"))
        {
            if (go.activeInHierarchy)
            {
                VisualEffect gv = go.GetComponent<VisualEffect>();
                if (!All())
                {
                    gv.enabled = false;
                }
                else if (All())
                {
                    gv.enabled = true;
                }
            }
        }

    }
}

