using System;
using UnityEngine;
using UnityEngine.UI;

public class GameCheats : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField moneyInputField;
    public GameObject cheatsUI;
    private int ExpandNum(string num)
    {
        double newVal;
        string suffix = num.ToLower()[num.Length - 1].ToString();
        if (suffix == "b")
        {
            num = num.ToLower().Replace(suffix, "").Replace("m","").Replace("k","");
            newVal = double.Parse(num);
            newVal *= 1_000_000_000;
            return (int)newVal;

        }
        if (suffix == "m")
        {
            num = num.ToLower().Replace(suffix, "").Replace("b", "").Replace("k", "");
            newVal = double.Parse(num);
            newVal *= 1_000_000;
            return (int)newVal;

        }
        else if (suffix == "k")
        {
            num = num.ToLower().Replace(suffix, "").Replace("m", "").Replace("b", "");
            newVal = double.Parse(num);
            newVal *= 1_000;
            return (int)newVal;

        }
        else
        {
            return int.Parse(num);
        }
    }

    public void CloseMenu()
    {
        cheatsUI.SetActive(false);
    }
    public void OpenMenu()
    {
        cheatsUI.SetActive(true);
    }

    public void Plus5Lives()
    {
        PlayerStats.Lives += 5;
    }
    public void Plus1Lives()
    {
        PlayerStats.Lives += 1;
    }
    public void Minus5Lives()
    {
        PlayerStats.Lives -= 5;
        PlayerStats.Lives = (int)Mathf.Clamp(PlayerStats.Lives, 1, Mathf.Infinity);
    }
    public void Minus1Lives()
    {
        PlayerStats.Lives -= 1;
        PlayerStats.Lives = (int)Mathf.Clamp(PlayerStats.Lives, 1, Mathf.Infinity);
    }
    public void SetMoney()
    {
        try
        {
            if (ExpandNum(moneyInputField.text) < 0 || ExpandNum(moneyInputField.text) > 2_000_000_000)
            {
                moneyInputField.text = "BAD INPUT";
                return;
            }
            PlayerStats.Money = ExpandNum(moneyInputField.text);
            //Debug.LogError(ExpandNum(moneyInputField.text));
        }
        catch (FormatException)
        {
            moneyInputField.text = "BAD INPUT";
        }
        catch (OverflowException)
        {
            moneyInputField.text = "BAD INPUT";
        }
    }
}
