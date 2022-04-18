using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageLine : MonoBehaviour
{
    // 動くものの配列
    public MovableObject[] MovableObjects { get; private set; } = null;

    // 敵の配列
    public Enemy[] Enemies { get; private set; } = null;

    // プレイヤーに触れられたか
    public bool IsTouchedByPlayer { get; private set; } = false;

    private void Start()
    {
        // ステージ内の動くものを全て取得する
        MovableObjects = transform.parent.GetComponentsInChildren<MovableObject>();

        // ステージ内の敵を全て取得する
        Enemies = transform.parent.GetComponentsInChildren<Enemy>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // 既にプレイヤーによって触れられているか、プレイヤー以外に触れられたなら反応しない
        if (IsTouchedByPlayer || !collision.CompareTag(TagName.Player))
        {
            return;
        }

        // プレイヤー接触フラグON
        IsTouchedByPlayer = true;
    }
}
