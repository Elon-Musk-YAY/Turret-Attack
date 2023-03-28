using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeasonalEventsManager: MonoBehaviour
{
    public static SeasonalEventsManager instance;
    private void Awake()
    {
        instance = this;
    }
    public void SetSeasonalEvents(bool halloween, bool christmas) {
        Debug.LogWarning($"Values recieved: {halloween} and {christmas}");
		SeasonalEvents.HalloweenSeason = halloween;
		SeasonalEvents.ChristmasSeason = christmas;
	}
}

