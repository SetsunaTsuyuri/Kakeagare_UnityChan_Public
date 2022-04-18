using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// セーブデータ
/// </summary>
[System.Serializable]
public class SaveData
{
    /// <summary>
    /// ハイスコア
    /// </summary>
    public int highScore = 0;

    /// <summary>
    /// ハイスコアを登録する
    /// </summary>
    /// <param name="score">スコア</param>
    public void Register(int score)
    {
        highScore = score;
    }
}
