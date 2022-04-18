using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Cherry,
    Bananas,
    Apple
}

public class Item : MovableObject
{
    // 得点
    [SerializeField]
    int score = 0;
    public int Score
    {
        get
        {
            return score;
        }
    }

    // ゴールアイテムか？
    [SerializeField]
    bool isGoalFlag = false;
    public bool IsGoalFlag
    {
        get
        {
            return isGoalFlag;
        }
    }

    // FloatTextCreatorコンポーネント
    FloatTextCreator floatTextCreator;

    private void Awake()
    {
        // コンポーネント取得
        floatTextCreator = GetComponent<FloatTextCreator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤー以外は無視する
        if (!collision.CompareTag(TagName.Player))
        {
            return;
        }

        // Playerコンポーネントを取得する
        Player player = collision.GetComponent<Player>();

        // それにアイテム獲得処理を行わせる
        player.AcquireItem(this);

        // 消滅エフェクトを生成する
        InstantiateDestructionEffect();

        // 得点エフェクトを生成する
        floatTextCreator.CreateFloatText(score, transform.position);

        // 自分自身を破壊する
        Destroy(gameObject);
    }
}