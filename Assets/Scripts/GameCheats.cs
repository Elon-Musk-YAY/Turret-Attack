using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCheats : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField moneyInputField;
    public GameObject cheatsUI;

    private string RemoveBadChars(string p) {
        List<string> badChars = new List<string>
        {
            "k","m","b","t","qa","qi"
        }
        ;
        string n = p;
        n = n.ToLower();
        foreach(string badChar in badChars) {
            n = n.Replace(badChar, "");
        }
        return n;
    }
    private long ExpandNum(string num)
    {
        double newVal;
        Dictionary<string, long> multiplyValues = new Dictionary<string, long>
        {
            {"qi", 1_000_000_000_000_000_000 },
            {"qa", 1_000_000_000_000_000 },
            {"t", 1_000_000_000_000 },
            {"b", 1_000_000_000 },
            {"m", 1_000_000 },
            {"k", 1_000 }
        }
        ;

        
        string suffix = num.ToLower()[num.Length - 1].ToString();
        string suffix2char = num.ToLower()[num.Length - 2].ToString() + num.ToLower()[num.Length-1].ToString();
        if (suffix != "") {
            long multiplyVal = multiplyValues[suffix];
            if (!multiplyValues.ContainsKey(suffix) && multiplyValues[suffix2char] > 0) {
                multiplyVal = multiplyValues[suffix2char];
            }
            num = RemoveBadChars(num);
            newVal = double.Parse(num);
            newVal *= multiplyVal;
            return (long)newVal;
        }
        else
        {
            return long.Parse(num);
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
            if (ExpandNum(moneyInputField.text) < 0 || ExpandNum(moneyInputField.text) > long.MaxValue)
            {
                moneyInputField.text = "BAD INPUT";
                return;
            }
            PlayerStats.Money = ExpandNum(moneyInputField.text);
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
