#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityStandardAssets.ImageEffects;

public class EffectEditor : EditorWindow
{
    [MenuItem("IP3/Edge Detection")]
    private static void Init()
    {
        // Get existing open window or if none, make a new one:
        EffectEditor window = (EffectEditor)EditorWindow.GetWindow(typeof(EffectEditor));
        window.maxSize = new Vector2(300, 75);
        window.minSize = new Vector2(300, 75);
        window.Show();
    }

    private void OnGUI()
    {
        //Crosshair
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Senstivity Depth", GUILayout.Width(150));
        EdgeDetectionColor.sensitivityDepth = EditorGUILayout.FloatField(EdgeDetectionColor.sensitivityDepth);

        EditorGUILayout.EndHorizontal();

        //Range
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Senstivity Normal", GUILayout.Width(150));
        EdgeDetectionColor.sensitivityNormals = EditorGUILayout.FloatField(EdgeDetectionColor.sensitivityNormals);

        EditorGUILayout.EndHorizontal();

        //damage
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Sample Distance", GUILayout.Width(150));
        EdgeDetectionColor.sampleDist = EditorGUILayout.FloatField(EdgeDetectionColor.sampleDist);

        EditorGUILayout.EndHorizontal();
    }
}

#endif