using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DecorationMarker)), CanEditMultipleObjects]
public class DecorationHelper : Editor
{
    List<DecorationMarker> _selectedGO = new List<DecorationMarker>();

    private void OnEnable()
    {
        foreach (var item in targets)
        {
            _selectedGO.Add((DecorationMarker)item);
        }
    }

    private void OnSceneGUI()
    {
        Handles.BeginGUI();

        SetChildButton();

        Handles.EndGUI();
    }

    void SetChildButton()
    {
        GUILayout.BeginArea(new Rect(20, 20, 200, 175));

        if (GUILayout.Button("SetChild"))
        {
            foreach (var go in _selectedGO)
            {
                if(Physics.Raycast(go.transform.position, Vector3.down, out var hit, .5f, 1 << 9))
                {
                    go.transform.SetParent(hit.transform);
                    EditorUtility.SetDirty(go);
                }
            }
        }

        GUILayout.EndArea();
    }
}
