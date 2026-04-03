using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvent : MonoBehaviour
{
    public static Action<bool> GameOver;
   public static Action<int> AddScores;

    public static Action CheckIfShapeCanBePlaced;
    public static Action MoveShapeToStartPosition;

    public static Action RequestNewShape;

    public static Action SetShapeInactive;
    
    public static Action<int , int> UpdateBestScoreBar;
}
