using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using static BoardData;
using System.Text.RegularExpressions;

[CustomEditor(typeof(BoardData))]
[CanEditMultipleObjects]
[System.Serializable]
public class BoardDataDrawer : Editor
{
    private BoardData GameDataInstance => target as BoardData;
    private ReorderableList _dataList;

    public void OnEnable()
    {
        InitializeReorderableList(ref _dataList,poperty: "SearchingWords",listTable: "Searching word");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Space();
        DrawColumsRowsInputField();
        ConvertToUpperButton();
        if (GameDataInstance.Board != null && GameDataInstance.Colums > 0 && GameDataInstance.Rows > 0)
        {
            DrawBoardTable();
        }
        GUILayout.BeginHorizontal();
        ClearBoardButton();
        FillUpWithRAdomLetterButton();
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        _dataList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(GameDataInstance);
        }
    }

    private void DrawColumsRowsInputField()
    {
        var columnsTemp = GameDataInstance.Colums;
        var rowsTemp = GameDataInstance.Rows;

        GameDataInstance.Colums = EditorGUILayout.IntField("Columns", GameDataInstance.Colums);
        GameDataInstance.Rows = EditorGUILayout.IntField("Rows", GameDataInstance.Rows);

        if ((GameDataInstance.Colums != columnsTemp || GameDataInstance.Rows != rowsTemp)
            && GameDataInstance.Colums > 0 && GameDataInstance.Rows > 0)
        {
            GameDataInstance.CreatNewBoard();
        }
    }

    private void DrawBoardTable()
    {
        var tablestyle = new GUIStyle("box")
        {
            padding = new RectOffset(10, 10, 10, 10),
            margin = { left = 32 }
        };
        var headerColumnStyle = new GUIStyle
        {
            fixedWidth = 35
        };
        var columnstyle = new GUIStyle
        {
            fixedWidth = 50
        };
        var rowStyle = new GUIStyle
        {
            fixedHeight = 25,
            fixedWidth = 40,
            alignment = TextAnchor.MiddleCenter
        };
        var textfieldStyle = new GUIStyle
        {
            normal =
            {
                background = Texture2D.grayTexture,
                textColor = Color.white
            },
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };

        if (GameDataInstance.Board == null || GameDataInstance.Board.Length == 0)
        {
            Debug.LogError("Board is not initialized properly.");
            return;
        }

        EditorGUILayout.BeginHorizontal(tablestyle);

        for (int x = 0; x < GameDataInstance.Colums; x++)
        {
            if (x >= GameDataInstance.Board.Length || GameDataInstance.Board[x] == null)
            {
                Debug.LogError($"Board[{x}] is not initialized properly.");
                continue;
            }

            EditorGUILayout.BeginVertical(x == -1 ? headerColumnStyle : columnstyle);
            for (var y = 0; y < GameDataInstance.Rows; y++)
            {
                if (y >= GameDataInstance.Board[x].Row.Length)
                {
                    Debug.LogError($"Board[{x}].Row[{y}] is not initialized properly.");
                    continue;
                }

                EditorGUILayout.BeginHorizontal(rowStyle);
                var character = EditorGUILayout.TextArea(GameDataInstance.Board[x].Row[y], textfieldStyle);

                if (GameDataInstance.Board[x].Row[y].Length > 1)
                {
                    character = GameDataInstance.Board[x].Row[y].Substring(0, 1);
                }

                GameDataInstance.Board[x].Row[y] = character;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void InitializeReorderableList(ref ReorderableList list, string poperty, string listTable)
    {
        list = new ReorderableList(serializedObject, elements: serializedObject.FindProperty(poperty), draggable: true,displayHeader: true,displayAddButton: true,
            displayRemoveButton: true );
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect,listTable);

        };
        var l = list;
        list.drawElementCallback = (Rect rect, int index,bool isActive, bool isFocuesed) =>{
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(position: new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("word"),GUIContent.none);
                
        };

    }

    private void ConvertToUpperButton()
    {
        if (GUILayout.Button(text: "to Upper")){
            for(var i = 0; i < GameDataInstance.Colums; i++)
            {
                for(var j = 0;j < GameDataInstance.Rows; j++)
                {
                    var errorCuter = Regex.Matches(input: GameDataInstance.Board[i].Row[j], pattern: @"[a-z]").Count;
                    if(errorCuter > 0)
                    {
                        GameDataInstance.Board[i].Row[j] = GameDataInstance.Board[i].Row[j].ToUpper();
                    }
                }


            }

            foreach(var SearchWord in GameDataInstance.SearchingWords)
            {
                var errcounter = Regex.Matches(input: SearchWord.word, pattern: @"[a-z]").Count; 
                if(errcounter > 0)
                {
                    SearchWord.word = SearchWord.word.ToUpper();
                }
            }
        }

    }

    private void ClearBoardButton()
    {
        if(GUILayout.Button(text: "Clear Board"))
        {
            for(int i = 0;i < GameDataInstance.Colums; i++)
            {
                for(int j =0; j < GameDataInstance.Rows; j++)
                {
                    GameDataInstance.Board[i].Row[j] = " ";
                }
            }
        }
    }

    private void FillUpWithRAdomLetterButton()
    {
        if (GUILayout.Button(text: "Fill up with  Board"))
        {
            for (int i = 0; i < GameDataInstance.Colums; i++)
            {
                for (int j = 0; j < GameDataInstance.Rows; j++)
                {
                    int errorCounter = Regex.Matches(GameDataInstance.Board[i].Row[j], pattern: @"[a-zA-Z]").Count;
                    string letters = "abcdefghijklmnopqrstuvwxyz";
                    int index = UnityEngine.Random.Range(0, letters.Length);
                    if(errorCounter == 0)
                    {
                        GameDataInstance.Board[i].Row[j]= letters[index].ToString();
                    }
                }
            }
        }
    }
}
