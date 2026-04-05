using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public ShapeStorage shapeStorage;
    public int columns = 0;
    public int rows = 0;
    public float squaresGap = 0.1f;
    public GameObject gridSquare;
    public Vector2 startPosition = new Vector2(0.0f, 0.0f);
    public float squareScale = 0.5f;
    public float everySquareOffset = 0.0f;
    public SquareTextureData squareTextureData;

    private Vector2 _offset = new Vector2(0.0f, 0.0f);
    private List<GameObject> _gridSquares = new List<GameObject>();

    private LineIndicator _lineIndicator;

    private Config.SquareColor currentActiveSquareColor = Config.SquareColor.NotSet;
    private List<Config.SquareColor> colorsInTheGrid = new List<Config.SquareColor>();
    private void OnEnable()
    {
        GameEvent.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
        GameEvent.UpdateSquareColor += OnUpdateSquareColor;
    }

    private void OnDisable(){
        GameEvent.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
        GameEvent.UpdateSquareColor -= OnUpdateSquareColor;
    }

    void Start()
    {
       _lineIndicator = GetComponent<LineIndicator>();
        CreateGrid();
        currentActiveSquareColor = squareTextureData.activeSquareTextures[0].squareColor;
    }

    private void OnUpdateSquareColor(Config.SquareColor color)
    {
        currentActiveSquareColor = color;
    }

    private List<Config.SquareColor> GetAllColorsInTheGrid()
    {
        var colors = new List<Config.SquareColor>();
        foreach(var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();
            if(gridSquare.SquareOccupied)
            {
                var color = gridSquare.GetCurrentColor();
                if(colors.Contains(color) == false)
                {
                    colors.Add(color);
                }
            }
        }
        return colors;
    }

    

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquaresPosition();
    }

   private void SpawnGridSquares()
   {
    int square_index = 0;
    int totalSquares = rows * columns;
    for(var i = 0; i < totalSquares; ++i){
        _gridSquares.Add(Instantiate(gridSquare) as GameObject);

        _gridSquares[_gridSquares.Count -1].GetComponent<GridSquare>().SquareIndex = square_index;
        _gridSquares[_gridSquares.Count -1].transform.SetParent(this.transform);
        _gridSquares[_gridSquares.Count -1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
        _gridSquares[_gridSquares.Count -1].GetComponent<GridSquare>().SetImage(_lineIndicator.GetGridLineIndex(square_index) % 2 == 0);
        square_index++;
        
    }
   }

   private void SetGridSquaresPosition()
   {
    int column_number = 0;
    int row_number = 0;
    Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
    bool row_moved = false;
    
    var square_rect = _gridSquares[0].GetComponent<RectTransform>();
    
    _offset.x = square_rect.rect.width * square_rect.transform.localScale.x + everySquareOffset;
    _offset.y = square_rect.rect.height * square_rect.transform.localScale.y + everySquareOffset;
    
    foreach (GameObject square in _gridSquares)
    {
        if(column_number >= columns)
        {
            square_gap_number.x = 0;
            //next colums
            column_number = 0;
            row_number++;
            row_moved = false;
        }

        if(column_number > 0 && column_number % 3 == 0)
        {
            square_gap_number.x++;
        }

        if(row_number > 0 && row_number % 3 == 0 && row_moved == false)
        {
            row_moved = true;
            square_gap_number.y++;
        }

        var pos_x_offset = _offset.x * column_number + (square_gap_number.x * squaresGap);
        var pos_y_offset = _offset.y * row_number + (square_gap_number.y * squaresGap);

        square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset);
        
        square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset, 0.0f);
        column_number++;
    }
    
   }

   private void CheckIfShapeCanBePlaced(){  

    var squareIndexes = new List<int>();
    foreach(var square in _gridSquares){
        var gridSquare = square.GetComponent<GridSquare>();
        if(gridSquare.Selected && !gridSquare.SquareOccupied){
            squareIndexes.Add(gridSquare.SquareIndex);
            gridSquare.Selected = false;
            //gridSquare.ActivateSquare();
        }
    }
    
    var currentSelectedShape = shapeStorage.GetCurrentSelectedShape();
    if(currentSelectedShape == null) return;

    if(currentSelectedShape.TotalSquareNumber == squareIndexes.Count){
        foreach(var squareIndex in squareIndexes){
            _gridSquares[squareIndex].GetComponent<GridSquare>().PlaceShapeOnBoard(currentActiveSquareColor);
        }

            // ---> THÊM ĐOẠN NÀY ĐỂ PHÁT ÂM THANH VÀ RUNG KHI ĐẶT KHỐI <---
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.placeBlockClip);
                AudioManager.Instance.Vibrate();
            }
            // -----------------------------------------------------------

            var shapeLeft = 0;

        foreach (var shape in shapeStorage.shapeList)
        {
            if (shape.IsOnStartPosition() && shape.IsAnyOfShapeSquareActive())
            {
                shapeLeft++;
            }
        }
            if (shapeLeft == 0)
            {
                GameEvent.RequestNewShape();
            }
            else
            {
                GameEvent.SetShapeInactive();
            }

            CheckIfAnyLinesIsCompleted();
            CheckIfPlayerLost();
        }
    else{
        GameEvent.MoveShapeToStartPosition();
    }

   }
   void CheckIfAnyLinesIsCompleted()
   {
    List<int[]> Lines = new List<int[]>();
    //columns
    foreach (var column in _lineIndicator.columnIndexes)
    {
       Lines.Add(_lineIndicator.GetVerticalLine(column));
    }
    // rows
    for(var row =0; row < rows; row++)
    {
        List<int> data = new List<int> (9);
        for(var index =0; index <9; index++)
            {
                data.Add(_lineIndicator.line_data[row,index]);
            }
        Lines.Add(data.ToArray());
    }
//CÃ¡i nÃ y nÃªn gá»i trÆ°á»›c khi CheckIfSquareAreCompleted
    colorsInTheGrid = GetAllColorsInTheGrid();

    var completedLines = CheckIfSquareAreCompleted(Lines);

        // ---> THÊM ĐOẠN NÀY ĐỂ PHÁT ÂM THANH KHI ĂN ĐIỂM <---
        if (completedLines > 0 && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.clearLineClip);
        }
        // ---------------------------------------------------

        if (completedLines >= 2)
    {
      GameEvent.ShowCongratulationWritings();
    }
   
   var totalScores =10 * completedLines * completedLines;
   var bonusScores = ShouldPlayColorBonusAnimation();
    GameEvent.AddScores(totalScores + bonusScores);

   }

   private int ShouldPlayColorBonusAnimation()
   {
    var colorsInTheGridAfterLineRemoved = GetAllColorsInTheGrid();
    Config.SquareColor colorToPlayBonusFor = Config.SquareColor.NotSet;

    foreach (var squareColor in colorsInTheGrid)
    {
        if (colorsInTheGridAfterLineRemoved.Contains(squareColor) == false)
        {
            colorToPlayBonusFor = squareColor;
        }
    }
    if(colorToPlayBonusFor == Config.SquareColor.NotSet)
    {
        Debug.Log("Cannot find color for bonus");
        return 0;
    }

    //KhÃ´ng nÃªn cháº¡y bonus cho mÃ u hiá»‡n táº¡i
    if (colorToPlayBonusFor == currentActiveSquareColor)
    {
        return 0;
    }

    GameEvent.ShowBonusScreen(colorToPlayBonusFor);
    return 50;
   }
       private int CheckIfSquareAreCompleted(List<int[]> data)
    {
        List<int[]>completeLines = new List<int[]>();
        var linesComplete = 0;

        foreach (var line in data)
        {
            var lineComplete = true;
            foreach(var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                if (comp.SquareOccupied == false)
                {
                    lineComplete = false;
                    
                }
            }
            if(lineComplete)
            {
                completeLines.Add(line);
                
            }
        }
        foreach(var Line in completeLines)
        {
            var complete = false;
            foreach(var squaIndex in Line)
            {
                var comp = _gridSquares[squaIndex].GetComponent<GridSquare>();
                comp.Deactivate();
                complete = true;
            }
           
            foreach(var squaIndex in Line)
            {
                var comp = _gridSquares[squaIndex].GetComponent<GridSquare>();
                comp.ClearOccupied();
            }

            if (complete)
            {
                linesComplete++;
            }
        }
        return linesComplete;

    }
     private void CheckIfPlayerLost()
    {
        var validShapes = 0;
        for(var index =0; index < shapeStorage.shapeList.Count; index++)
        {
            var isShapeActive = shapeStorage.shapeList[index].IsAnyOfShapeSquareActive();
           if(CheckIfShapeCanBePlacedOnGrid(shapeStorage.shapeList[index]) && isShapeActive)
            {
                shapeStorage.shapeList[index]?.ActivateShape();
                validShapes++;
            }
        }
        if(validShapes == 0)
        {
            GameEvent.GameOver?.Invoke(false);
            Debug.Log("Game Over");
        }
    }
    private bool CheckIfShapeCanBePlacedOnGrid(Shape currentShape)
    {
        var currentShapeData = currentShape.CurrentShapeData;
        var ShapeColumns = currentShapeData.columns;
        var shapeRows = currentShapeData.rows;

        List<int> originalShapeFilledUpSquares = new List<int>();
        var squareIndex = 0;
        for (var rowIndex = 0; rowIndex < shapeRows; rowIndex++)
        {
            for (var columnIndex = 0; columnIndex < ShapeColumns; columnIndex++)
            {
                if (currentShapeData.board[rowIndex].column[columnIndex])
                {
                    originalShapeFilledUpSquares.Add(squareIndex);
                }
                squareIndex++;
            }
        }

        if(currentShape.TotalSquareNumber != originalShapeFilledUpSquares.Count){
            Debug.LogError("Number of filled up squares are not the same as the original shape have");
        }

        var squareList = GetAllSquaresCombination(ShapeColumns, shapeRows);

        bool canBePlaced = false;
        foreach(var number in squareList)
        {
            bool shapeCanBePlacedOnTheBoard = true;
            foreach(var squareIndexToCheck in originalShapeFilledUpSquares)
            {
                var comp = _gridSquares[number[squareIndexToCheck]].GetComponent<GridSquare>();
                if(comp.SquareOccupied )
                {
                    shapeCanBePlacedOnTheBoard = false;
                }
            }

            if (shapeCanBePlacedOnTheBoard)
            {
                canBePlaced = true;
            }
        }

        return canBePlaced;
    }
    private List<int[]> GetAllSquaresCombination(int columns, int rows)
    {
     var squareList = new List<int[]>();

      var lastColumnIndex = 0;
      var lastRowIndex = 0;

      int safeIndex = 0;

      while(lastRowIndex +(rows -1) < 9){
        var rowData = new List<int>();
        for(var row = lastRowIndex; row < lastRowIndex + rows; row++)
        {
            for(var column = lastColumnIndex; column < lastColumnIndex + columns; column++)
            {
                rowData.Add(_lineIndicator.line_data[row, column]);

            }
        }
        squareList.Add(rowData.ToArray());
        lastColumnIndex++;
        if(lastColumnIndex + (columns -1) >= 9)
        {
            lastColumnIndex = 0;
            lastRowIndex++;
        }
        safeIndex++;
        if(safeIndex > 100)
        {
            break;
        }

      }
      return squareList;
    }

    public void ClearLinesForRevive()
    {
        // Máº·c Ä‘á»‹nh chá»n ngáº«u nhiÃªn XÃ³a HÃ ng hoáº·c Cá»™t
        bool isRow = Random.Range(0, 2) == 0;
        int startIndex = Random.Range(0, rows - 2); // Chá»n vá»?trÃ­ báº¯t Ä‘áº§u (tá»?0 Ä‘áº¿n tá»‘i Ä‘a 6)

        List<int[]> linesToClear = new List<int[]>();

        if (isRow)
        {
            for (int r = startIndex; r < startIndex + 3; r++)
            {
                List<int> lineData = new List<int>();
                for (int c = 0; c < columns; c++)
                {
                    lineData.Add(_lineIndicator.line_data[r, c]);
                }
                linesToClear.Add(lineData.ToArray());
            }
        }
        else
        {
            for (int c = startIndex; c < startIndex + 3; c++)
            {
                linesToClear.Add(_lineIndicator.GetVerticalLine(c));
            }
        }

        // Tiáº¿n hÃ nh dá»n dáº¹p cÃ¡c khá»‘i trong 3 hÃ ng/cá»™t nÃ y
        foreach (var line in linesToClear)
        {
            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                if (comp.SquareOccupied)
                {
                    comp.Deactivate();
                    comp.ClearOccupied();
                }
            }
        }

        // Báº­t sÃ¡ng láº¡i táº¥t cáº?cÃ¡c Shape dÆ°á»›i khay (náº¿u chÃºng bá»?lÃ m má»?
        for (var index = 0; index < shapeStorage.shapeList.Count; index++)
        {
            if (shapeStorage.shapeList[index] != null)
            {
                // Chá»?kÃ­ch hoáº¡t láº¡i hÃ¬nh náº¿u nÃ³ váº«n cÃ²n trÃªn khay chá»?Ä‘áº·t
                if (shapeStorage.shapeList[index].IsOnStartPosition())
                {
                    shapeStorage.shapeList[index].ActivateShape();
                }
            }
        }

        // Cháº¡y láº¡i hÃ m kiá»ƒm tra Ä‘á»?game nháº­n diá»‡n ráº±ng ngÆ°á»i chÆ¡i Ä‘Ã£ cÃ³ thá»?tiáº¿p tá»¥c
        CheckIfPlayerLost();
    }
}
