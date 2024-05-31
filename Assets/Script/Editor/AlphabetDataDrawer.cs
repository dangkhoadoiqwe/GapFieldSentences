using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Alphabet))]
[CanEditMultipleObjects]
[System.Serializable]
public class NewBehaviourScript : Editor
{
    private ReorderableList AlphabetPlainList;
    private ReorderableList AlphabetNomalList;
    private ReorderableList AlphabetHightlightedList;
    private ReorderableList AlphabetWrongList;


    private void OnEnable()
    {
        InitalizeReorderableList(ref AlphabetWrongList, propertyName: "AlphabetPlain", listlabel: "Alphabet Plain");
        InitalizeReorderableList(ref AlphabetWrongList, propertyName: "AlphabetNomal", listlabel: "Alphabet Nomal");
        InitalizeReorderableList(ref AlphabetWrongList, propertyName: "AlphabetHightlighted", listlabel: "Alphabet Hightlighted");
        InitalizeReorderableList(ref AlphabetWrongList, propertyName: "AlphabetWrong", listlabel: "Alphabet Wrong");
    }
    public void OnInspectorGUI()
    {
        serializedObject.Update();
        AlphabetPlainList.DoLayoutList();
        AlphabetNomalList.DoLayoutList();
        AlphabetHightlightedList.DoLayoutList();
        AlphabetWrongList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    public void InitalizeReorderableList(ref ReorderableList list,  string propertyName, string listlabel)
    {
        list = new ReorderableList(serializedObject, elements: serializedObject.FindProperty(propertyName), draggable: true, displayHeader: true, displayAddButton: true,
          displayRemoveButton: true);
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, listlabel);

        };
        var l = list;
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocuesed) => {
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(
                position: new Rect(rect.x, rect.y, width:60,  EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("letter"), GUIContent.none);


            EditorGUI.PropertyField(position: new Rect(x: rect.x +70, rect.y, width:rect.width -60 -30 ,
                EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("image"), GUIContent.none);
        };
    }
}
