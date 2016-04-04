﻿public class Settings
{
    public static int playerid;
    public static string username = "my username";
    public static TeamType team = TeamType.Blue;
    public static WeaponType weaponType = WeaponType.ScatterGun;
    public static Character character = Character.Fishy;
    public static float masterVolume;
    public static float musicVolume;
    public static float SFXVolume;
}

public enum WeaponType
{
    ScatterGun,
    WaterRake,
    WaterBazooka,
    HoseGun,
    WaterGarnade
}

public enum Character
{
    Potatree,
    Rak,
    Fishy,
    JackieChan
}