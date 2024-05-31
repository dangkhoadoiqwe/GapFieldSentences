using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BoardData;

[System.Serializable]
[CreateAssetMenu]

 
public class SelectBoardData
{
    public int Rows;
    public int Colums;
    public List<BoardRow> Board;
}
[System.Serializable]
public class GameData : ScriptableObject
{
    public string selectCategoryName;
    public BoardData selectboardData;
    public SelectBoardData selectBoardData;
}
