using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DirectorScript))]
public class DirectorScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DirectorScript director = (DirectorScript)target;

        // Afficher les informations de temps et les objets en cours
        EditorGUILayout.LabelField("Current Time", director.currentTime.ToString("F2"));
        EditorGUILayout.LabelField("Current Animation", director.currentAnimation);
        EditorGUILayout.LabelField("Current Camera", director.currentCamera);
        EditorGUILayout.LabelField("Current Camera Movement", director.currentMovement);

        // Afficher les autres propriétés de DirectorScript
        DrawDefaultInspector();
    }
}
