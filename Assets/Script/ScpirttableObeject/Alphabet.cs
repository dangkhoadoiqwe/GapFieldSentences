using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class Alphabet : ScriptableObject
{
    [System.Serializable]
    public class LetterData
    {
        public string letter;
        public Sprite image;
    }
    public List<LetterData> AlphabetPlain = new List<LetterData>();

    public List<LetterData> AlphabetNomal = new List<LetterData>();

    public List<LetterData> AlphabetHightlighted = new List<LetterData>();

    public List<LetterData> AlphabetWrong = new List<LetterData>();
}

