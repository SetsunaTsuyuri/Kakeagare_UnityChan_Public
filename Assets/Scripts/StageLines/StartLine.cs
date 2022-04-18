using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLine : StageLine
{
    // ステージ背景の色
    [SerializeField]
    Color backgroundColor = Color.white;
    public Color BackgroundColor
    {
        get
        {
            return backgroundColor;
        }
    }

    // 背景の色の変更にかかる時間
    [SerializeField]
    float colorChangeDuration = 1.0f;
    public float ColorChangeDuration
    {
        get
        {
            return colorChangeDuration;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // 既にプレイヤーによって触れられているか、プレイヤー以外に触れられたなら反応しない
        if (IsTouchedByPlayer || !collision.CompareTag(TagName.Player))
        {
            return;
        }

        base.OnTriggerEnter2D(collision);

        // コンポーネント取得
        Player player = collision.GetComponent<Player>();

        // プレイヤーの今いる階層を1増やす
        player.CurrentStage++;

        // UI反映フラグOFF
        player.CurrentStageIsReferectedOnUI = false;

        // ステージギミック始動
        StartStageGimmicks();

        // 敵始動
        SwitchAllEnemysStatesToNormal();
    }

    /// <summary>
    /// ステージ内で動くものに動作の許可を与える
    /// </summary>
    private void StartStageGimmicks()
    {
        if (MovableObjects == null)
        {
            return;
        }

        foreach (var val in MovableObjects)
        {
            val.IsAllowedToMove = true;
        }
    }

    /// <summary>
    /// ステージ内の敵を通常状態にする
    /// </summary>
    private void SwitchAllEnemysStatesToNormal()
    {
        if (Enemies == null)
        {
            return;
        }

        foreach (var val in Enemies)
        {
            val.SwitchState(CreatureState.Normal);
        }
    }
}