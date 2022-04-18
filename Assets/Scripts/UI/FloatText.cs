using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatText : MonoBehaviour
{
    // 動く速さ
    [SerializeField]
    float moveSpeed = 1.0f;

    // フェードアウトにかかる時間
    [SerializeField]
    float fadeOutTime = 1.5f;

    // Textコンポーネント
    public Text Text { get; set; } = null;

    // フェードアウト開始時の色(テキストの元の色)
    Color startColor = new Color();

    // フェードアウト終了時の色(テキストの元の色の透明度をゼロにした色);
    Color endColor = new Color();

    // 経過時間
    float elapsedTime = 0.0f;

    private void Awake()
    {
        // テキストコンポーネントを取得する
        Text = GetComponentInChildren<Text>();

        // フェードアウト開始時の色(テキストの元の色)を取得する
        startColor = Text.color;

        // フェードアウト終了時の色(テキストの元の色の透明度をゼロにした色)を求める
        endColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f);

        // 自身のsortingLayerをUIにする
        GetComponent<Canvas>().sortingLayerName = SortingLayerName.UI;
    }

    private void Update()
    {
        // 移動処理
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // フェードアウトの補間値を決める
        elapsedTime += Time.deltaTime;
        float interpolation = elapsedTime / fadeOutTime;

        // フェードアウト処理。テキストの色を変更する
        Text.color = Color.Lerp(startColor, endColor, interpolation);

        // 透明度がゼロなったら自身を破壊する
        if (Text.color.a == 0.0f)
        {
            Destroy(gameObject);
        }
    }
}