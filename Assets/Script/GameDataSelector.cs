using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataSelector : MonoBehaviour
{
    public GameData currentGameData;
    public GameLevelData levelData;

    void Awake()
    {
        if (currentGameData == null || levelData == null)
        {
            Debug.LogError("GameData or GameLevelData is not assigned!");
            return;
        }

        SelectSequentialBoardData();
    }

    private void SelectSequentialBoardData()
    {
        foreach (var data in levelData.data)
        {
            if (data.categoryName == currentGameData.selectCategoryName)
            {
                var boardIndex = DataSaver.ReadCatologryIndexValue(currentGameData.selectCategoryName); // Ensure this method works as expected

                if (boardIndex < data.boardData.Count)
                {
                    currentGameData.selectboardData = data.boardData[boardIndex];
                }
                else
                {
                    Debug.LogWarning($"Board index {boardIndex} is out of bounds. Selecting a random board instead.");
                    var randomIndex = Random.Range(0, data.boardData.Count);
                    currentGameData.selectboardData = data.boardData[randomIndex];
                }

                return; // Exit loop once the correct category is found and processed
            }
        }

        Debug.LogError($"Category '{currentGameData.selectCategoryName}' not found in level data.");
    }
}
