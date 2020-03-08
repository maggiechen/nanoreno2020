using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Chapter))]
public class ChapterDrawer : PropertyDrawer {

    // this method is adapted from http://answers.unity.com/answers/683381/view.html
    public string GetIntListAsString(SerializedProperty sp) {
        int arrayLength = 0;
 
        sp.Next(true); // skip generic field
        sp.Next(true); // advance to array size field

        // Get the array size
        arrayLength = sp.intValue;

        sp.Next(true); // advance to first array index

        // Write values to list
        List<int> values = new List<int>(arrayLength);
        int lastIndex = arrayLength - 1;
        for(int i = 0; i < arrayLength; i++) {
            values.Add(sp.intValue); // copy the value to the list
            if (i < lastIndex) {
                sp.Next(false);
            } // advance without drilling into children
        }

        return string.Join(", ", values);
    }

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
        SerializedProperty nextLinesProperty = currentLineProperty.FindPropertyRelative("nextLineIds");
        string nextLinesString = GetIntListAsString(nextLinesProperty);
        SerializedProperty stateProperty = property.FindPropertyRelative("state");

        var currentNameRect = new Rect(position.x, startY, position.width, singleHeight);
        var currentDialogueRect = new Rect(position.x, startY + singleHeight, position.width, singleHeight);
        var nextLinesRect = new Rect(position.x, startY + 2*singleHeight, position.width, singleHeight);
        var stateRect = new Rect(position.x, startY + 3*singleHeight, position.width, singleHeight);

        // indent
        int oldIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel++;

        // draw fields
        EditorGUI.PropertyField(currentNameRect, currentNameProperty);
        EditorGUI.PropertyField(currentDialogueRect, currentDialogueProperty);
        EditorGUI.LabelField(nextLinesRect, $"Next ids: {nextLinesString}");
        EditorGUI.PropertyField(stateRect, stateProperty);

        //reset indent
        EditorGUI.indentLevel = oldIndentLevel;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorGUIUtility.singleLineHeight * 5;
    }


}