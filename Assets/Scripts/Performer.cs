using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PerformerType
{
    Normal,
    Flip
}

public class Performer : MonoBehaviour
{
    [SerializeField]
    PerformerType type = PerformerType.Normal;

    // ポジティブ
    public static readonly int hashPositive = Animator.StringToHash("Positive");

    // 指さし
    public static readonly int hashPointAt = Animator.StringToHash("PointAt");

    // 走る
    public static readonly int hashRun = Animator.StringToHash("Run");

    // ジャンプ
    public static readonly int hashJump = Animator.StringToHash("Jump");

    // 撃つ
    public static readonly int hashIsShooting = Animator.StringToHash("IsShooting");

    // スプライトを反転させるタイミング ゼロの場合は反転しない
    [SerializeField]
    float flipTime = 0.0f;

    // 経過時間
    float elapsedTime = 0.0f;

    // Animatorコンポーネント
    public Animator Animator { get; private set; }

    // SpriteRendererコンポーネント
    SpriteRenderer spriteRenderer = null;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        switch (type)
        {
            case PerformerType.Normal:
                Normal();
                break;

            case PerformerType.Flip:
                Flip();
                break;

            default:
                break;
        }
    }

    private void Normal()
    {
        // 何もしない
    }

    private void Flip()
    {
        // 経過時間を増やす
        elapsedTime += Time.deltaTime;

        // 反転するタイミングに達した場合
        if (elapsedTime >= flipTime)
        {
            // スプライトを反転する
            spriteRenderer.flipX = !spriteRenderer.flipX;

            // 経過時間をゼロにする
            elapsedTime = 0.0f;
        }
    }
}