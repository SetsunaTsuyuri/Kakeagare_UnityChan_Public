using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownUI : MonoBehaviour
{
    // カウントがゼロの場合に表示する文字列
    [SerializeField]
    string countZeroTextMessage = "GO!";

    // カウントがゼロの場合に再生する効果音プレファブ
    [SerializeField]
    GameObject countZeroSEPrefab = null;

    // カウントダウンで表示が切り替わるたびに再生する効果音プレファブ
    [SerializeField]
    GameObject countDownSEPrefab = null;

    // ゲームスタート何秒前からカウント演出を始めるか
    [SerializeField]
    float countStartSeconds = 3.0f;

    // CountDownLogicコンポーネント
    [SerializeField]
    CountDownLogic countDownLogic = null;

    // カウントダウンの表示が変化したか
    bool changeText = false;

    // Textコンポーネント
    Text text = null;

    // Animatorコンポーネント
    Animator animator = null;

    private void Awake()
    {
        // Textコンポーネントを取得する
        text = GetComponent<Text>();

        // Animatorコンポーネントを取得する
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        // カウントダウンの残り時間
        float seconds = countDownLogic.Seconds;

        // カウントダウンの表示を更新する
        UpdateCountDownText(seconds);

        // カウントダウンのテキストの表示が変わった場合
        if (changeText)
        {
            // フラグOFF
            changeText = false;

            // アニメ再生
            animator.Play(0);

            // 効果音再生
            PlayCountDownSE(seconds);
        }

        // カウントダウンが0の場合
        if (seconds == 0.0f)
        {
            // このコンポーネントを使用不可にする
            enabled = false;
        }
    }

    /// <summary>
    /// カウントダウンの表示を更新する
    /// </summary>
    private void UpdateCountDownText(float seconds)
    {
        // 現在のカウントダウン残り時間がカウント表示開始秒数より大きければ中止する
        if (seconds > countStartSeconds)
        {
            return;
        }

        // 更新前のテキストの内容
        string previousUpdateMessage = text.text;

        // 現在の残り秒数(端数切り上げ)の一時変数
        string tempMessage = Mathf.CeilToInt(seconds).ToString();

        // 前回のテキストと内容が異なる場合
        if (!tempMessage.Equals(previousUpdateMessage))
        {
            // 0秒の場合
            if (seconds == 0.0f)
            {
                // 0秒用のテキスト
                text.text = countZeroTextMessage;
            }
            else
            {
                // 現在の残り秒数
                text.text = tempMessage;
            }

            // テキスト変更フラグON
            changeText = true;
        }
    }

    /// <summary>
    /// カウントダウン効果音を再生する
    /// </summary>
    /// <param name="seconds"></param>
    private void PlayCountDownSE(float seconds)
    {

        if (countZeroSEPrefab != null && seconds == 0.0f)
        {
            //カウントゼロ効果音を再生する
            Instantiate(countZeroSEPrefab);
        }
        else if (countDownSEPrefab != null && seconds > 0.0f)
        {
            // カウントダウン効果音を再生する
            Instantiate(countDownSEPrefab);
        }
    }
}