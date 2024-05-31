using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordScript : MonoBehaviour
{
    public GameData currentgameData;  // Assign in Inspector
    public GameObject gridsquarePrefab;  // Assign in Inspector
    public Alphabet alphabet;  // Assign in Inspector
    public float squareoffset = 0.0f;
    public float topPosition;
    public List<GameObject> squarelist = new List<GameObject>();

    void Start()
    {
        if (gridsquarePrefab == null)
        {
            Debug.LogError("gridsquarePrefab is not assigned");
            return;
        }

        SpamGridSquares();
        if (squarelist.Count == 0)
        {
            Debug.LogError("No squares to position.");
            return;
        }
        SetSquarePositioning();
    }

    private void SetSquarePositioning()
    {
        if (squarelist.Count == 0)
        {
            Debug.LogError("No squares to position.");
            return;
        }

        var squareRect = squarelist[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = squarelist[0].GetComponent<Transform>();
        var offset = new Vector2
        {
            x = (squareRect.width * squareTransform.localScale.x + squareoffset) * 0.01f,
            y = (squareRect.height * squareTransform.localScale.y + squareoffset) * 0.01f
        };

        var startPosition = GetFirstSquare();
        int columnNumber = 0;
        int rowNumber = 0;
        foreach (var square in squarelist)
        {
            if (rowNumber + 1 > currentgameData.selectboardData.Rows)
            {
                columnNumber++;
                rowNumber = 0;
            }

            var positionX = startPosition.x + offset.x * columnNumber;
            var positionY = startPosition.y - offset.y * rowNumber;
            square.GetComponent<Transform>().position = new Vector2(positionX, positionY);
            rowNumber++;
        }
    }

    private Vector2 GetFirstSquare()
    {
        var startPosition = new Vector2(0f, transform.position.y);
        var squareRect = squarelist[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = squarelist[0].GetComponent<Transform>();
        var squareSize = new Vector2
        {
            x = squareRect.width * squareTransform.localScale.x,
            y = squareRect.height * squareTransform.localScale.y
        };

        var midWidthPosition = (((currentgameData.selectboardData.Colums - 1) * squareSize.x) / 2) * 0.01f;
        var midHeightPosition = (((currentgameData.selectboardData.Rows - 1) * squareSize.y) / 2) * 0.01f;
        startPosition.x = (midWidthPosition != 0) ? midWidthPosition * -1 : midWidthPosition;
        startPosition.y += midHeightPosition;

        return startPosition;
    }

    private void SpamGridSquares()
    {
        if (currentgameData == null)
        {
            Debug.LogError("currentgameData is null");
            return;
        }

        if (gridsquarePrefab == null)
        {
            Debug.LogError("gridsquarePrefab is not assigned");
            return;
        }

        var squareScale = GetSquareScale(new Vector3(1.5f, 1.5f, 0.1f));

        foreach (var row in currentgameData.selectboardData.Board)
        {
            if (row.Row == null)
            {
                Debug.LogError("Row is null in Board");
                continue;
            }

            foreach (var squareLetter in row.Row)
            {
                var normalLetterData = alphabet.AlphabetNomal?.Find(data => data.letter == squareLetter);
                var selectLetterData = alphabet.AlphabetHightlighted?.Find(data => data.letter == squareLetter);
                var correctLetterData = alphabet.AlphabetWrong?.Find(data => data.letter == squareLetter);

                if (normalLetterData?.image == null || selectLetterData?.image == null || correctLetterData?.image == null)
                {
                    Debug.LogError($"Missing image for letter: {squareLetter}");
#if UNITY_EDITOR
                    if (UnityEditor.EditorApplication.isPlaying)
                    {
                        UnityEditor.EditorApplication.isPlaying = false;
                    }
#endif
                    return;
                }

                var newSquare = Instantiate(gridsquarePrefab);
                if (newSquare == null)
                {
                    Debug.LogError("Failed to instantiate gridSquarePrefab");
                    return;
                }

                var gridSquareComponent = newSquare.GetComponent<GridSquare>();
                if (gridSquareComponent == null)
                {
                    Debug.LogError("GridSquare component not found on instantiated prefab");
                    Destroy(newSquare); // Clean up the instantiated prefab
                    return;
                }

                squarelist.Add(newSquare);
                gridSquareComponent.SetSprite(normalLetterData, selectLetterData, correctLetterData);
                newSquare.transform.SetParent(this.transform);
                newSquare.transform.localPosition = Vector3.zero;
                squarelist[squarelist.Count - 1].GetComponent<GridSquare>().SetIndex(squarelist.Count - 1);
                //newSquare.transform.localScale = squareScale;
            }
        }
    }


    private Vector3 GetSquareScale(Vector3 defaultScale)
    {
        var findScale = defaultScale;
        var ad = 0.01f;

        while (ShouldScaleDown(findScale))
        {
            findScale.x = ad;
            findScale.y = ad;
            if (findScale.x < 0 || findScale.y <= 0)
            {
                findScale.x = ad;
                findScale.y = ad;
                return findScale;
            }
        }
        return findScale;
    }

    private bool ShouldScaleDown(Vector3 scale)
    {
        var squareRect = gridsquarePrefab.GetComponent<SpriteRenderer>().sprite.rect;
        var squareSize = new Vector2
        {
            x = (squareRect.width * scale.x) + squareoffset,
            y = (squareRect.height * scale.y) + squareoffset
        };

        var midWidthPosition = ((currentgameData.selectboardData.Colums * squareSize.x) / 2) * 0.01f;
        var midHeightPosition = ((currentgameData.selectboardData.Rows * squareSize.y) / 2) * 0.01f;

        var startPosition = new Vector2
        {
            x = (midWidthPosition != 0) ? midWidthPosition * -1 : midWidthPosition,
            y = midHeightPosition
        };

        return startPosition.x < GetHalfScreenWidth() * -1 || startPosition.y > topPosition;
    }

    private float GetHalfScreenWidth()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = (1.7f * height) * Screen.width / Screen.height;
        return width / 2;
    }
}
