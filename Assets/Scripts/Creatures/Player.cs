using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
    public static readonly int hashFallSpeed = Animator.StringToHash("FallSpeed");
    public static readonly int hashGroundDistance = Animator.StringToHash("GroundDistance");
    //readonly int hashIsCrouch = Animator.StringToHash("IsCrouch");
    //readonly int hashAttack1 = Animator.StringToHash("Attack1");
    //readonly int hashAttack2 = Animator.StringToHash("Attack2");
    //readonly int hashAttack3 = Animator.StringToHash("Attack3");
    public static readonly int hashIsAttacking = Animator.StringToHash("IsAttacking");
    public static readonly int hashIsDead = Animator.StringToHash("IsDead");

    // キャラクターの高さに合わせたオフセット
    [SerializeField]
    float characterHeightOffset = 1.0f;

    // レイヤーマスク
    [SerializeField]
    LayerMask groundMask = 0;

    // 弾のプレファブ
    [SerializeField]
    GameObject bulletPrefab = null;

    // 弾の速度
    [SerializeField]
    float bulletVelocity = 12.0f;

    // 弾を発射するまでにかかる時間
    [SerializeField]
    float waitTimeBeforeFire = 0.35f;

    // ジャンプの最高速度
    [SerializeField]
    float maxJumpSpeed = 7.5f;
    public float MaxJumpSpeed
    {
        get
        {
            return maxJumpSpeed;
        }
        private set
        {
            maxJumpSpeed = value;
        }
    }

    // ジャンプ時の加速のしやすさ
    [SerializeField]
    float jumpSpeedMultiplier = 2000.0f;
    public float JumpSpeedMultiplier
    {
        get
        {
            return jumpSpeedMultiplier;
        }
        private set
        {
            jumpSpeedMultiplier = value;
        }
    }

    // ジャンプ可能な時間の最大値
    [SerializeField]
    float maxJumpTimeCount = 0.25f;
    public float MaxJumpTimeCount
    {
        get
        {
            return maxJumpTimeCount;
        }
        private set
        {
            maxJumpTimeCount = value;
        }
    }

    // ジャンプ時の効果音プレファブ
    [SerializeField]
    GameObject jumpSEPrefab = null;

    // ジャンプ可能な時間の残り時間
    public float CurrentJumpTimeCount { get; private set; } = 0.0f;

    // 空中ジャンプが可能な回数
    [SerializeField]
    int maxAerialJumps = 1;
    public int MaxAerialJumps
    {
        get
        {
            return maxAerialJumps;
        }
        private set
        {
            maxAerialJumps = value;
        }
    }

    // 空中ジャンプの残り回数
    int currentAerialJumps = 0;
    public int CurrentAerialJumps
    {
        get
        {
            return currentAerialJumps;
        }
        private set
        {
            // ゼロ未満にならず、最大値を超えない
            currentAerialJumps = Mathf.Clamp(value, 0, MaxAerialJumps);
        }
    }

    // 接地判定
    [SerializeField]
    IsGroundedChecker isGroundedChecker = null;

    // 弾の発射位置となる座標
    [SerializeField]
    Transform shotPoint = null;

    // 反転しているか
    public bool IsInverted { get; private set; } = false;

    // 接地しているか
    public bool IsGrounded { get; private set; } = false;

    // 攻撃中か
    public bool IsAttacking { get; private set; } = false;

    // ジャンプ中か
    public bool IsJumping { get; private set; } = false;

    // ゴールアイテムを入手したか
    public bool HasGotGoalItem { get; private set; } = false;

    // スタートからゴールまでのプレイ時間
    public float PlayTimeFromStartToGoal { get; private set; } = 0.0f;

    // 獲得したスコア
    public int Score { get; private set; } = 0;

    // 撃破した敵の数
    public int DefeatedEnemys { get; private set; } = 0;

    // 入手したアイテム数
    public int AcquiredItems { get; private set; } = 0;

    // 入手したアイテムの個数がUIに反映されたフラグ
    public bool AcquiredItemsIsReflectedOnUI { get; set; } = false;

    // 現在の階層
    public int CurrentStage { get; set; } = 1;

    // 現在の階層の変化がUIに反映されたフラグ
    public bool CurrentStageIsReferectedOnUI { get; set; } = false;

    // 水平方向の入力
    public float HorizontalAxis { get; private set; } = 0.0f;

    // 垂直方向の入力
    public float VerticalAxis { get; private set; } = 0.0f;

    // ジャンプボタンが押された
    public bool InputGetButtonDownJump { get; private set; } = false;

    // ジャンプボタンが離された
    public bool InputGetButtonUpJump { get; private set; } = false;

    // 攻撃ボタンが押されているか
    public bool InputGetButtonAttack { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();

        // 空中ジャンプ回数初期化
        InitAerialJumps();

        // アイテム初期化
        InitAcquiredItems();
    }

    /// <summary>
    /// 空中ジャンプ回数を初期化する
    /// </summary>
    public void InitAerialJumps()
    {
        CurrentAerialJumps = MaxAerialJumps;
    }

    /// <summary>
    /// 入手したアイテムを初期化する
    /// </summary>
    public void InitAcquiredItems()
    {
        AcquiredItems = 0;
    }

    private void Update()
    {
        // 入力を受け付ける
        GetInputs();

        // 反転状態を更新する
        UpdateInvert();

        // 攻撃状態を更新する
        UpdateAttack();

        // ジャンプ状態を更新する
        UpdateJump();

        // ステータスを更新する
        UpdateState();

        // アニメーションを更新する
        UpdateAnimation();

        // 弾の発射位置を更新する

        // プレイ時間を増やす
        IncreasePlayTime();
    }

    private void FixedUpdate()
    {
        // 反転状態を更新する
        UpdateInvert();

        // 左右移動
        AddforceForMove();

        // ジャンプ
        if (IsJumping)
        {
            AddforceForJump();
        }
    }

    /// <summary>
    /// 左右移動のためのAddForce処理
    /// </summary>
    private void AddforceForMove()
    {
        if (!CanMove())
        {
            InitAxis();
        }

        // 左右移動速度の計算
        Vector2 movement = Vector2.zero;
        movement.x = (MaxMoveSpeed * HorizontalAxis - Rigidbody2D.velocity.x) * MoveSpeedMultiplier * Time.deltaTime;

        // 力を加える
        Rigidbody2D.AddForce(movement);
    }

    /// <summary>
    /// ジャンプのためのAddForce処理
    /// </summary>
    private void AddforceForJump()
    {
        // 操作不可なら中断する
        if (!AcceptInputs())
        {
            return;
        }

        // ジャンプ速度の計算
        Vector2 movement = Vector2.zero;
        movement.y = (MaxJumpSpeed - Rigidbody2D.velocity.y) * JumpSpeedMultiplier * Time.deltaTime;

        // 力を加える
        Rigidbody2D.AddForce(movement);
    }

    /// <summary>
    /// 各種入力を受け取る
    /// </summary>
    private void GetInputs()
    {
        // 操作不可なら水平、垂直方向の入力をリセットして中止する
        if (!AcceptInputs())
        {
            InitAxis();
            return;
        }

        // 水平方向の入力状態
        HorizontalAxis = Input.GetAxis("Horizontal");

        // 垂直方向の入力状態
        VerticalAxis = Input.GetAxis("Vertical");

        // 攻撃ボタンが押されている
        InputGetButtonAttack = Input.GetButton("Fire1");

        // ジャンプボタンが押された
        InputGetButtonDownJump = Input.GetButtonDown("Jump");

        // ジャンプボタンが離された
        InputGetButtonUpJump = Input.GetButtonUp("Jump");
    }

    /// <summary>
    /// 水平、垂直方向の入力をゼロに戻す
    /// </summary>
    private void InitAxis()
    {
        // 水平方向の入力をリセットする
        if (HorizontalAxis != 0.0f)
        {
            HorizontalAxis = 0.0f;
        }

        // 垂直方向の入力をリセットする
        if (VerticalAxis != 0.0f)
        {
            VerticalAxis = 0.0f;
        }
    }

    /// <summary>
    /// 反転状態を更新する
    /// </summary>
    private void UpdateInvert()
    {
        if (HorizontalAxis > 0.0f && IsInverted || HorizontalAxis < 0.0f && !IsInverted)
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
        IsInverted = !IsInverted;

        transform.localScale = scale;
    }

    /// <summary>
    /// 攻撃の更新処理
    /// </summary>
    private void UpdateAttack()
    {
        // 攻撃可能で、なおかつ攻撃ボタンが押されている場合
        if (CanAttack() && InputGetButtonAttack)
        {
            // 攻撃フラグON
            IsAttacking = true;

            // 弾発射コルーチンを実行する
            StartCoroutine(Fire());
        }
    }

    /// <summary>
    /// 弾を発射する
    /// </summary>
    /// <returns></returns>
    private IEnumerator Fire()
    {
        // 発射前の待機時間
        yield return new WaitForSeconds(waitTimeBeforeFire);

        // 発射不可能なら中止する
        if (!CanFire())
        {
            IsAttacking = false;
            yield break;
        }

        // 弾を生成し、Bulletコンポーネントを取得する
        PlayerBullet bullet = Instantiate(bulletPrefab).GetComponent<PlayerBullet>();

        // 弾にプレイヤーコンポーネントを渡す
        bullet.RecievePlayerComponentByPlayer(this);

        // 弾の移動速度を決める
        Vector2 movement = new Vector2(bulletVelocity, 0.0f);
        if (IsInverted)
        {
            movement.x = -movement.x;
        }

        // 弾の移動を開始する
        bullet.Setup(shotPoint.position, movement);

        // 攻撃フラグOFF
        IsAttacking = false;
    }

    /// <summary>
    /// ジャンプ動作関連の更新処理
    /// </summary>
    private void UpdateJump()
    {
        // ジャンプ中の場合
        if (IsJumping)
        {
            // ジャンプ時間カウントを進める
            CurrentJumpTimeCount += Time.deltaTime;

            // ジャンプボタンが離された、またはジャンプ時間カウントが最大値に達した場合、ジャンプフラグをOFFにする
            if (InputGetButtonUpJump || CurrentJumpTimeCount >= MaxJumpTimeCount)
            {
                IsJumping = false;
            }
        }
        // ジャンプボタンが押され、なおかつジャンプが可能な場合
        else if (InputGetButtonDownJump && CanJump())
        {
            // 空中ジャンプを行う場合、その残り回数を1減らす
            if (!IsGrounded)
            {
                CurrentAerialJumps--;
            }

            // ジャンプ時間カウントをリセットする
            CurrentJumpTimeCount = 0.0f;

            // ジャンプフラグON
            IsJumping = true;

            // ジャンプ効果音再生
            if (jumpSEPrefab != null)
            {
                Instantiate(jumpSEPrefab);
            }
        }
    }

    /// <summary>
    /// 状態の更新処理
    /// </summary>
    private void UpdateState()
    {
        // 接地状態を更新する
        IsGrounded = isGroundedChecker.UpdateIsGrounded();

        // 接地しているなら、空中ジャンプ回数を回復する
        if (IsGrounded)
        {
            CurrentAerialJumps = MaxAerialJumps;
        }

        // ダメージを受けている場合
        if (State == CreatureState.Damaged)
        {
            //気絶時間を減らす
            CurrentStunTime -= Time.deltaTime;

            // 気絶時間がゼロになったら、通常状態に戻る
            if (CurrentStunTime == 0.0f)
            {
                State = CreatureState.Normal;
            }
        }
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="offensePower"></param>
    public override void RecieveDamage(int offensePower)
    {
        // 通常状態ではない、または既にゴールしているなら中止する
        if (State != CreatureState.Normal || HasGotGoalItem)
        {
            return;
        }

        // 元の処理を行う
        base.RecieveDamage(offensePower);

        // 死亡した場合
        if (State == CreatureState.Dead)
        {
            // ダメージ効果音を鳴らす
            Instantiate(DamageSEPrefab);
        }
    }

    /// <summary>
    /// 得点を初期化する
    /// </summary>
    public void InitScore()
    {
        Score = 0;
    }

    /// <summary>
    /// 得点を加算する
    /// </summary>
    /// <param name="value"></param>
    public void AddScore(int value)
    {
        Score += value;
    }

    /// <summary>
    /// アイテムを手に入れる
    /// </summary>
    /// <param name="item"></param>
    public void AcquireItem(Item item)
    {
        // アイテム入手数を増やす
        AcquiredItems++;

        // 得点を加算する
        AddScore(item.Score);

        // 入手したアイテムがゴールフラグのものの場合
        if (item.IsGoalFlag)
        {
            // ゴールフラグON
            HasGotGoalItem = true;
        }

        // アイテム入手数変化反映フラグOFF
        AcquiredItemsIsReflectedOnUI = false;
    }

    /// <summary>
    /// アニメーションを更新する
    /// </summary>
    private void UpdateAnimation()
    {
        Animator.SetBool(hashIsDamaged, State == CreatureState.Damaged);
        Animator.SetBool(hashIsDead, State == CreatureState.Dead);
        Animator.SetBool(hashIsAttacking, IsAttacking);

        Animator.SetFloat(hashSpeed, Mathf.Abs(HorizontalAxis));

        var distanceFromGround = Physics2D.Raycast(transform.position, Vector3.down, 1, groundMask);
        Animator.SetFloat(hashGroundDistance, distanceFromGround.distance == 0 ? 99 : distanceFromGround.distance - characterHeightOffset);
        Animator.SetFloat(hashFallSpeed, Rigidbody2D.velocity.y);
    }

    /// <summary>
    /// 別のシーンに存在するPlayerからの必要なメンバー変数を引き継ぐ
    /// </summary>
    public void  TakeOverMenberVariablesFromPlayerInAnotherScene(Player player)
    {
        Score = player.Score;
        HasGotGoalItem = player.HasGotGoalItem;
        State = player.State;
        MaxHp = player.MaxHp;
        CurrentHp = player.CurrentHp;
        AcquiredItems = player.AcquiredItems;
        PlayTimeFromStartToGoal = player.PlayTimeFromStartToGoal;
    }

    /// <summary>
    /// プレイ時間を増やす
    /// </summary>
    private void IncreasePlayTime()
    {
        // クリアしている、または準備状態、死亡状態なら増やさない
        if (HasGotGoalItem || State == CreatureState.Ready || State == CreatureState.Dead)
        {
            return;
        }

        PlayTimeFromStartToGoal += Time.deltaTime;
    }

    /// <summary>
    /// 操作を受け付けるか
    /// </summary>
    /// <returns></returns>
    private bool AcceptInputs()
    {
        // 通常状態で、なおかつゴールしていないなら操作可能
        return State == CreatureState.Normal && !HasGotGoalItem;
    }

    /// <summary>
    /// 移動可能か
    /// </summary>
    /// <returns></returns>
    private bool CanMove()
    {
        // 操作可能で、なおかつ攻撃中でなければ可能
        return AcceptInputs() && !IsAttacking;
    }

    /// <summary>
    /// 攻撃可能か
    /// </summary>
    /// <returns></returns>
    private bool CanAttack()
    {
        // 操作可能で、なおかつ攻撃中でなければ可能
        return AcceptInputs() && !IsAttacking;
    }

    /// <summary>
    /// 攻撃前の待機時間の後、発射可能か
    /// </summary>
    /// <returns></returns>
    private bool CanFire()
    {
        return AcceptInputs();
    }

    /// <summary>
    /// ジャンプ可能か
    /// </summary>
    /// <returns></returns>
    private bool CanJump()
    {
        // 操作可能で、攻撃中でなく、なおかつ地上にいるか空中ジャンプ回数が残っているなら可能
        return AcceptInputs() && !IsAttacking && (IsGrounded || currentAerialJumps > 0);
    }

    /// <summary>
    /// HPの割合を計算する
    /// </summary>
    /// <returns>HPの割合</returns>
    public float HpRate()
    {
        return (float)CurrentHp / MaxHp;
    }

    /// <summary>
    /// HPの割合を百分率で求める
    /// </summary>
    /// <returns>HPの割合(百分率)</returns>
    public int HpRatePercentage()
    {
        return 100 * CurrentHp / MaxHp;
    }
}