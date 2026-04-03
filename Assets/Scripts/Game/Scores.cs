using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BestScoreData
{
    public int score = 0;
}

public class Scores : MonoBehaviour
{
 public Text ScoreText;

 private bool newBestScore = false;
 private BestScoreData bestScores = new BestScoreData();
 private int currentScores_;

private string bestScoreKey_ = "bsdat";

private void Awake()
{
    if(BinaryDataStream.Exist(bestScoreKey_))
    {
        StartCoroutine(ReadDataFile());
    }
}
private IEnumerator ReadDataFile()
{
    bestScores = BinaryDataStream.Read<BestScoreData>(bestScoreKey_);
    yield return new WaitForEndOfFrame();
    Debug.Log("Read Best Scores = " + bestScores.score);
}
 void Start()
    {
        currentScores_ = 0;
        newBestScore = false;
        UpdateScoreText();
       
    }
    private void OnEnable(){
        GameEvent.AddScores += AddScores;
        GameEvent.GameOver += SaveBestScores;
    }
    private void OnDisable(){
        GameEvent.AddScores -= AddScores;
        GameEvent.GameOver -= SaveBestScores;
    }

    public void SaveBestScores(bool newBestScores)
    {
        BinaryDataStream.Save<BestScoreData>(bestScores, bestScoreKey_);
    }

    private void AddScores(int scores)
    {
        currentScores_ += scores;
        if(currentScores_ > bestScores.score)
        {
            bestScores.score = currentScores_;
            newBestScore = true;
        }

        GameEvent.UpdateBestScoreBar(currentScores_, bestScores.score);
        UpdateScoreText();
    }
    private void UpdateScoreText()
    {
        ScoreText.text = currentScores_.ToString();
    }
}
