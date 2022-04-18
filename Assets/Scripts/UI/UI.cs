using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    // プレイヤー
    [SerializeField]
    Player player = null;

    // HPバーの増減の速さ
    [SerializeField]
    float hpBarAnimationSpeed = 3.0f;

    // プレイヤーのHPバー
    [SerializeField]
    Slider hpBar = null;

    // 入手したさくらんぼの数の表示テキスト
    [SerializeField]
    Text acquiredCherryCountText = null;

    // タイマーの表示テキスト
    [SerializeField]
    Text timerText = null;

    private void Start()
    {
        // HPバーの表示を満タンにする 
        hpBar.maxValue = player.MaxHp;
        hpBar.value = player.CurrentHp;
    }

    private void LateUpdate()
    {
        // プレイ時間の表示を更新する
        UpdatePlayTime();

        // プレイヤーのHPバーの表示を更新する
        UpdateHpBar();

        // プレイヤーのアイテム入手数の表示が反映されていない場合、その表示を更新する
        if (!player.AcquiredItemsIsReflectedOnUI)
        {
            UpdateAcquiredItemsText();
        }
    }

    /// <summary>
    /// プレイ時間の表示を更新する
    /// </summary>
    private void UpdatePlayTime()
    {
        // プレイ時間を取得する
        float playTime = player.PlayTimeFromStartToGoal;

        // (分:秒:1/100秒)の形にする
        int minutes = Mathf.FloorToInt(playTime / 60.0f);
        int seconds = Mathf.FloorToInt(playTime - minutes * 60);
        int tenMilliSeconds = Mathf.FloorToInt((playTime - minutes * 60 - seconds) * 100);
        string time = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, tenMilliSeconds);

        // テキストを更新する
        timerText.text = time;
    }

    /// <summary>
    /// HPバーの表示を更新する
    /// </summary>
    private void UpdateHpBar()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, player.CurrentHp, hpBarAnimationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 入手したアイテムの数の表示を更新する
    /// </summary>
    private void UpdateAcquiredItemsText()
    {
        // テキストを更新する
        acquiredCherryCountText.text = string.Format("x {0, 3}", player.AcquiredItems);

        // アイテム入手数変化反映フラグON
        player.AcquiredItemsIsReflectedOnUI = true;
    }
}
