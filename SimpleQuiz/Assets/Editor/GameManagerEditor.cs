using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("Διαγραφή σκορ παίχτη"))
        {
            if (EditorUtility.DisplayDialog("Διαγραφή του highscore;",
                                       "Είσαι σίγουρος;",
                                       "Ναι",
                                       "Όχι"))
            {
                PlayerPrefs.DeleteAll();
            }            
        }
    }
}
