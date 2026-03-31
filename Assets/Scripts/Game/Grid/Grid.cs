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

    private Vector2 _offset = new Vector2(0.0f, 0.0f);
    private List<GameObject> _gridSquares = new List<GameObject>();

    private LineIndicator _lineIndicator;
    private void OnEnable(){
        GameEvent.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
    }

    private void OnDisable(){
        GameEvent.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
    }

    void Start()
    {
       _lineIndicator = GetComponent<LineIndicator>();
        CreateGrid();
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
            _gridSquares[squareIndex].GetComponent<GridSquare>().PlaceShapeOnBoard();
        }

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
    //square
    for(var square =0 ; square < 9; square++)
    {
        List<int> data = new List<int> (9);
        for(var index=0; index <9; index++)
            {
                data.Add(_lineIndicator.square_data[index,square]);
            }
            Lines.Add(data.ToArray());
    }

    
    var completedLines = CheckIfSquareAreCompleted(Lines);
    if (completedLines > 2)
    {
        //TODO : Play bonus
    }
   
   var totalScores =10 * completedLines;
        GameEvent.AddScores(totalScores);

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
}
