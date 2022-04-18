using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CreatureState
{
    Ready,
    Normal,
    Damaged,
    Dead
}

public abstract class Creature : MonoBehaviour
{
    // 状態
    public CreatureState State { get; protected set; } = CreatureState.Ready;

    public static readonly int hashDamage = Animator.StringToHash("Damage");
    public static readonly int hashIsDamaged = Animator.StringToHash("IsDamaged");
    public static readonly int hashSpeed = Animator.StringToHash("Speed");

    // 最大HP
    [SerializeField]
    int maxHp = 5;
    public int MaxHp
    {
        get
        {
            return maxHp;
        }
        protected set
        {
            // ゼロ未満にならない
            if (value < 0)
            {
                value = 0;
            }
            maxHp = value;
        }
    }

    // 現在HP
    int currentHp = 0;
    public int CurrentHp
    {
        get
        {
            return currentHp;
        }
        protected set
        {
            // ゼロ未満にならず、最大HPを超えない
            currentHp = Mathf.Clamp(value, 0, MaxHp);
        }
    }

    // 最大移動速度
    [SerializeField]
    float maxMoveSpeed = 7.5f;
    public float MaxMoveSpeed
    {
        get
        {
            return maxMoveSpeed;
        }
        protected set
        {
            maxMoveSpeed = value;
        }
    }

    // 移動時の加速のしやすさ
    [SerializeField]
    float moveSpeedMultiplier = 2000.0f;
    public float MoveSpeedMultiplier
    {
        get
        {
            return moveSpeedMultiplier;
        }
        protected set
        {
            moveSpeedMultiplier = value;
        }
    }

    // ダメージを受けた際の硬直時間の最大値
    [SerializeField]
    float maxStunTime = 0.5f;
    public float MaxStunTime
    {
        get
        {
            return maxStunTime;
        }
        protected set
        {
            // ゼロ未満にならない
            if (value < 0.0f)
            {
                value = 0.0f;
            }
            maxStunTime = value;
        }
    }

    // ダメージを受けた際の硬直時間の現在値
    float currentStunTime = 0.0f;
    public float CurrentStunTime
    {
        get
        {
            return currentStunTime;
        }
        protected set
        {
            // ゼロ未満にならず、最大値を超えない
            currentStunTime = Mathf.Clamp(value, 0.0f, MaxStunTime);
        }
    }


    // 被ダメージのSEプレハブ
    [SerializeField]
    GameObject damageSEPrefab = null;
    public GameObject DamageSEPrefab
    {
        get
        {
            return damageSEPrefab;
        }
        protected set
        {
            damageSEPrefab = value;
        }
    }

    // Animatorコンポーネント
    public Animator Animator { get; private set; } = null;

    // SpriteRendererコンポーネント
    public SpriteRenderer SpriteRenderer { get; private set; } = null;

    // RigidBody2Dコンポーネント
    public Rigidbody2D Rigidbody2D { get; private set; } = null;

    protected virtual void Awake()
    {
        // ステータス初期化
        InitStatus();

        // コンポーネント取得
        Animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// HPを初期化する
    /// </summary>
    public void InitStatus()
    {
        CurrentHp = MaxHp;
    }

    /// <summary>
    /// 動きを止める
    /// </summary>
    public void Stop()
    {
        Rigidbody2D.velocity = Vector2.zero;
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="offensePower"></param>
    public virtual void RecieveDamage(int offensePower)
    {
        // HPを減らす
        CurrentHp -= offensePower;

        // HPがゼロになった場合
        if (CurrentHp == 0)
        {
            // 死亡状態になる
            State = CreatureState.Dead;
        }
        else
        {
            // 被ダメージ効果音を鳴らす
            if (DamageSEPrefab != null)
            {
                Instantiate(DamageSEPrefab);
            }

            // 気絶時間を最大にする
            CurrentStunTime = MaxStunTime;

            // 被ダメージ状態になる
            State = CreatureState.Damaged;
        }
    }

    /// <summary>
    /// 押される
    /// </summary>
    public void BePushed(Vector2 knockBackPower)
    {
        Rigidbody2D.AddForce(knockBackPower, ForceMode2D.Impulse);
    }

    /// <summary>
    /// 状態を変更する
    /// </summary>
    public void SwitchState(CreatureState creatureState)
    {
        State = creatureState;
    }
}