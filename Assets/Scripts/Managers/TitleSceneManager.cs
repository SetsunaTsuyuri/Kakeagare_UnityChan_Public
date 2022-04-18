using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    // フェードインにかかる時間
    [SerializeField]
    float fadeInTime = 0.5f;

    // フェードアウトにかかる時間
    [SerializeField]
    float fadeOutTime = 0.5f;

    // スペースキーが押されてからの待機時間
    [SerializeField]
    float waitTimeAfterSpaceKeyPressed = 1.0f;

    // Performerコンポーネント
    [SerializeField]
    Performer performer = null;

    // Fadeコンポーネント
    [SerializeField]
    Fade fade = null;

    // 「 スペースキーが押されたときの処理」コルーチンが既に実行されたか
    bool excutedOnKeyDownSpaceCoroutine = false;

    private IEnumerator Start()
    {
        // フェードイン処理を行う
        fade.FadeIn(fadeInTime);

        // フェードインが終わるまで待つ
        yield return new WaitForSeconds(fadeInTime);
    }

    private void Update()
    {
        // スペースキーが押された場合
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 「スペースキーが押されたときの処理」がまだ実行されていない場合
            if (!excutedOnKeyDownSpaceCoroutine)
            {
                // コルーチンを実行する
                StartCoroutine(OnKeyDownSpace());
            }
        }

    }

    /// <summary>
    /// スペースキーが押されたときの処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnKeyDownSpace()
    {
        // フラグON
        excutedOnKeyDownSpaceCoroutine = true;

        // 指さしポーズを取らせる
        performer.Animator.SetTrigger(Performer.hashPointAt);

        // 指定された秒数待つ
        yield return new WaitForSeconds(waitTimeAfterSpaceKeyPressed);

        // フェードアウト処理を行う
        fade.FadeOut(fadeOutTime);

        // フェードアウトが終わるまで待つ
        yield return new WaitForSeconds(fadeOutTime);

        // Instructionシーンへ移行する
        SceneManager.LoadScene(SceneName.Instruction);
    }
}