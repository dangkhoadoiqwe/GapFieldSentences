using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace
using UnityEngine.UI;

public class SearchingWord : MonoBehaviour
{
    public TextMeshProUGUI displayedText; // Change to TextMeshProUGUI
    public Image crossline;

    private string _word;

    void Start()
    {
        if (displayedText == null)
        {
            Debug.LogError("DisplayedText is not assigned in the Inspector.");
        }

        if (crossline == null)
        {
            Debug.LogError("Crossline is not assigned in the Inspector.");
        }
    }

    private void OnEnable()
    {
        GameEvent.OnCorrectWord += CorrectWord;
    }

    private void OnDisable()
    {
        GameEvent.OnCorrectWord -= CorrectWord;
    }

    public void SetWord(string word)
    {
        _word = word;
        if (displayedText != null)
        {
            displayedText.text = word;
        }
        else
        {
            Debug.LogError("DisplayedText is not assigned!");
        }
    }

    private void CorrectWord(string word, List<int> squareIndexes)
    {
        if (word == _word)
        {
            if (crossline != null)
            {
                crossline.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Crossline is not assigned!");
            }
        }
    }
}
