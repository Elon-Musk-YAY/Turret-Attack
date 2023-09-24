using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Runtime.InteropServices;



public class StatsManager : MonoBehaviour
{
	public static StatsManager Instance;

	public static ulong moneyEarnedThisSession = 0;
    public static ulong importedMoneyEarned = 0;
    public static int secondsPlayed = 0;
    public static ulong damageDealtThisSession = 0;
    public static ulong importedDamageDealt = 0;
    public static double standardTurretMultiplier = 1f;
    public static double missleLauncherMultiplier = 1f;
    public static double laserBeamerMultiplier = 1f;
    public static double auraLauncherMultiplier = 1f;
    public static double bufferMultiplier = 1f;
    public static double spiralMultiplier = 1f;
    public static int remainingStandardTurretsAvailible =25;
    public static int remainingMissleLaunchersAvailible = 20;
    public static int remainingLaserBeamersAvailible = 20;
    public static int remainingAuraLaunchersAvailible = 15;
    public static int remainingBuffersAvailible = 10;
    public static int remainingSpiralTurretsAvailible = 5;

    private void Awake()
    {
		Instance = this;
        ImportStats();
    }

	public static /*IEnumerator*/ void AddToMoneyEarned(ulong num) {
        //BigIn t e ger parsed = Big  I nteger.Parse(num.ToString());
        moneyEarnedThisSession += num;
		//yield return null;
	}
    public static /*IEnumerator*/ void AddToDamageDealt(ulong num)
    {
        // B i gInteger parsed = BigInt  e  g er.Parse(num.ToString());
        damageDealtThisSession += num;
        //yield return null;
    }
    IEnumerator AddSeconds()
    {
        while (true)
        {
            if (Time.timeScale == 0f)
            {
                yield return null;
                continue;
            }
            yield return new WaitForSecondsRealtime(1f);
            secondsPlayed++;
        }
    }
    // Use this for initialization
    void Start()
	{
        StartCoroutine(AddSeconds());
	}

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void SyncDB();
#endif


    public static void SaveStats() {
        GameStats gs = new();
        gs.moneyGained = moneyEarnedThisSession + importedMoneyEarned;
        gs.secondsPlayed = secondsPlayed;
        gs.damageDealt = importedDamageDealt + damageDealtThisSession;
        gs.standardTurretCost = standardTurretMultiplier;
        gs.missleLauncherCost = missleLauncherMultiplier;
        gs.laserBeamerCost = laserBeamerMultiplier;
        gs.forceFieldLauncherCost = auraLauncherMultiplier;
        gs.bufferCost = bufferMultiplier;
        gs.spiralTurretCost = spiralMultiplier;
        gs.remainingStandardTurretsAvailible = remainingStandardTurretsAvailible;
        gs.remainingMissleLaunchersAvailible = remainingMissleLaunchersAvailible;
        gs.remainingLaserBeamersAvailible = remainingLaserBeamersAvailible;
        gs.remainingAuraLaunchersAvailible = remainingAuraLaunchersAvailible;
        gs.remainingBuffersAvailible = remainingBuffersAvailible;
        gs.remainingSpiralTurretsAvailible = remainingSpiralTurretsAvailible;
        BinaryFormatter formatter = new();
        FileStream stream = new(SaveSystem.statsPath, FileMode.Create);
        formatter.Serialize(stream, gs);
        stream.Close();
        stream.Dispose();
        Debug.Log("Saving Stats");

#if UNITY_WEBGL && !UNITY_EDITOR
        //flush our changes to IndexedDB
        SyncDB();
#endif
    }

    public static void ImportStats() {
        Debug.Log("Importing Stats");
        GameStats gs = new();
        BinaryFormatter formatter = new();
        if (!File.Exists(SaveSystem.statsPath)) {
            SaveStats();
            return;
        }
        FileStream stream = new FileStream(SaveSystem.statsPath, FileMode.Open);
        try
        {
            if (stream.Length == 0)
            {
                SaveStats();
                stream.Dispose();
                return;
            }
            object data = formatter.Deserialize(stream);
            gs = data as GameStats;
            importedMoneyEarned = gs.moneyGained;
            importedDamageDealt = gs.damageDealt;
            secondsPlayed = gs.secondsPlayed;
            standardTurretMultiplier = Math.Round(Math.Clamp(gs.standardTurretCost, 1, GameManager.maxTurretPriceIncrease), 2);
            missleLauncherMultiplier = Math.Round(Math.Clamp(gs.missleLauncherCost, 1, GameManager.maxTurretPriceIncrease), 2);
            laserBeamerMultiplier = Math.Round(Math.Clamp(gs.laserBeamerCost, 1, GameManager.maxTurretPriceIncrease), 2);
            auraLauncherMultiplier = Math.Round(Math.Clamp(gs.forceFieldLauncherCost, 1, GameManager.maxTurretPriceIncrease), 2);
            bufferMultiplier = Math.Round(Math.Clamp(gs.bufferCost,1, GameManager.maxTurretPriceIncrease), 2);
            spiralMultiplier = Math.Round(Math.Clamp(gs.spiralTurretCost, 1, GameManager.maxTurretPriceIncrease), 2);
            remainingStandardTurretsAvailible = gs.remainingStandardTurretsAvailible;
            remainingMissleLaunchersAvailible = gs.remainingMissleLaunchersAvailible;
            remainingLaserBeamersAvailible = gs.remainingLaserBeamersAvailible;
            remainingAuraLaunchersAvailible = gs.remainingAuraLaunchersAvailible;
            remainingBuffersAvailible = gs.remainingBuffersAvailible;
            remainingSpiralTurretsAvailible = gs.remainingSpiralTurretsAvailible;
        }
        catch (Exception e)
        {
            Debug.LogError("file is currupted\n" + e + "\n" + stream);
        }
    }

    public static string GetFormattedPlayTime()
    {
        int seconds = secondsPlayed;
        int hours = seconds / 3600; // 1 hour = 3600 seconds
        seconds %= 3600; // Remaining seconds after subtracting hours
        int minutes = seconds / 60; // 1 minute = 60 seconds
        int remainingSeconds = seconds % 60;
        string output = "";
        if (hours > 0)
        {
            output += $"{hours}h, ";
        }
        if (minutes > 0)
        {
            output += $"{minutes}m, ";
        }
        if (remainingSeconds > 0)
        {
            output += $"{remainingSeconds}s";
        }

        // Remove trailing comma and whitespace if present
        output = output.TrimEnd(',', ' ');
        return output;
    }
}

