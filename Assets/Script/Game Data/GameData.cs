using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData
{
  public int highScore;
    public int currentScore;
    
    // Lưu ý quan trọng: JsonUtility của Unity không hỗ trợ lưu mảng 2 chiều (int[,]).
    // Vì vậy, với bàn cờ 8x8, chúng ta sẽ dùng mảng 1 chiều 64 phần tử.
    public int[] boardState; 
    
    public int coins;

    // Hàm khởi tạo (Constructor) thiết lập giá trị mặc định khi người chơi mới tải game
    public GameData()
    {
        highScore = 0;
        currentScore = 0;
        boardState = new int[64]; // Tạo sẵn mảng trống 64 ô
        coins = 0;
    }
}
