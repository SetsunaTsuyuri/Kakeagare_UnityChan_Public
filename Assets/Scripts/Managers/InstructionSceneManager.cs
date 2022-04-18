using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionSceneManager : MonoBehaviour
{
    // フェードインにかかる時間
    [SerializeField]
    float fadeInTime = 0.5f;

    // フェードアウトにかかる時間
    [SerializeField]
    float fadeOutTime = 0.5f;

    // スペースキーが押された後の待機時間
    [SerializeField]
    float waitTimeAfterSpaceKeyPressed = 0.5f;

    // 走るPerformer
    [SerializeField]
    Performer runningPerformer = null;

    // 撃つPerformer
    [SerializeField]
    Performer ShootingPerformer = null;

    // ジャンプするPerformer
    [SerializeField]
    Performer jumpingPerformer = null;

    // Fadeコンポーネント
    [SerializeField]
    Fade fade = null;

    // 「 スペースキーが押されたときの処理」コルーチンが既に実行されたか
    bool excutedOnKeyDownSpaceCoroutine = false;

    private IEnumerator Start()
    {
        // パフォーマーに走らせる
        runningPerformer.Animator.SetTrigger(Performer.hashRun);

        // パフォーマーに撃たせる
        ShootingPerformer.Animator.SetBool(Performer.hashIsShooting, true);

        // パフォーマーにジャンプさせる
        jumpingPerformer.Animator.SetTrigger(Performer.hashJump);

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

        // 指定された秒数待つ
        yield return new WaitForSeconds(waitTimeAfterSpaceKeyPressed);

        // フェードアウト処理を行う
        fade.FadeOut(fadeOutTime);

        // フェードアウトが終わるまで待つ
        yield return new WaitForSeconds(fadeOutTime);

        // Gameシーンへ移行する
        SceneManager.LoadScene(SceneName.Game);
    }
}