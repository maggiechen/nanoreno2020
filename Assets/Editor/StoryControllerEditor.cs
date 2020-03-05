using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Chapter))]
public class ChapterDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        float singleHeight = EditorGUIUtility.singleLineHeight;
        float startY = position.y + singleHeight; // account for prefix height

        // get properties
        SerializedProperty currentLineProperty = property.FindPropertyRelative("currentLine");
        SerializedProperty currentNameProperty = currentLineProperty.FindPropertyRelative("actorName");
        SerializedProperty currentDialogueProperty = currentLineProperty.FindPropertyRelative("dialogueText");
        SerializedProperty stateProperty = property.FindPropertyRelative("state");

        var currentNameRect = new Rect(position.x, startY, position.width, singleHeight);
        var currentDialogueRect = new Rect(position.x, startY + singleHeight, position.width, singleHeight);
        var stateRect = new Rect(position.x, startY + 2*singleHeight, position.width, singleHeight);

        // indent
        int oldIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel++;

        // draw fields
        EditorGUI.PropertyField(currentNameRect, currentNameProperty);
        EditorGUI.PropertyField(currentDialogueRect, currentDialogueProperty);
        EditorGUI.PropertyField(stateRect, stateProperty);

        //reset indent
        EditorGUI.indentLevel = oldIndentLevel;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorGUIUtility.singleLineHeight * 4;
    }


}