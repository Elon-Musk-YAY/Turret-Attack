using System;
using System.Collections.Generic;

[System.Serializable]
public class Save
{
	public int Money { get; set; }
	public int Lives { get; set; }
	public bool win { get; set; }
	public int Rounds { get; set; }
	public int enemiesPerRound { get; set; }
	public float enemyHealth { get; set; }
	public float enemySpeed { get; set; }
	public float enemyWorth { get; set; }
	public bool glow { get; set; }
	public bool particles { get; set; }
	public int lastMulti { get; set; }


}

