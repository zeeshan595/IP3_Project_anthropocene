#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;

public class PlayerEditor : EditorWindow
{
    private static float playerSpeed;
    private static float gravity;
    private static float health;
    private static float water;
    private static float jumpHeight;
    private static float rotateSpeed;
    private static float jumpSpeed;

    private static string path = "Assets/Source/Prefabs/Player/";

    [MenuItem("IP3/Player Editor")]
    private static void Init()
    {
        // Get existing open window or if none, make a new one:
        PlayerEditor window = (PlayerEditor)EditorWindow.GetWindow(typeof(PlayerEditor));
        window.maxSize = new Vector2(500, 300);
        window.minSize = new Vector2(500, 300);
        window.Show();
        LoadStats();
    }

    private static void LoadStats()
    {
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(path + "Player.prefab", typeof(GameObject));
        GameObject clone = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity);
        PlayerStats script = clone.GetComponent<PlayerStats>();
        PlayerMovement script2 = clone.GetComponent<PlayerMovement>();

        playerSpeed = script.playerSpeed;
        gravity = script.gravity;
        health = script.health;
        water = script.water;

        jumpHeight = script2.jumpHeight;
        rotateSpeed = script2.rotateSpeed;
        jumpSpeed = script2.jumpSpeed;

        DestroyImmediate(clone);
    }

    private void SavePlayer()
    {
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(path + "Player.prefab", typeof(GameObject));
        GameObject clone = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity);
        PlayerStats script = clone.GetComponent<PlayerStats>();
        PlayerMovement script2 = clone.GetComponent<PlayerMovement>();

        script.playerSpeed = playerSpeed;
        script.gravity = gravity;
        script.health = health;
        script.water = water;

        script2.jumpHeight = jumpHeight;
        script2.rotateSpeed = rotateSpeed;
        script2.jumpSpeed = jumpSpeed;

        AssetDatabase.DeleteAsset(path + "Player.prefab");
        PrefabUtility.CreatePrefab(path + "Player.prefab", clone);
        DestroyImmediate(clone);
    }

    private void OnGUI()
    {
        GUILayout.Box("General");

        //Gravity
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Gravity", GUILayout.Width(150));
        gravity = EditorGUILayout.FloatField(gravity);

        EditorGUILayout.EndHorizontal();

        //health
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Health", GUILayout.Width(150));
        health = EditorGUILayout.FloatField(health);

        EditorGUILayout.EndHorizontal();

        GUILayout.Box("Movment");

        //Speed
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Speed", GUILayout.Width(150));
        playerSpeed = EditorGUILayout.FloatField(playerSpeed);

        EditorGUILayout.EndHorizontal();

        //Rotate Speed
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Rotate Speed", GUILayout.Width(150));
        rotateSpeed = EditorGUILayout.FloatField(rotateSpeed);

        EditorGUILayout.EndHorizontal();

        //Jump Height
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Jump Height", GUILayout.Width(150));
        jumpHeight = EditorGUILayout.FloatField(jumpHeight);

        EditorGUILayout.EndHorizontal();

        //Jump Speed
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Jump Speed", GUILayout.Width(150));
        jumpSpeed = EditorGUILayout.FloatField(jumpSpeed);

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Save"))
        {
            SavePlayer();
        }
    }
}

#endif