using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentStage : MonoBehaviour
{
    // プレイヤー
    [SerializeField]
    Player player = null;

    // ステージマネージャー
    [SerializeField]
    StageManager stageManager = null;

    // 現在の階層表示テキスト
    Text currentStageText = null;

    private void Awake()
    {
        // コンポーネント取得
        currentStageText = GetComponent<Text>();
    }

    private void Update()
    {
        // プレイヤーのいる階層の表示が反映されていない場合、その表示を更新する
        if (!player.CurrentStageIsReferectedOnUI)
        {
            UpdateCurrentStage();
        }

    }

    /// <summary>
    /// 現在の階層表示テキストを更新する
    /// </summary>
    private void UpdateCurrentStage()
    {
        string textMessage = string.Format("{0}F / {1}F", player.CurrentStage, stageManager.StageCount);
        currentStageText.text = textMessage;

        // 反映フラグON
        player.CurrentStageIsReferectedOnUI = true;
    }
}
