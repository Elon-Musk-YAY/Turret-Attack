using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeasonalEventsManager: MonoBehaviour
{
    public static SeasonalEventsManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void SetSeasonalEvents(bool halloween, bool christmas) {
        Debug.LogWarning($"Values recieved: {halloween} and {christmas}");
		SeasonalEvents.HalloweenSeason = halloween;
		SeasonalEvents.ChristmasSeason = christmas;
	}
}

