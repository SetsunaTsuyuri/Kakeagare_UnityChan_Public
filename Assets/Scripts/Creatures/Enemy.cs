using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    // 攻撃力
    [SerializeField]
    int offensePower = 1;

    // 撃破時に得られる得点
    [SerializeField]
    int score = 200;
    public int Score
    {
        get
        {
            return score;
        }
        private set
        {
            score = value;
        }
    }

    // 消滅効果音
    [SerializeField]
    GameObject destructionSEPrefab = null;

    // 消滅エフェクト
    [SerializeField]
    GameObject destructionEffectPrefab = null;

    // ドロップアイテムプレファブ
    [SerializeField]
    GameObject dropItemPrefab = null;

    // 障害物判定
    [SerializeField]
    ObstacleChecker obstacleChecker = null;

    // 地形判定
    [SerializeField]
    TerrianChecker terrianChecker = null;

    // 移動方向のX軸
    float moveDirectionX = 1.0f;

    // プレイヤーによって倒されたか
    public bool IsDefeatedByPlayer { get; set; } = false;

    // FloatTextCreatorコンポーネント
    FloatTextCreator floatTextCreator = null;

    protected override void Awake()
    {
        base.Awake();

        // コンポーネント取得
        floatTextCreator = GetComponent<FloatTextCreator>();
    }

    private void Start()
    {
        // 最初から反転しているなら、それに合わせて移動方向を変える
        if (transform.localScale.x < 0.0f)
        {
            moveDirectionX = -moveDirectionX;
        }
    }

    private void Update()
    {
        switch (State)
        {
            case CreatureState.Ready:
                // 何もしない
                break;

            case CreatureState.Normal:
                // 何もしない
                break;

            case CreatureState.Damaged:
                Damaged();
                break;

            case CreatureState.Dead:
                Dead();
                break;
            default:
                break;
        }

        // アニメーターの更新処理
        UpdateAnimation();
    }

    private void Damaged()
    {
        // 気絶時間を減らす
        CurrentStunTime -= Time.deltaTime;

        // 気絶時間がゼロになった場合
        if (CurrentStunTime == 0.0f)
        {
            //通常状態に戻る
            State = CreatureState.Normal;
        }
    }

    private void Dead()
    {
        // 消滅エフェクトを生成する
        InstantiateDestructionEffect();

        // プレイヤーによって倒された場合
        if (IsDefeatedByPlayer)
        {
            // 消滅効果音を再生する
            InstantiateDestructionSE();

            // ドロップオブジェクトを生成する
            InstantiateDropObject();

            // 得点を表示する
            floatTextCreator.CreateFloatText(Score, transform.position);
        }

        // 自分自身を破壊する
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        switch (State)
        {
            case CreatureState.Ready:
                // 何もしない
                break;

            case CreatureState.Normal:
                FixedNormal();
                break;

            case CreatureState.Damaged:
                // 何もしない
                break;

            case CreatureState.Dead:
                // 何もしない
                break;

            default:
                break;
        }
    }

    private void FixedNormal()
    {
        // 移動
        Move();

        // 反転状態の更新
        UpdateInvert();
    }

    /// <summary>
    /// 移動する
    /// </summary>
    private void Move()
    {
        // 移動速度の計算
        Vector2 movement = Vector2.zero;
        movement.x = (MaxMoveSpeed * moveDirectionX - Rigidbody2D.velocity.x) * MoveSpeedMultiplier * Time.deltaTime;

        // 力を加える
        Rigidbody2D.AddForce(movement);
    }

    /// <summary>
    /// 反転状態を更新する
    /// </summary>
    private void UpdateInvert()
    {
        if (obstacleChecker.HitObstacle || terrianChecker.ExitTerrian)
        {
            Invert();
        }
    }

    /// <summary>
    /// 反転する
    /// </summary>
    private void Invert()
    {
        Vector3 scale = transform.localScale;

        scale.x = -scale.x;
        moveDirectionX = -moveDirectionX;

        transform.localScale = scale;
    }


    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="offensePower"></param>
    public override void RecieveDamage(int offensePower)
    {
        base.RecieveDamage(offensePower);

        //ダメージトリガーセット
        Animator.SetTrigger(hashDamage);
    }

    /// <summary>
    /// 消滅効果音を再生する
    /// </summary>
    private void InstantiateDestructionSE()
    {
        // プレファブがなければ中止する
        if (destructionSEPrefab == null)
        {
            return;
        }

        // 消滅効果音を再生する
        Instantiate(destructionSEPrefab);
    }

    /// <summary>
    /// 消滅エフェクトを生成する
    /// </summary>
    private void InstantiateDestructionEffect()
    {
        // プレファブがないなら中止する
        if (destructionEffectPrefab == null)
        {
            return;
        }

        // 消滅エフェクトを生成する
        GameObject instance = Instantiate(destructionEffectPrefab);

        // その位置を自身と同じ位置にする
        instance.transform.position = transform.position;
    }

    /// <summary>
    /// ドロップアイテムを生成する
    /// </summary>
    private void InstantiateDropObject()
    {
        // プレファブがないなら中止する
        if (dropItemPrefab == null)
        {
            return;
        }

        // ドロップオブジェクトを生成する
        GameObject instance = Instantiate(dropItemPrefab);

        // その位置を自身と同じ位置にする
        instance.transform.position = transform.position;
    }

    /// <summary>
    /// アニメーターの更新処理
    /// </summary>
    private void UpdateAnimation()
    {
        Animator.SetBool(hashIsDamaged, State == CreatureState.Damaged);
        Animator.SetFloat(hashSpeed, Mathf.Abs(Rigidbody2D.velocity.x));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // プレイヤーに触れた場合
        if (collision.gameObject.CompareTag(TagName.Player))
        {
            // コンポーネント取得
            Player player = collision.gameObject.GetComponent<Player>();

            // 動きを止める
            player.Stop();

            // ダメージを与える
            player.RecieveDamage(offensePower);

            // 死亡状態になる
            State = CreatureState.Dead;
        }
    }
}