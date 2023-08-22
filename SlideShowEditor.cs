using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(SlideShow))]
public class SlideShowEditor : Editor
{
    private ReorderableList slideList;
    private ReorderableList audioList;

    private void OnEnable()
    {
        slideList = new ReorderableList(serializedObject,
                                        serializedObject.FindProperty("slideEvents"),
                                        true, true, true, true);
        audioList = new ReorderableList(serializedObject,
                                        serializedObject.FindProperty("audioEvents"),
                                        true, true, true, true);

        slideList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Slide Events");
        };

        audioList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Audio Events");
        };

        slideList.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = slideList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            float halfWidth = rect.width * 0.5f;

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, halfWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("slide"),
                GUIContent.none);

            EditorGUI.PropertyField(
                new Rect(rect.x + halfWidth, rect.y, halfWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("duration"),
                new GUIContent("Duration (s)"));
        };

        audioList.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = audioList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            float halfWidth = rect.width * 0.5f;

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, halfWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("audioClip"),
                GUIContent.none);

            EditorGUI.PropertyField(
                new Rect(rect.x + halfWidth, rect.y, halfWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("duration"),
                new GUIContent("Duration (s)"));
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw default properties
        DrawPropertiesExcluding(serializedObject, "slideEvents", "audioEvents");

        EditorGUILayout.Space(10);  // Add some spacing

        // Draw our reorderable lists
        slideList.DoLayoutList();

        EditorGUILayout.Space(5);   // Add some spacing

        audioList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}
