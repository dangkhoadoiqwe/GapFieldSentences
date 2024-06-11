using System.Collections;
using System.Collections.Generic;
using TMPro; // Ensure this namespace is included
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectPuzzleButton : MonoBehaviour
{
    public GameData gameData;
    public GameLevelData levelData;
    public TextMeshProUGUI categoryText;
    public Image progressBarFilling;

    private string gameSceneName = "GameScene";

    private bool _levelLock;

    void Start()
    {
        _levelLock = false;

        var button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Button component is missing!");
            return;
        }

        button.onClick.AddListener(OnButtonClick);
        UpdateButtonInformation();

        button.interactable = !_levelLock;
    }

    void UpdateButtonInformation()
    {
        int currentIndex = -1;
        int totalBoards = 0;

        foreach (var data in levelData.data)
        {
            if (data.categoryName == gameObject.name)
            {
                currentIndex = DataSaver.ReadCatologryIndexValue(gameObject.name);
                totalBoards = data.boardData.Count;

                if (levelData.data[0].categoryName == gameObject.name && currentIndex < 0)
                {
                    DataSaver.SaveCatologryData(levelData.data[0].categoryName, 0);
                    currentIndex = DataSaver.ReadCatologryIndexValue(gameObject.name);
                    totalBoards = data.boardData.Count;
                }
            }
        }

        if (currentIndex == -1)
            _levelLock = true;

        categoryText.text = _levelLock ? string.Empty : $"{currentIndex}/{totalBoards}";
        progressBarFilling.fillAmount = (currentIndex > 0 && totalBoards > 0) ? (float)currentIndex / totalBoards : 0f;
    }

    void OnButtonClick()
    {
        gameData.selectCategoryName = gameObject.name;
        SceneManager.LoadScene(gameSceneName);
    }
}
