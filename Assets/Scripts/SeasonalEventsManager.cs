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
		SeasonalEvents.halloweenSeason = halloween;
		SeasonalEvents.christmasSeason = christmas;
	}
    public Dictionary<string, bool> GetSeasonalEvents()
    {
        Dictionary<string, bool> values = new()
        {
            { "halloween", SeasonalEvents.halloweenSeason },
            {"christmas", SeasonalEvents.christmasSeason }
        };
        return values;
    }
}

