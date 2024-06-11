using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPopup : MonoBehaviour
{
    public GameObject winPopup;

    void Start()
    {
        if (winPopup == null)
        {
            Debug.LogError("WinPopup GameObject is not assigned!");
            return;
        }

        winPopup.SetActive(false);
    }

    private void OnEnable()
    {
        Debug.Log("Subscribing to OnBoardComplete");
        GameEvent.OnBoardComplete += ShowWinPopup;
    }

    private void OnDisable()
    {
        Debug.Log("Unsubscribing from OnBoardComplete");
        GameEvent.OnBoardComplete -= ShowWinPopup;
    }

    private void ShowWinPopup()
    {
        if (winPopup != null)
        {
            Debug.Log("ShowWinPopup called");
            winPopup.SetActive(true);
        }
        else
        {
            Debug.LogError("WinPopup GameObject is not assigned!");
        }
    }

    public void LoadNextLevel()
    {
        Debug.Log("Loading next level");
        GameEvent.LoadNextLevelMethod();
    }
}
