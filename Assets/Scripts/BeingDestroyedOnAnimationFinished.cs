using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeingDestroyedOnAnimationFinished : MonoBehaviour
{
    // Animatorコンポーネント
    Animator animator = null;

    private void Awake()
    {
        // Animatorコンポーネントを取得する
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // アニメーションが終了したら、自分自身を破壊する
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Destroy(gameObject);
        }
    }
}