using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Scores : MonoBehaviour
{
 public Text ScoreText;
 private int currentScores_;
 void Start()
    {
        currentScores_ = 0;
        UpdateScoreText();
       
    }
    private void OnEnable(){
        GameEvent.AddScores += AddScores;
    }
    private void OnDisable(){
        GameEvent.AddScores -= AddScores;
    }

    private void AddScores(int scores)
    {
        currentScores_ += scores;
        UpdateScoreText();
    }
    private void UpdateScoreText()
    {
        ScoreText.text = currentScores_.ToString();
    }
}
