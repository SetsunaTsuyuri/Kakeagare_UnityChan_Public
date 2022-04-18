using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // 得点の表示テキスト
    [SerializeField]
    Text scoreText = null;
    public Text ScoreText
    {
        get
        {
            return scoreText;
        }
    }

    // Playerコンポーネント
    [SerializeField]
    Player player = null;

    // Animatorコンポーネント
    Animator animator = null;

    private void Awake()
    {
        // 子からAnimatorコンポーネントを取得する
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        // 得点が増加した場合
        if (CalculateIncreaseValueOfScore() > 0)
        {
            // 得点の表示を更新する
            UpdateScoreText();
        }

    }

    /// <summary>
    /// 得点の表示を更新する
    /// </summary>
    private void UpdateScoreText()
    {
        // テキストを更新する
        ScoreText.text = player.Score.ToString();
    }

    /// <summary>
    /// スコアの増加量を計算する
    /// </summary>
    /// <returns></returns>
    private int CalculateIncreaseValueOfScore()
    {
        int result = 0;

        // 表示されているスコアの数値
        int previousUpdateScore = int.Parse(scoreText.text);

        // 実際のスコア
        int actualScore = player.Score;

        if (actualScore > previousUpdateScore)
        {
            result = actualScore - previousUpdateScore;
        }

        return result;
    }
}