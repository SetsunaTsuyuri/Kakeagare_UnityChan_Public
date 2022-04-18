using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    // 効果音プレファブ
    [SerializeField]
    GameObject keyPressedSEPrefab = null;

    // Animatorコンポーネント
    Animator animator;

    // 「スペースキーを押したとき処理」が完了したか
    bool finishedProcessingOnSpaceKeyPressed = false;

    readonly int hashPressed = Animator.StringToHash("KeyPressed");

    private void Awake()
    {
        // コンポーネント取得
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // スペースキーが押された場合
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpaceKeyPressed();
        }
    }

    /// <summary>
    /// スペースキーが押されたときの処理
    /// </summary>
    private void OnSpaceKeyPressed()
    {
        // 既に処理がなされているなら中止する
        if (finishedProcessingOnSpaceKeyPressed)
        {
            return;
        }

        // キーが押されたときのアニメーションを実行する
        animator.SetTrigger(hashPressed);

        // 効果音を再生する
        if (keyPressedSEPrefab != null)
        {
            Instantiate(keyPressedSEPrefab);
        }

        // フラグON
        finishedProcessingOnSpaceKeyPressed = true;
    }
}