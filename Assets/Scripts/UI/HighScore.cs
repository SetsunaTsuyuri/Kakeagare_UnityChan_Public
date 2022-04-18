using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    /// <summary>
    /// ハイスコアの表示テキスト
    /// </summary>
    public Text ScoreText { get; private set; } = null;

    SaveManager saveManager = null;

    readonly string message = "ハイスコア：{0}"; 

    private void Awake()
    {
        ScoreText = GetComponent<Text>();
    }

    private void Start()
    {
        saveManager = SaveManager.Instance;
    }

    private void Update()
    {
        UpdateText();
    }

    /// <summary>
    /// テキスト更新
    /// </summary>
    private void UpdateText()
    {
        int highScore = saveManager.SaveData.highScore;

        ScoreText.text =  string.Format(message, highScore.ToString());
    }
}
