using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour
{
    // HP割合ボーナスのメッセージ表示テキスト
    [SerializeField]
    Text hpRateBonusMessageText = null;

    // HP割合ボーナスのスコア表示テキスト
    [SerializeField]
    Text hpRateBonusValueText = null;

    // HP割合ボーナスのアニメーター
    [SerializeField]
    Animator hpRateBonusPanelAnimator = null;
    public Animator HpRateBonusPanelAnimator
    {
        get
        {
            return hpRateBonusPanelAnimator;
        }
    }

    // クリアタイムボーナスのメッセージ表示テキスト
    [SerializeField]
    Text clearTimeBonusMessageText = null;

    // クリアタイムボーナスのスコア表示テキスト
    [SerializeField]
    Text clearTimeBonusValueText = null;

    // クリアタイムボーナスのアニメーター
    [SerializeField]
    Animator clearTimeBonusPanelAnimator = null;
    public Animator ClearTimeBonusPanelAnimator
    {
        get
        {
            return clearTimeBonusPanelAnimator;
        }
    }

    // アイテム収集ボーナスのメッセージ表示テキスト
    [SerializeField]
    Text itemCollectionMessageText = null;

    // アイテム収集ボーナスのスコア表示テキスト
    [SerializeField]
    Text itemCollectionValueText = null;

    // アイテム収集ボーナスのアニメーター
    [SerializeField]
    Animator itemCollectionBonusPanelAnimator = null;
    public Animator ItemCollectionBonusPanelAnimator
    {
        get
        {
            return itemCollectionBonusPanelAnimator;
        }
    }

    //Playerコンポーネント
    [SerializeField]
    Player player = null;

    // BonusManagerコンポーネント
    [SerializeField]
    BonusManger bonusManager = null;

    // 各ボーナスの点数を表示する際に先頭に付ける文字列
    readonly string scorePrefix = "+ ";

    private void Start()
    {
        // HP割合ボーナスパネルを更新する
        UpdateHpRateBonusPanel();

        // クリアタイムボーナスパネルを更新する
        UpdateClearTimeBonusPanel();

        // アイテム収集ボーナスパネルを更新する
        UpdateItemCollectionBonusPanel();
    }

    /// <summary>
    /// HP割合ボーナスパネルの更新処理
    /// </summary>
    private void UpdateHpRateBonusPanel()
    {
        string messageText = "HPボーナス";
        string valueText = "なし";

        int bonus = bonusManager.CalculateHpRateBonus();
        if (bonus != 0)
        {
            messageText = string.Format("HP{0}%", player.HpRatePercentage());
            valueText = scorePrefix + bonus.ToString();
        }

        hpRateBonusMessageText.text = messageText;
        hpRateBonusValueText.text = valueText;
    }

    /// <summary>
    /// クリアタイムボーナスパネルの更新処理
    /// </summary>
    private void UpdateClearTimeBonusPanel()
    {
        string messageText = "クリアタイムボーナス";
        string valueText = "なし";

        int bonus = bonusManager.CalculateClearTimeBonus();
        if (bonus != 0)
        {
            int minutes = Mathf.FloorToInt(Mathf.Ceil(player.PlayTimeFromStartToGoal / 60.0f));
            messageText = string.Format("{0}分未満にゴール", minutes);
            valueText = scorePrefix + bonus.ToString();
        }

        clearTimeBonusMessageText.text = messageText;
        clearTimeBonusValueText.text = valueText;
    }

    /// <summary>
    /// アイテム収集ボーナスパネルの更新処理
    /// </summary>
    private void UpdateItemCollectionBonusPanel()
    {
        string messageText = "食べ物収集ボーナス";
        string valueText = "なし";

        int bonus = bonusManager.CalculateItemCollectionBonus();
        if (bonus != 0)
        {
            int requiredItems = bonusManager.RequiredNumberForGettingItemCollectionBonus;
            int bonusCount = player.AcquiredItems / requiredItems * requiredItems;
            messageText = string.Format("食べ物を{0}個以上集めた", bonusCount);
            valueText = scorePrefix + bonus.ToString();
        }

        itemCollectionMessageText.text = messageText;
        itemCollectionValueText.text = valueText;
    }
}