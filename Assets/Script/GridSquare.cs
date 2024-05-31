using System.Collections.Generic;
using UnityEngine;
using static GameEvent;

public class GridSquare : MonoBehaviour
{
    public int squareIndex { get; set; }
    public Alphabet.LetterData _normalletterData;
    public Alphabet.LetterData _selectetterData;
    public Alphabet.LetterData _correctletterData;
    private SpriteRenderer _displayImage;
    private bool _select;
    private bool _clicked;
    private int _index = -1;
    private bool _correct;
    void Start()
    {
        _select = false;
        _clicked = false;
        _correct= false;
       _displayImage = GetComponent<SpriteRenderer>();
    }
    public void SetIndex(int index)
    {
        _index = index;
    }
    public int GetIndex()
    {
        return _index;
    }
    private void OnEnable()
    {
        GameEvent.OnEnableSquareSelection += OnEnableSquareSelection;
        GameEvent.OnDisableSquareSelection += OnDisableSquareSelection;
        GameEvent.OnSelectSquare += SelectSquare;
        GameEvent.OnCorrectWord += CorrectWord;
    }
    private void OnDisable()
    {

        GameEvent.OnEnableSquareSelection -= OnEnableSquareSelection;
        GameEvent.OnDisableSquareSelection -= OnDisableSquareSelection;
        GameEvent.OnSelectSquare -= SelectSquare;
        GameEvent.OnCorrectWord -= CorrectWord;
    }
    private void CorrectWord(string word, List<int> squareIndexes)
    {
        if (_select && squareIndexes.Contains(_index))
        {
            _correct = true;
            _displayImage.sprite = _correctletterData.image;
        }
        _select = false;
        _clicked = false;
    }

    public void OnEnableSquareSelection()
    {
        _clicked = true;
        _select = false;
    }
    public void OnDisableSquareSelection()
    {
        _clicked = false;
        _select = false;
        if (_correct == true)
        {
            _displayImage.sprite = _correctletterData.image;
        }
        else
        {
            _displayImage.sprite = _normalletterData.image;
        }
    }
    public void SelectSquare(Vector3 position)
    {
        if(this.gameObject.transform.position == position)
        {
            _displayImage.sprite = _selectetterData.image;
        }

    }
    public void SetSprite(Alphabet.LetterData normalletterData, Alphabet.LetterData selectetterData, Alphabet.LetterData correctletterData)
    {
        _normalletterData = normalletterData;
        _selectetterData = selectetterData;
        _correctletterData = correctletterData;
        GetComponent<SpriteRenderer>().sprite = _normalletterData.image;
    }

    private void OnMouseDown()
    {
        OnEnableSquareSelection();
        GameEvent.EnableSqaureSelectionMethod();
        CheckSquare();
        _displayImage.sprite =_selectetterData.image;

    }

    private void OnMouseEnter()
    {
        CheckSquare();

    }
    private void OnMouseUp()
    {
        GameEvent.ClearSelectionMethod();
        GameEvent.DisableSqaureSelectionMethod() ;
    }
    
    public void CheckSquare()
    {
        if( _select == false && _clicked ==true)
        {
            _select = true;
            GameEvent.CheckSquareMethod(_normalletterData.letter,gameObject.transform.position,_index);

        }
    }
}
