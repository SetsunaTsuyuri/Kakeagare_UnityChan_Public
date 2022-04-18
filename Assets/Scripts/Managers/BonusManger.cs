using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusManger : MonoBehaviour
{
    // HP割合ボーナスの最大値
    [SerializeField]
    float maxValueOfHpRateBonus = 5000;

    // 最大クリアタイムボーナスの最大値
    [SerializeField]
    int maxValueOfClearTimeBonus = 6000;

    // 1分経過する毎にクリアボーナスから減点される値
    [SerializeField]
    int deducationEveryMinute = 1000;

    // アイテム収集ボーナス
    [SerializeField]
    int itemCollectionBonus = 1000;

    // アイテム収集ボーナスを1回得るために必要なアイテム個数
    [SerializeField]
    int requiredNumberForGettingItemCollectionBonus = 10;
    public int RequiredNumberForGettingItemCollectionBonus
    {
        get
        {
            return requiredNumberForGettingItemCollectionBonus;
        }
    }

    // シーンに入った直後の待機時間
    [SerializeField]
    float waitTimeForSceneLoaded = 0.5f;

    // 各ボーナスパネルのAnimatorが有効になってからの待機時間
    [SerializeField]
    float waitTimeAfterAppearingBonusPanel = 0.2f;

    // スコア加算の待機時間
    [SerializeField]
    float waitTimeAfterAddingScore = 0.4f;

    // ボーナス取得効果音プレファブ
    [SerializeField]
    GameObject gettingBonusSEPrefab = null;

    // Playerコンポーネント
    [SerializeField]
    Player player = null;

    // Performerコンポーネント
    [SerializeField]
    Performer performer = null;

    // Bonusコンポーネント
    [SerializeField]
    Bonus bonus = null;

    // FloatTextCreatorコンポーネント
    FloatTextCreator floatTextCreator;

    private void Awake()
    {
        // コンポーネント取得
        floatTextCreator = GetComponent<FloatTextCreator>();
    }

    /// <summary>
    /// ボーナス処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator DoBonusProcessing()
    {
        // 指定された秒数待つ
        yield return new WaitForSeconds(waitTimeForSceneLoaded);

        // 各種ボーナスを計算する
        for (int i = 0; i < 3; i++)
        {
            // ボーナス得点
            int bonusScore = 0;

            switch (i)
            {
                case 0:
                    // HP割合ボーナス
                    bonusScore = CalculateHpRateBonus();

                    // Animator有効化
                    if (bonusScore > 0)
                    {
                        bonus.HpRateBonusPanelAnimator.enabled = true;

                    }

                    break;

                case 1:
                    // クリアタイムボーナス
                    bonusScore = CalculateClearTimeBonus();

                    // Animator有効化
                    if (bonusScore > 0)
                    {
                        bonus.ClearTimeBonusPanelAnimator.enabled = true;
                    }

                    break;

                case 2:
                    // アイテム収集ボーナス
                    bonusScore = CalculateItemCollectionBonus();

                    // Animator有効化
                    if (bonusScore > 0)
                    {
                        bonus.ItemCollectionBonusPanelAnimator.enabled = true;

                    }

                    break;

                default:
                    break;
            }

            // 指定された秒数待つ
            yield return new WaitForSeconds(waitTimeAfterAppearingBonusPanel);

            // 得点を加算する
            player.AddScore(bonusScore);

            // ボーナスが得られる場合
            if (bonusScore > 0)
            {
                // 効果音再生
                Instantiate(gettingBonusSEPrefab);

                // ボーナス得点を表示する
                floatTextCreator.CreateFloatText(bonusScore, performer.transform.position);

                // 指定された秒数待つ
                yield return new WaitForSeconds(waitTimeAfterAddingScore);
            }
        }
    }

    /// <summary>
    /// HP割合ボーナスの計算処理
    /// </summary>
    /// <returns></returns>
    public int CalculateHpRateBonus()
    {
        int result = 0;

        float playerHpRate = player.HpRate();
        result += Mathf.FloorToInt(maxValueOfHpRateBonus * playerHpRate);

        return result;
    }

    /// <summary>
    /// クリアタイムボーナスの計算処理
    /// </summary>
    /// <returns></returns>
    public int CalculateClearTimeBonus()
    {
        int result = maxValueOfClearTimeBonus;

        result -= deducationEveryMinute * Mathf.FloorToInt(player.PlayTimeFromStartToGoal / 60.0f);

        if (result < 0)
        {
            result = 0;
        }

        return result;
    }

    /// <summary>
    /// アイテム収集ボーナスの計算処理
    /// </summary>
    /// <returns></returns>
    public int CalculateItemCollectionBonus()
    {
        int result = 0;

        int items = player.AcquiredItems;
        while (items >= requiredNumberForGettingItemCollectionBonus)
        {
            result += itemCollectionBonus;
            items -= requiredNumberForGettingItemCollectionBonus;
        }

        return result;
    }
}