using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameSceneState
{
    FadeIn,
    Ready,
    Play,
    FadeOut
}

public class GameSceneManager : MonoBehaviour
{
    // ゲームシーンの状態
    public GameSceneState state = GameSceneState.FadeIn;

    // フェードイン処理にかかる時間
    [SerializeField]
    float fadeInTime = 0.5f;

    // フェードアウト処理にかかる時間
    [SerializeField]
    float fadeOutTime = 0.5f;

    // シーン内で再生するBGMプレファブ
    [SerializeField]
    GameObject sceneBGMPrefab = null;

    // シーンのやり直しの際の効果音プレファブ
    [SerializeField]
    GameObject ReloadSceneSEPrefab = null;

    // ゲーム開始からBGMを再生するまでの秒数
    [SerializeField]
    float waitForPlayBGM = 1.0f;

    // カウントダウン
    [SerializeField]
    CountDownLogic countDown = null;

    // 同シーン移行までの秒数
    [SerializeField]
    float waitTimeBeforeThisScene = 0.5f;

    // リザルトシーン移行までの秒数
    [SerializeField]
    float waitTimeBeforeResultScene = 1.0f;

    // ゲームオーバーシーン移行までの秒数
    [SerializeField]
    float waitTimeBeforeGameOverScene = 2.0f;

    // Player
    [SerializeField]
    Player player = null;

    // FloorOfDeath
    [SerializeField]
    FloorOfDeath floorOfDeath = null;

    // Fade
    [SerializeField]
    Fade fade = null;

    // 既にシーン移行コルーチンを実行したか
    bool excutedSceneSwitchCoroutine = false;

    // 移行するシーンの名称
    string nextSceneName = SceneName.Result;

    private void Start()
    {
        // フェードイン処理を行う
        fade.FadeIn(fadeInTime);
    }

    private void Update()
    {
        // ゲームシーンの状態に応じた処理を行う
        switch (state)
        {
            case GameSceneState.FadeIn:
                FadeIn();
                break;

            case GameSceneState.Ready:
                Ready();
                break;

            case GameSceneState.Play:
                Play();
                break;

            case GameSceneState.FadeOut:
                FadeOut();
                break;

            default:
                break;
        }
    }

    private void FadeIn()
    {
        // フェード処理が終わった場合
        if (!fade.IsFade())
        {
            // 準備状態に移行する
            state = GameSceneState.Ready;
        }
    }

    private void Ready()
    {
        // カウントダウンが0になった場合
        if (countDown.Seconds == 0)
        {
            // プレイヤーを通常状態にして、行動可能にさせる
            player.SwitchState(CreatureState.Normal);

            // 死の床を通常状態にして、行動可能にさせる
            floorOfDeath.SwitchStateToNormal();

            // BGMを再生する
            StartCoroutine(PlayBGM());

            // プレイ状態に移行する
            state = GameSceneState.Play;
        }
    }

    /// <summary>
    /// BGMを再生する
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayBGM()
    {
        // BGMを生成し、AudioSourceコンポーネントを取得する
        AudioSource bgm = Instantiate(sceneBGMPrefab).GetComponent<AudioSource>();

        // 指定された秒数待つ
        yield return new WaitForSeconds(waitForPlayBGM);

        // BGMを再生する
        bgm.Play();
    }


    private void Play()
    {
        // プレイヤーがゴールした場合
        if (player.HasGotGoalItem)
        {
            // まだコルーチンが始まっていないなら、リザルトシーン移行コルーチンを実行する
            if (!excutedSceneSwitchCoroutine)
            {
                StartCoroutine(SwitchSceneToReslut());
            }
        }
        // プレイヤーが死亡した場合
        else if (player.State == CreatureState.Dead)
        {
            // まだコルーチンが始まっていないなら、ゲームオーバーシーン移行コルーチンを実行する
            if (!excutedSceneSwitchCoroutine)
            {
                StartCoroutine(SwitchSceneToGameOver());
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            // まだコルーチンが始まっていないなら、現在と同じシーンに移行するコルーチンを実行する
            if (!excutedSceneSwitchCoroutine)
            {
                StartCoroutine(SwitchSceneToThis());
            }
        }
    }

    /// <summary>
    /// 同じシーンへ移行することでこのシーンをやり直す
    /// </summary>
    /// <returns></returns>
    private IEnumerator SwitchSceneToThis()
    {
        // シーン切り替えコルーチン実行フラグON
        excutedSceneSwitchCoroutine = true;

        // 効果音を再生する
        Instantiate(ReloadSceneSEPrefab);

        // 次のシーンの名前を同じ名前にする
        nextSceneName = SceneManager.GetActiveScene().name;

        // フェードアウト処理
        fade.FadeOut(fadeOutTime);

        // 指定された秒数待つ
        yield return new WaitForSeconds(waitTimeBeforeThisScene);

        // フェードアウト状態に移行する
        state = GameSceneState.FadeOut;
    }

    /// <summary>
    /// Resultシーンへ移行する
    /// </summary>
    /// <returns></returns>
    private IEnumerator SwitchSceneToReslut()
    {
        // シーン切り替えコルーチン実行フラグON
        excutedSceneSwitchCoroutine = true;

        // 次のシーンの名前をリザルトにする
        nextSceneName = SceneName.Result;

        // シーン遷移後に行うメソッドを追加する
        SceneManager.sceneLoaded += ResultSceneLoaded;

        // 指定された秒数待つ
        yield return new WaitForSeconds(waitTimeBeforeResultScene);

        // フェードアウト処理
        fade.FadeOut(fadeOutTime);

        // フェードアウト状態に移行する
        state = GameSceneState.FadeOut;
    }

    /// <summary>
    /// Reslutシーンへ移行した後に行う処理
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="loadSceneMode"></param>
    private void ResultSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        // シーン内のプレイヤーを検索して取得する
        Player playerInResultScene = GameObject.FindWithTag(TagName.Player).GetComponent<Player>();

        // 取得したプレイヤーに前のシーンのプレイヤーの情報を引き継がせる
        playerInResultScene.TakeOverMenberVariablesFromPlayerInAnotherScene(player);

        // このメソッドを削除する
        SceneManager.sceneLoaded -= ResultSceneLoaded;
    }


    /// <summary>
    /// GameOverシーンへ移行する
    /// </summary>
    /// <returns></returns>
    private IEnumerator SwitchSceneToGameOver()
    {
        // シーン切り替えコルーチン実行フラグON
        excutedSceneSwitchCoroutine = true;

        // 次のシーンの名前をゲームオーバーにする
        nextSceneName = SceneName.GameOver;

        // 指定された秒数待つ
        yield return new WaitForSeconds(waitTimeBeforeGameOverScene);

        // フェードアウト処理
        fade.FadeOut(fadeOutTime);

        // フェードアウト状態に移行する
        state = GameSceneState.FadeOut;
    }

    private void FadeOut()
    {
        // フェードアウト処理が終わった場合
        if (!fade.IsFade())
        {
            //次のシーンに移行する
            SceneManager.LoadScene(nextSceneName);
        }
    }
}