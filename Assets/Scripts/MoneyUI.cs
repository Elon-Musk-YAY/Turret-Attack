using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    public Text moneyText;

    // Update is called once per frame
    void Update()
    {
        PlayerStats.Money = System.Math.Clamp(PlayerStats.Money, 0, 9_000_000_000_000_000_000);
        moneyText.text = $"${GameManager.ShortenNumL(PlayerStats.Money)}".Replace(".10",".1").Replace(".20", ".2").Replace(".30", ".3").Replace(".40", ".4").Replace(".50", ".5").Replace(".60", ".6").Replace(".70", ".7").Replace(".80", ".8").Replace(".90", ".9").Replace(".00","");
        
    }
}
