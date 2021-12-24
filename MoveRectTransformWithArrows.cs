using System.Reflection;
using UnityEditor;
using UnityEngine;

/// Script allows to position canvas objects more precisely.
/// You need to select the object and set focus to the scene view or inpsector for script to process events.
[CanEditMultipleObjects]
[CustomEditor(typeof(RectTransform), true)]
public class MoveRectTransformWithArrows : UnityEditor.Editor
{
    private const float Shift = 0.1f;
    private const string UndoName = "MoveRectTransform";

    private RectTransform _targetTransf;
    private UnityEditor.Editor _editorInstance;

    private void OnEnable()
    {
        var ass = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var rtEditor = ass.GetType("UnityEditor.RectTransformEditor");
        _editorInstance = CreateEditor(target, rtEditor);
    }

    //Event.Use() should not be called from OnInspectorGUI
    public override void OnInspectorGUI()
    {
        _editorInstance.OnInspectorGUI();
        HandleMovement();
    }

    public void OnSceneGUI()
    {
        WorkaroundRectTransformInheritance();

        if (HandleMovement())
            Event.current.Use();
    }

    //https://forum.unity.com/threads/recttransform-custom-editor-ontop-of-unity-recttransform-custom-editor.455925/
    private void WorkaroundRectTransformInheritance()
    {
        var onSceneGUIMethod = _editorInstance.GetType()
            .GetMethod("OnSceneGUI", BindingFlags.NonPublic | BindingFlags.Instance);
        onSceneGUIMethod?.Invoke(_editorInstance, null);
    }

    private bool HandleMovement()
    {
        _targetTransf = target as RectTransform;

        var e = Event.current;
        switch (e.keyCode)
        {
            case KeyCode.UpArrow:
                Undo.RecordObject(_targetTransf, UndoName);
                MoveRect(_targetTransf, 0f, Shift);
                return true;
            case KeyCode.DownArrow:
                Undo.RecordObject(_targetTransf, UndoName);
                MoveRect(_targetTransf, 0f, -Shift);
                return true;
            case KeyCode.LeftArrow:
                Undo.RecordObject(_targetTransf, UndoName);
                MoveRect(_targetTransf, -Shift, 0f);
                return true;
            case KeyCode.RightArrow:
                Undo.RecordObject(_targetTransf, UndoName);
                MoveRect(_targetTransf, Shift, 0f);
                return true;
        }

        return false;
    }

    private static void MoveRect(Transform rect, float x, float y)
    {
        rect.localPosition += new Vector3(x, y);
        EditorUtility.SetDirty(rect);
    }
}