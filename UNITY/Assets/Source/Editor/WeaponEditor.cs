#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

public class WeaponEditor : EditorWindow
{
    private WeaponType weapon = WeaponType.ScatterGun;
    private WeaponType oldWeapon = WeaponType.HoseGun;
    private Sprite crosshair;
    private float range;
    private float damage;
    private float waterUsage;
    private float acuracy;
    private float spray;
    private float tankLimit;
    private bool explode;
    private bool automatic;

    private string path = "Assets/Source/Prefabs/Weapons/";

    [MenuItem("IP3/Weapon Editor")]
    private static void Init()
    {
        // Get existing open window or if none, make a new one:
        WeaponEditor window = (WeaponEditor)EditorWindow.GetWindow(typeof(WeaponEditor));
        window.maxSize = new Vector2(500, 300);
        window.minSize = new Vector2(500, 300);
        window.Show();
    }

    private void LoadWeaponStats()
    {
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(path + weapon.ToString() + ".prefab", typeof(GameObject));
        Weapon script = prefab.GetComponent<Weapon>();
        crosshair = script.crosshair;
        range = script.range;
        damage = script.damage;
        waterUsage = script.waterUsage;
        acuracy = script.acuracy;
        spray = script.spray;
        explode = script.explode;
        automatic = script.automatic;
        tankLimit = script.waterTank;
    }

    private void SaveWeapon()
    {
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(path + weapon.ToString() + ".prefab", typeof(GameObject));
        Weapon script = prefab.GetComponent<Weapon>();
        script.crosshair = crosshair;
        script.range = range;
        script.damage = damage;
        script.waterUsage = waterUsage;
        script.acuracy = acuracy;
        script.spray = spray;
        script.explode = explode;
        script.automatic = automatic;
        script.waterTank = tankLimit;
    }

    private void OnGUI()
    {
        GUILayout.Box("Select a weapon to edit");

        weapon = (WeaponType)EditorGUILayout.EnumPopup(weapon, GUILayout.Width(150));
        if (weapon != oldWeapon)
        {
            LoadWeaponStats();
            oldWeapon = weapon;
        }

        //Crosshair
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Cross Hair", GUILayout.Width(150));
        crosshair = (Sprite)EditorGUILayout.ObjectField(crosshair, typeof(Sprite), true);

        EditorGUILayout.EndHorizontal();

        //Range
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Range", GUILayout.Width(150));
        range = EditorGUILayout.FloatField(range);

        EditorGUILayout.EndHorizontal();

        //damage
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Damage", GUILayout.Width(150));
        damage = EditorGUILayout.FloatField(damage);

        EditorGUILayout.EndHorizontal();

        //water usage
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Water Usage", GUILayout.Width(150));
        waterUsage = EditorGUILayout.FloatField(waterUsage);

        EditorGUILayout.EndHorizontal();

        //tank limit
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Water Tank", GUILayout.Width(150));
        tankLimit = EditorGUILayout.FloatField(tankLimit);

        EditorGUILayout.EndHorizontal();

        //acuracy
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Acuracy", GUILayout.Width(150));
        acuracy = EditorGUILayout.FloatField(acuracy);

        EditorGUILayout.EndHorizontal();

        //Spray
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Spray", GUILayout.Width(150));
        spray = EditorGUILayout.FloatField(spray);

        EditorGUILayout.EndHorizontal();

        //Explode
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("explode", GUILayout.Width(150));
        explode = EditorGUILayout.Toggle(explode);

        EditorGUILayout.EndHorizontal();

        //Automatic
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Automatic", GUILayout.Width(150));
        automatic = EditorGUILayout.Toggle(automatic);

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Save"))
        {
            SaveWeapon();
        }
    }
}

#endif