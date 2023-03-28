using UnityEngine;

public class EventNodeUIToggle: MonoBehaviour
{

	public EventTypes toggleEvent;
    public BuildManager buildManager;
    public GameObject mainUI;
    public GameObject eventUI;

    private void Start()
    {
        if (toggleEvent == EventTypes.Halloween && SeasonalEvents.HalloweenSeason) {
            mainUI.SetActive(false);
            eventUI.SetActive(true);
            buildManager.nodeUI = eventUI.GetComponent<NodeUI>();
		} else if (toggleEvent == EventTypes.Christmas && SeasonalEvents.ChristmasSeason) {
            mainUI.SetActive(false);
            eventUI.SetActive(true);
            buildManager.nodeUI = eventUI.GetComponent<NodeUI>();
        }
    }
}

