using UnityEngine;

public class EventMaterialToggle: MonoBehaviour
{

	public EventTypes toggleEvent;
	public Material newGameObjectMaterial;
    public bool isNode;

    private void Start()
    {
        if (toggleEvent == EventTypes.Halloween && SeasonalEvents.HalloweenSeason) {
			gameObject.GetComponent<Renderer>().material = newGameObjectMaterial;
            if (isNode) {
                gameObject.GetComponent<Node>().startColor = newGameObjectMaterial.color;
                gameObject.GetComponent<Node>().startEmission = newGameObjectMaterial.GetColor("_EmissionColor");
            }
		} else if (toggleEvent == EventTypes.Christmas && SeasonalEvents.ChristmasSeason) {
            gameObject.GetComponent<Renderer>().material = newGameObjectMaterial;
        }
    }
}

