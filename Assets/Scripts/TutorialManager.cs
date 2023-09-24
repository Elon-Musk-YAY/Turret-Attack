using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

class WaitForOkayButtonPressed : CustomYieldInstruction
{
    bool allowContinue = false;

    public override bool keepWaiting
    {
        get
        {
            return !(allowContinue);
        }
    }

    public void Allow()
    {
        allowContinue = true;
    }
}

class WaitForSelectStandardTurret : CustomYieldInstruction
{
    public override bool keepWaiting
    {
        get
        {
            return !(BuildManager.Instance.GetTurretToBuild() == Shop.Instance.GetBlueprintByID(0));
        }
    }

}

class WaitForPlaceStandardTurret : CustomYieldInstruction
{
    public override bool keepWaiting
    {
        get
        {
            return !(StatsManager.remainingStandardTurretsAvailible == 24);
        }
    }
}

public class TutorialManager : MonoBehaviour
{
    // Start is called before the first frame update

    public int buttonPressIndex = -1;
    public int instructionIndex = 0;
    public static bool allowedToPlace;

  

    public GameObject[] instructionPages;
    private WaitForOkayButtonPressed f;
    // node index 114 is wehre i watn the turret to be placed


    IEnumerator s()
    {
        if (File.Exists(SaveSystem.path))
        {
            this.enabled = false;
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(2f);
            for (int i = 0; i < instructionPages.Length; i++)
            {
                GameObject g = instructionPages[i];
                instructionIndex = i;
                g.SetActive(true);
                Time.timeScale = 0;
                f = new WaitForOkayButtonPressed();
                yield return f;
                Time.timeScale = 1;
                g.SetActive(false);
                if (i == 2)
                {
                    yield return new WaitForSelectStandardTurret();
                    yield return new WaitForSeconds(1f);
                } else if (i == 3)
                {
                    GameManager.nodes.transform.GetChild(114).GetComponent<Node>().startColor = new Color32(0,0,255,255);
                    GameManager.nodes.transform.GetChild(114).GetComponent<Renderer>().material.color = new Color32(0, 0, 255, 255);
                    allowedToPlace = true;
                    yield return new WaitForPlaceStandardTurret();
                    yield return new WaitForSeconds(1f);
                    GameManager.nodes.transform.GetChild(114).GetComponent<Node>().startColor = new Color32(255, 255, 255, 255);
                    GameManager.nodes.transform.GetChild(114).GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 255);
                } else if (i == instructionPages.Length-1)
                {
                    WaveSpawner.Instance.ReadyGame();
                }
                else
                {
                    yield return new WaitForSeconds(2f);
                }
            }
            this.enabled = false;
            yield return null;
        }
    }

    void Awake()
    {
        StartCoroutine(nameof(s));
    }

    public void Click()
    {
        f.Allow();
    }
}
