using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(ButtonWithText))]
[CanEditMultipleObjects]
public class ButtonWithAudioEditor : ButtonEditor {
    private SerializedProperty _text;

    protected override void OnEnable() {
        base.OnEnable();
        _text = serializedObject.FindProperty(nameof(_text));
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        base.OnInspectorGUI();

        EditorGUILayout.PropertyField(_text);
        serializedObject.ApplyModifiedProperties();
    }
}