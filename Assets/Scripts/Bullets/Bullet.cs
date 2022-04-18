using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    // 攻撃力
    [SerializeField]
    protected int offsensePower = 1;

    // 発射されてから消えるまでの時間
    [SerializeField]
    float timeToDisappear = 2.0f;

    // 効果音プレファブ
    [SerializeField]
    GameObject shotSEPrefab = null;

    // 経過時間
    float elapsedTime = 0.0f;

    // 弾の移動速度と方向
    Vector2 movement = Vector2.zero;

    // SpriteRendererコンンポーネント
    SpriteRenderer spriteRenderer = null;

    // RigidBody2Dコンポーネント
    Rigidbody2D rigid2D = null;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 効果音を再生する
        if (shotSEPrefab != null)
        {
            Instantiate(shotSEPrefab);
        }
    }

    private void Update()
    {
        // 経過時間を増やす
        elapsedTime += Time.deltaTime;

        // 規定時間に達したら自分自身を破壊する
        if (elapsedTime >= timeToDisappear)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        // 移動処理
        Vector2 positon = transform.position;
        positon += movement * Time.deltaTime;
        rigid2D.MovePosition(positon);
    }

    /// <summary>
    /// 弾の設定
    /// </summary>
    /// <param name="position"></param>
    /// <param name="moveDirection"></param>
    public void Setup(Vector3 position, Vector2 movement)
    {
        transform.position = position;
        this.movement = movement;

        if (movement.x < 0.0f)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    protected abstract void OnTriggerEnter2D(Collider2D collision);
}