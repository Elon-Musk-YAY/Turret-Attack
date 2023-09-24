using UnityEngine;

public class EventLightToggle : MonoBehaviour
{
    public EventTypes toggleEvent;
    public Light newGameObjectLight;

    private void Start()
    {
        Light l = gameObject.GetComponent<Light>();
        if (toggleEvent == EventTypes.Halloween && SeasonalEvents.HalloweenSeason)
        {
            l.color = newGameObjectLight.color;
        }
        else if (toggleEvent == EventTypes.Christmas && SeasonalEvents.ChristmasSeason)
        {
            l.color = newGameObjectLight.color;
        }
    }
   
}

