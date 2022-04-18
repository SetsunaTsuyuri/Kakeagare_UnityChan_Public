using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    // フェード処理にかかる時間
    float fadeTime = 0.0f;

    // フェード処理を開始してから経過した時間
    float elapsedTime = 0.0f;

    // フェード処理開始時の透明度
    float startAlpha = 0.0f;

    // フェード処理終了時の透明度
    float endAlpha = 0.0f;

    // Imageコンポーネント
    Image image = null;

    // imageの色
    Color color = new Color();

    private void Start()
    {
        // コンポーネント取得
        image = GetComponent<Image>();

        // imageの色を取得する
        color = image.color;
    }

    private void Update()
    {
        // フェード処理中なら、しかるべき処理を行う
        if (IsFade())
        {
            // 経過時間を増やす
            elapsedTime += Time.deltaTime;

            // Lerp処理の補間値を決める
            float interpolation = elapsedTime / fadeTime;

            // 透明度を変更する
            float alpha = Mathf.Lerp(startAlpha, endAlpha, interpolation);
            image.color = new Color(color.r, color.g, color.b, alpha);

            // 補間値が1以上になったら、フェード処理時間をゼロにする
            if (interpolation >= 1.0f)
            {
                fadeTime = 0.0f;
            }
        }
    }

    /// <summary>
    /// フェードイン処理
    /// </summary>
    public void FadeIn(float fadeTime)
    {
        this.fadeTime = fadeTime;
        elapsedTime = 0.0f;
        
        startAlpha = 1.0f;
        endAlpha = 0.0f;
    }

    /// <summary>
    /// フェードアウト処理
    /// </summary>
    public void FadeOut(float fadeTime)
    {
        this.fadeTime = fadeTime;
        elapsedTime = 0.0f;

        startAlpha = 0.0f;
        endAlpha = 1.0f;
    }

    /// <summary>
    /// フェード処理中か
    /// </summary>
    /// <returns></returns>
    public bool IsFade()
    {
        return fadeTime > 0.0f;
    }
}