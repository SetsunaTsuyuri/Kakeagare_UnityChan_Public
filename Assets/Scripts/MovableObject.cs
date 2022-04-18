using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovableObject : MonoBehaviour
{
    // 移動速度
    [SerializeField]
    float moveSpeed = 0.0f;
    float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            moveSpeed = value;
        }
    }

    // 移動先の座標リスト
    [SerializeField]
    List<Vector2> movePositionList = new List<Vector2>();
    List<Vector2> MovePositionList
    {
        get
        {
            return movePositionList;
        }
        set
        {
            movePositionList = value;
        }
    }

    // 動作を繰り返すかどうか
    [SerializeField]
    bool isLoop = false;
    bool IsLoop
    {
        get
        {
            return isLoop;
        }
        set
        {
            isLoop = value;
        }
    }

    // 回転速度
    [SerializeField]
    float rotationalVelocity = 0.0f;
    float RotationalVelocity
    {
        get
        {
            return rotationalVelocity;
        }
        set
        {
            rotationalVelocity = value;
        }
    }

    // 移動先に達した後の待機時間
    [SerializeField]
    float waitTimeAfterArrivalNextPosition = 0.0f;

    // 消滅エフェクトのプレファブ
    [SerializeField]
    GameObject destructionEffectPrefab = null;

    // 待機時間
    float waitTime = 0.0f;

    // 動作が許されているか
    public bool IsAllowedToMove { get; set; } = false;

    // 移動するか
    bool IsMovable { get; set; } = false;

    // 回転するか
    bool IsRotatable { get; set; } = false;

    // 移動先のインデックス
    int PositionListIndex { get; set; } = 0;

    // 初期位置
    Vector2 InitialPosition { get; set; } = Vector2.zero;

    // 移動先の座標
    Vector2 NextPosition { get; set; } = Vector2.zero;

    // Rigidbody2Dコンポーネント
    Rigidbody2D Rigidbody2D { get; set; } = null;

    private void Start()
    {
        // Rigidbody2Dコンポーネントを取得する
        Rigidbody2D = GetComponent<Rigidbody2D>();

        // 移動先の座標が存在する場合
        if (MovePositionList.Count > 0)
        {
            // 動作が許可されている場合、動けるようになる
            IsMovable = true;

            // ループする場合、最後に初期位置へ戻るよう、移動先のリストの最後尾に(0, 0)の座標を追加する
            if (IsLoop)
            {
                MovePositionList.Add(Vector2.zero);
            }

            // 初期位置を取得する
            InitialPosition = transform.position;

            // リスト0番目の座標を最初の移動先とする
            NextPosition = InitialPosition + MovePositionList[0];
        }

        // 回転速度が0でない場合、動作が許可されていれば回転できるようになる
        if (RotationalVelocity != 0.0f)
        {
            IsRotatable = true;
        }
    }

    private void Update()
    {
        // 待機時間中の場合
        if (waitTime > 0.0f)
        {
            // 時間を減らす
            waitTime -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        // 動作が許可されていないなら中止する
        if (!IsAllowedToMove)
        {
            return;
        }

        // 待機時間中なら中止する
        if (waitTime > 0.0f)
        {
            return;
        }

        // 移動できるなら、移動する
        if (IsMovable)
        {
            Move();
        }

        // 回転できるなら、回転する
        if (IsRotatable)
        {
            Rotate();
        }
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void Move()
    {
        // 移動先の座標の近くにいないなら、そこへ近づく
        if (Vector2.Distance(transform.position, NextPosition) > 0.1f)
        {
            Rigidbody2D.MovePosition(Vector2.MoveTowards(transform.position, NextPosition, MoveSpeed * Time.deltaTime));
        }
        else
        {
            // 移動先の座標を更新する
            UpdateNextPosition();

            // 待機時間セット
            waitTime = waitTimeAfterArrivalNextPosition;
        }
    }

    /// <summary>
    /// 移動先の座標を更新する
    /// </summary>
    private void UpdateNextPosition()
    {
        // 座標リストの番地を1つ進める
        PositionListIndex++;

        // 座標リスト最後尾を超えた場合
        if (PositionListIndex > MovePositionList.Count - 1)
        {
            // 0番目に戻る
            PositionListIndex = 0;

            // ループしないなら、今後動かないようにする
            if (!IsLoop)
            {
                IsMovable = false;
            }
        }

        // 移動先の座標を決める
        NextPosition = InitialPosition + MovePositionList[PositionListIndex];
    }

    /// <summary>
    /// 回転処理
    /// </summary>
    private void Rotate()
    {
        // 角度の計算
        float angle = Mathf.Repeat(Rigidbody2D.rotation + RotationalVelocity * Time.deltaTime, 360.0f);

        // 回転する
        Rigidbody2D.MoveRotation(angle);
    }

    /// <summary>
    /// 消滅エフェクトを生成する
    /// </summary>
    protected void InstantiateDestructionEffect()
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

}