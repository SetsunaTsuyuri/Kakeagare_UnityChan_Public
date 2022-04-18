using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneManager : MonoBehaviour
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

    // ゲームオーバーの効果音プレファブ
    [SerializeField]
    GameObject gameOverSEPrefab = null;

    // メッセージオブジェクト
    [SerializeField]
    GameObject messageObject = null;

    // Fadeコンポーネント
    [SerializeField]
    Fade fade = null;

    // CountDownLogicコンポーネント
    [SerializeField]
    CountDownLogic countDown = null;

    // シーン切り替えコルーチンが既に実行されたか
    bool excutedSceneSwitchCoroutine = false;

    private IEnumerator Start()
    {
        // フェードイン処理を行う
        fade.FadeIn(fadeInTime);

        // フェードインが終わるまで待つ
        yield return new WaitForSeconds(fadeInTime);

        // 効果音を再生する
        Instantiate(gameOverSEPrefab);

        // メッセージをアクティブにする
        messageObject.SetActive(true);
    }

    private void Update()
    {
        // メッセージがアクティブで、なおかつスペースキーが押された場合
        if (messageObject.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            // コルーチンが実行されていないなら、「スペースキーが押されたときの処理」コルーチンを実行する
            if (!excutedSceneSwitchCoroutine)
            {
                StartCoroutine(OnKeyDownSpace());
            }
        }
        // カウントダウンがゼロになった場合
        else if (countDown.Seconds == 0.0f)
        {
            // コルーチンが実行されていないなら、「カウントダウンがゼロになったときの処理」コルーチンを実行する
            if (!excutedSceneSwitchCoroutine)
            {
                StartCoroutine(OnCountDownSecondsIsZero());
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
        excutedSceneSwitchCoroutine = true;

        // カウントダウン停止
        countDown.Stop();

        // 指定された秒数待つ
        yield return new WaitForSeconds(waitTimeAfterSpaceKeyPressed);

        // フェードアウト処理を行う
        fade.FadeOut(fadeOutTime);

        // フェードアウトが終わるまで待つ
        yield return new WaitForSeconds(fadeOutTime);

        // Gameシーンへ移行する
        SceneManager.LoadScene(SceneName.Game);
    }

    /// <summary>
    /// カウントダウンがゼロになったときの処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnCountDownSecondsIsZero()
    {
        // フラグON
        excutedSceneSwitchCoroutine = true;

        // フェードアウト処理を行う
        fade.FadeOut(fadeOutTime);

        // フェードアウトが終わるまで待つ
        yield return new WaitForSeconds(fadeOutTime);

        // Titleシーンへ移行する
        SceneManager.LoadScene(SceneName.Title);

    }
}
