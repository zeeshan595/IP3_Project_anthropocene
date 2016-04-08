public class Settings
{
    public static int playerid;
    public static string username = "my username";
    public static TeamType team = TeamType.Blue;
    public static WeaponType weaponType = WeaponType.ScatterGun;
    public static Character character = Character.Potatree;
    public static float masterVolume;
    public static float musicVolume;
    public static float SFXVolume;
}

public enum WeaponType
{
    ScatterGun = 0,
    HoseGun = 1,
    WaterRake = 2,
    WaterBazooka = 3
}

public enum Character
{
    Potatree,
    Rak,
    Fishy,
    JackieChan
}