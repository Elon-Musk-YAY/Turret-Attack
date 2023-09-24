
using System;
using System.Numerics;
[Serializable]
public class GameStats
{
	public double standardTurretCost { get; set; }
    public double missleLauncherCost { get; set; }
    public double laserBeamerCost { get; set; }
    public double forceFieldLauncherCost { get; set; }
    public double bufferCost { get; set; }
    public double spiralTurretCost { get; set; }
    public int secondsPlayed { get; set; }
    public ulong damageDealt { get; set; }
    public ulong moneyGained { get; set; }
    public int remainingStandardTurretsAvailible { get; set; }
    public int remainingMissleLaunchersAvailible { get; set; }
    public int remainingLaserBeamersAvailible { get; set; }
    public int remainingAuraLaunchersAvailible { get; set; }
    public int remainingBuffersAvailible { get; set; }
    public int remainingSpiralTurretsAvailible { get; set; }
}

