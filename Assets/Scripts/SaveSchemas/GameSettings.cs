using System;
using System.Collections.Generic;

[System.Serializable]
public class GameSettings
{
	public bool glow { get; set; }
	public ParticleSettingTypes particles { get; set; }
	public float volume { get; set; }
	public bool showFPS { get; set; }
	public float scrollSensitivity { get; set; }

}


