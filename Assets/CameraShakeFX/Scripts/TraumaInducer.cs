 using UnityEngine;
using System.Collections;

/* Example script to apply trauma to the camera or any game object */
public class TraumaInducer : MonoBehaviour 
{
    [Tooltip("Maximum stress the effect can inflict upon objects Range([0,1])")]
    public float MaximumStress = 0.6f;
    [Tooltip("Maximum distance in which objects are affected by this TraumaInducer")]
    public float Range = 45;
    public StressReceiver receiver;

    public void heavyShake()
    {
        print("GO");
//        
//          CUSTOM  CODE
//        
// 
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        float distance = Vector3.Distance(transform.position, cam.transform.position);
        float distance01 = Mathf.Clamp01(distance / Range);
        float stress = (1 - Mathf.Pow(distance01, 2)) * MaximumStress * 2;
        receiver.InduceStress(stress);

    }

    public void lightShake()
    {
        print("GO");
        //        
        //          CUSTOM  CODE
        //        
        // 
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        float distance = Vector3.Distance(transform.position, cam.transform.position);
        float distance01 = Mathf.Clamp01(distance / Range);
        float stress = (1 - Mathf.Pow(distance01, 2)) * MaximumStress;
        receiver.InduceStress(stress,true);

    }
}