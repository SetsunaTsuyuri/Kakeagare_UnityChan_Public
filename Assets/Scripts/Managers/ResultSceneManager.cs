using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultSceneManager : MonoBehaviour
{
    //フェードインにかかる時間
    [SerializeField]
    float fadeInTime = 0.5f;

    // フェードアウトにかかる時間
    [SerializeField]
    float fadeOutTime = 0.5f;

    // ドラムロールSEプレファブ
    [SerializeField]
    GameObject drumRollPrefab = null;

    // ドラムロール終了SEプレファブ
    [SerializeField]
    GameObject drumRollFinishedPrefab = null;

    // ドラムーロール終了後の待機時間
    [SerializeField]
    float waitTimeAfterDrumRollFinished = 1.0f;

    // 歓声SEプレファブ
    [SerializeField]
    GameObject CheerSEPrefab = null;

    // 歓声SE後の待機時間
    [SerializeField]
    float waitTimeAfterCheerSE = 0.5f;

    // ハイスコアパネルのTextコンポーネント
    [SerializeField]
    Text highScoreText = null;

    // ハイスコア更新時のテキストの色
    [SerializeField]
    Color UpdateHighScoreColor = Color.white;

    // Performerコンポーネント
    [SerializeField]
    Performer performer = null;

    // BonusMangerコンポーネント
    [SerializeField]
    BonusManger bonusManger = null;

    // Playerコンポーネント
    [SerializeField]
    Player player = null;

    // メッセージオブジェクト
    [SerializeField]
    GameObject messageObject = null;

    // シーン移行までの待機時間
    [SerializeField]
    float waitTimeBeforeSwitchScene = 1.0f;

    // Fadeコンポーネント
    [SerializeField]
    Fade fade = null;

    // 再生された効果音
    GameObject instantiatedSEObject = null;

    // 既にシーン切り替えコルーチンを実行したか
    bool excutedSceneSwitchCoroutine = false;

    private IEnumerator Start()
    {
        // フェードイン処理
        fade.FadeIn(fadeInTime);

        // フェードインが終わるまで待つ
        yield return new WaitForSeconds(fadeInTime);

        // ドラムロールSEを再生する
        instantiatedSEObject = Instantiate(drumRollPrefab);

        // ボーナスマネージャーにボーナス処理を行わせる
        yield return bonusManger.DoBonusProcessing();

        // 演出処理を行う
        yield return PerformeOnBonusProcessingFinished();
    }

    private void Update()
    {
        // メッセージがアクティブになっており、なおかつスペースキーが押された場合
        if (messageObject.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            // シーン切り替えコルーチンが未実行なら、それを実行する
            if (!excutedSceneSwitchCoroutine)
            {
                StartCoroutine(OnKeyDownSpace());
            }
        }
    }

    /// <summary>
    /// ボーナス処理が終わった後の演出処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformeOnBonusProcessingFinished()
    {
        // ドラムロール効果音がまだ鳴っていれば消す
        if (instantiatedSEObject != null)
        {
            Destroy(instantiatedSEObject);
        }

        // パフォーマーにポジティブモーションを取らせる
        performer.Animator.SetTrigger(Performer.hashPositive);

        // ドラムロール終了SEを再生する
        Instantiate(drumRollFinishedPrefab);

        // 指定された秒数待つ
        yield return new WaitForSeconds(waitTimeAfterDrumRollFinished);

        // 歓声SEを再生する
        Instantiate(CheerSEPrefab);

        // ハイスコアを更新した場合
        if (CheckHighScore())
        {
            // ハイスコアの登録とセーブを行う
            SaveManager.Instance.Register(player.Score);
            SaveManager.Instance.Save();

            // ハイスコアのテキストの色を変更する
            highScoreText.color = UpdateHighScoreColor;
        }

        // 指定された秒数待つ
        yield return new WaitForSeconds(waitTimeAfterCheerSE);

        // メッセージを表示する
        messageObject.SetActive(true);
    }

    /// <summary>
    /// 今回のスコアがハイスコアかどうか調べる
    /// </summary>
    /// <returns></returns>
    private bool CheckHighScore()
    {
        bool result = false;

        int highScore = SaveManager.Instance.SaveData.highScore;
        if (player.Score > highScore)
        {
            result = true;
        }
        
        return result;
    }

    /// <summary>
    /// スペースキーが押されたときの処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnKeyDownSpace()
    {
        // 実行フラグON
        excutedSceneSwitchCoroutine = true;

        // 指さしポーズを取らせる
        //performer.Animator.SetTrigger(Performer.hashPointAt);

        // 指定された秒数待つ
        yield return new WaitForSeconds(waitTimeBeforeSwitchScene);

        // フェードアウト処理
        fade.FadeOut(fadeOutTime);

        // フェードアウトが終わるまで待つ
        yield return new WaitForSeconds(fadeOutTime);

        // Titleシーンへ移行する
        SceneManager.LoadScene(SceneName.Title);
    }
}