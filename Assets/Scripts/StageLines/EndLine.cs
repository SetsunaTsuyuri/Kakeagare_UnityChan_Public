using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLine : StageLine
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // 既にプレイヤーによって触れられているか、プレイヤー以外に触れられたなら反応しない
        if (IsTouchedByPlayer || !collision.CompareTag(TagName.Player))
        {
            return;
        }

        base.OnTriggerEnter2D(collision);

        // ステージギミック停止
        StopStageGimmicks();

        // 敵停止
        SwitchAllEnemysStatesToReady();
    }

    /// <summary>
    /// ステージ内で動くものの動作の許可を取り消す
    /// </summary>
    private void StopStageGimmicks()
    {
        if (MovableObjects == null)
        {
            return;
        }

        foreach (var val in MovableObjects)
        {
            val.IsAllowedToMove = false;
        }
    }

    /// <summary>
    /// ステージ内の敵を準備状態にする
    /// </summary>
    private void SwitchAllEnemysStatesToReady()
    {
        if (Enemies == null)
        {
            return;
        }

        foreach (var val in Enemies)
        {
            val.SwitchState(CreatureState.Ready);
        }
    }
}