using UnityEngine;

public class EventMaterialToggle: MonoBehaviour
{

	public EventTypes toggleEvent;
	public Material newGameObjectMaterial;
    public bool isNode;

    private void Start()
    {
        Node n = gameObject.GetComponent<Node>();
        Renderer r = gameObject.GetComponent<Renderer>();
        if (toggleEvent == EventTypes.Halloween && SeasonalEvents.HalloweenSeason) {
			r.sharedMaterial = newGameObjectMaterial;
            if (isNode) {
                n.startColor = newGameObjectMaterial.color;
                n.startEmission = newGameObjectMaterial.GetColor("_EmissionColor");
            }
		} else if (toggleEvent == EventTypes.Christmas && SeasonalEvents.ChristmasSeason) {
            r.sharedMaterial = newGameObjectMaterial;
            if (isNode)
            {
                n.startColor = newGameObjectMaterial.color;
                n.startEmission = newGameObjectMaterial.GetColor("_EmissionColor");
            }
        }
    }
}

