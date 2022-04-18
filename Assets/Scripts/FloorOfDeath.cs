using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FloorOfDeathState
{
    Ready,
    Normal
}

public class FloorOfDeath : MonoBehaviour
{
    // 状態
    public FloorOfDeathState State { get; private set; } = FloorOfDeathState.Ready;

    // 移動速度
    [SerializeField]
    float moveSpeed = 1.0f;
    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        private set
        {
            moveSpeed = value;
        }
    }

    // 上昇を開始するまでの時間
    [SerializeField]
    float moveStartTimeCount = 3.0f;
    public float MoveStartTimeCount
    {
        get
        {
            return moveStartTimeCount;
        }
        private set
        {
            if (value < 0.0f)
            {
                value = 0.0f;
            }
            moveStartTimeCount = value;
        }
    }

    // 攻撃力
    [SerializeField]
    int offensePower = 100;
    public int OffensePower
    {
        get
        {
            return offensePower;
        }
        private set
        {
            offensePower = value;
        }
    }

    // RigidBody2Dコンポーネント
    public Rigidbody2D Rigid2D { get; private set; } = null;

    private void Awake()
    {
        Rigid2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CountDownForMoveStart();
    }

    private void FixedUpdate()
    {
        Climbing();
    }

    /// <summary>
    /// 上昇する
    /// </summary>
    private void Climbing()
    {
        // 準備状態、または移動不可なら中止する
        if (State == FloorOfDeathState.Ready || !CanMove())
        {
            return;
        }

        // 上昇移動
        Vector2 positon = transform.position;
        positon.y += MoveSpeed * Time.deltaTime;
        Rigid2D.MovePosition(positon);
    }

    /// <summary>
    /// 移動開始カウントダウンを進める
    /// </summary>
    private void CountDownForMoveStart()
    {
        // 準備状態、または移動可能なら中止する
        if (State == FloorOfDeathState.Ready || CanMove())
        {
            return;
        }

        // 移動開始カウントを経過時間分減らす
        MoveStartTimeCount -= Time.deltaTime;
    }

    /// <summary>
    /// 移動可能か
    /// </summary>
    /// <returns></returns>
    private bool CanMove()
    {
        return MoveStartTimeCount == 0.0f;
    }

    /// <summary>
    /// 通常状態になる
    /// </summary>
    public void SwitchStateToNormal()
    {
        State = FloorOfDeathState.Normal;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 敵またはプレイヤーに触れた場合
        if (collision.gameObject.CompareTag(TagName.Enemy) || collision.gameObject.CompareTag(TagName.Player))
        {
            // コンポーネント取得
            Creature creature = collision.gameObject.GetComponent<Creature>();

            // 動きを止める
            creature.Stop();

            // ダメージを与える
            creature.RecieveDamage(OffensePower);
        }
    }
}