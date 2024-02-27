using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScorePopUp), editorForChildClasses: true)]
public class ScorePopUpEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        ScorePopUp e = target as ScorePopUp;
        if (GUILayout.Button("True"))
            e.OnScoredEvent(1, true);

        if (GUILayout.Button("False"))
            e.OnScoredEvent(5, false);
    }
}
