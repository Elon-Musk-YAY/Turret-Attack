using UnityEngine;

public class EventLightToggle : MonoBehaviour
{
    public EventTypes toggleEvent;
    public Light newGameObjectLight;

    private void Start()
    {
        if (toggleEvent == EventTypes.Halloween && SeasonalEvents.HalloweenSeason)
        {
            gameObject.GetComponent<Light>().color = newGameObjectLight.color;
        }
        else if (toggleEvent == EventTypes.Christmas && SeasonalEvents.ChristmasSeason)
        {
            gameObject.GetComponent<Light>().color = newGameObjectLight.color;
        }
    }
   
}

