using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CustomEditor(typeof(LevelInfo)), CanEditMultipleObjects]
public class LevelInfoEditor : Editor
{
    private BoxBoundsHandle boundsHandle = new BoxBoundsHandle();

    protected virtual void OnSceneGUI()
    {
        LevelInfo levelInfo = (LevelInfo)target;
        
        boundsHandle.center = levelInfo.GameplayArea.center;
        boundsHandle.size = levelInfo.GameplayArea.size;

        EditorGUI.BeginChangeCheck();

        boundsHandle.DrawHandle();

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(levelInfo, "Change Bounds");

            Bounds updatedBounds = default(Bounds);
            updatedBounds.center = boundsHandle.center;
            updatedBounds.size = boundsHandle.size;

            levelInfo.GameplayArea = updatedBounds;
        }
    }
}