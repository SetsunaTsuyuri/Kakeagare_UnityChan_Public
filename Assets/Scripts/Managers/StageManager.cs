using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    // 生成される候補となるステージプレファブリスト
    [SerializeField]
    List<GameObject> stagePrefabList = new List<GameObject>();
    public List<GameObject> StagePrefabList
    {
        get
        {
            return stagePrefabList;
        }
        private set
        {
            stagePrefabList = value;
        }
    }

    // 最終ステージのプレファブ
    [SerializeField]
    GameObject finalStagePrefab = null;
    public GameObject FinalStagePrefab
    {
        get
        {
            return finalStagePrefab;
        }
        private set
        {
            finalStagePrefab = value;
        }
    }

    // 生成されたステージオブジェクトリスト
    [SerializeField]
    List<GameObject> stageObjectList = new List<GameObject>();
    public List<GameObject> StageObjectList
    {
        get
        {
            return stageObjectList;
        }
        private set
        {
            stageObjectList = value;
        }
    }

    // ステージの親となるオブジェクトのトランスフォーム
    [SerializeField]
    Transform stageParent = null;
    public Transform StageParent
    {
        get
        {
            return stageParent;
        }
        private set
        {
            stageParent = value;
        }
    }

    // 背景のイメージ
    [SerializeField]
    Image backgroundImage = null;
    public Image BackgroundImage
    {
        get
        {
            return backgroundImage;
        }
        private set
        {
            backgroundImage = value;
        }
    }

    // ステージの全体数
    public int StageCount { get; private set; } = 0;

    // ステージスタートライン
    public StartLine StartLine { get; private set; } = null;

    // ステージエンドライン
    public EndLine EndLine { get; private set; } = null;

    // 生成されるステージのY座標オフセット
    public float NextStageHeightOffset { get; private set; } = 0.0f;

    private void Awake()
    {
        // ステージ生成位置Y座標オフセットに、ステージの高さを加算する
        NextStageHeightOffset = StageObjectList[0].GetComponentInChildren<EndLine>().transform.localPosition.y;

        // ステージプレファブリストの順番をシャッフルする
        StagePrefabList.Shuffle();

        // ステージプレファブリストに最終ステージを加える
        StagePrefabList.Add(FinalStagePrefab);

        // ステージの全体数を数える
        StageCount += stagePrefabList.Count + stageObjectList.Count;

        // ステージを2個作る
        for (int i = 0; i < 2; i++)
        {
            CreateStage();
        }

        // スタートおよびエンドラインコンポーネントを取得する
        GetStageLineComponents();
    }

    private void Update()
    {
        // プレイヤーがスタートラインに触れた場合、ステージの背景を更新する
        if (StartLine != null && StartLine.IsTouchedByPlayer)
        {
            UpdateBackgroundColor();
        }

        // プレイヤーがエンドラインに触れた場合、ステージを更新する
        if (EndLine != null && EndLine.IsTouchedByPlayer)
        {
            UpdateStages();
        }
    }

    /// <summary>
    /// ステージの背景色を更新する
    /// </summary>
    private void UpdateBackgroundColor()
    {
        // 背景色を徐々に変更する
        StartCoroutine(ChangeBackgroundColor());

    }

    /// <summary>
    /// 背景色を徐々に変更する
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeBackgroundColor()
    {
        // 変化前の色
        Color startColor = BackgroundImage.color;

        // 変化後の色
        Color endColor = StartLine.BackgroundColor;

        // 色が完全に変わるまでの所要時間
        float duration = StartLine.ColorChangeDuration;

        // 変色処理が始まってからの経過時間
        float elapsedTime = 0.0f;

        // 補間値
        float interpolation = 0.0f;

        // 変色処理
        while (interpolation < 1.0f)
        {
            // 経過時間を増やす
            elapsedTime += Time.deltaTime;

            // 補間値を求める
            interpolation = elapsedTime / duration;

            // 背景の色を変更する
            BackgroundImage.color = Color.Lerp(startColor, endColor, interpolation);

            // フレームが終わるまで待つ
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// ステージを更新する
    /// </summary>
    private void UpdateStages()
    {
        if (StagePrefabList.Count == 0)
        {
            return;
        }

        DestroyOldStage();
        CreateStage();
        GetStageLineComponents();
    }

    /// <summary>
    /// ステージを生成する
    /// </summary>
    public void CreateStage()
    {
        if (StagePrefabList.Count == 0)
        {
            Debug.Log("ステージ候補がなくなった");
            return;
        }

        // 生成するステージのプレファブ
        GameObject stagePrefab = StagePrefabList[0];

        // ステージオブジェクトを生成する
        GameObject stageObject = Instantiate(stagePrefab, StageParent);
        stageObject.transform.localPosition = Vector2.up * NextStageHeightOffset;

        // 生成したステージのプレファブを生成候補リストから削除する
        StagePrefabList.Remove(stagePrefab);

        // ステージオブジェクトリストに加える
        StageObjectList.Add(stageObject);

        // ステージ生成位置Y座標オフセットに、ステージの高さを加算する
        EndLine endLine = stageObject.GetComponentInChildren<EndLine>();
        if (endLine != null)
        {
            float height = endLine.transform.localPosition.y;
            NextStageHeightOffset += height;
        }
    }

    /// <summary>
    /// ステージを削除する
    /// </summary>
    public void DestroyOldStage()
    {
        if (StageObjectList.Count == 0)
        {
            Debug.Log("ステージは全て破壊された");
            return;
        }

        // リスト0番目のステージオブジェクトを破壊する
        GameObject stageObject = StageObjectList[0];
        Destroy(stageObject);

        // リストからも消去する
        stageObjectList.RemoveAt(0);
    }

    /// <summary>
    /// イベント発生ラインコンポーネントを取得する
    /// </summary>
    public void GetStageLineComponents()
    {
        // リストの1番目のステージを取得する
        GameObject stageObject = StageObjectList[1];

        // StartLineコンポーネントを取得する
        StartLine = stageObject.GetComponentInChildren<StartLine>();

        // EndLineコンポーネントを取得する
        EndLine = stageObject.GetComponentInChildren<EndLine>();
    }
}
