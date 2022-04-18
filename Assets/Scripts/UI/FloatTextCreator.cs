using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatTextCreator : MonoBehaviour
{
    // ポップアップテキストのプレファブ
    [SerializeField]
    GameObject FloatTextPrefab = null;

    // オフセット
    [SerializeField]
    Vector3 offset = new Vector3(0.0f, 1.0f, 0.0f);

    /// <summary>
    /// 浮かぶ文字を生成する
    /// </summary>
    public void CreateFloatText(int value, Vector3 position)
    {
        // 得点の文字を生成し、コンポーネントを取得する
        FloatText floatText = Instantiate(FloatTextPrefab).GetComponent<FloatText>();

        // その位置を引数の位置+オフセットと同じにする
        floatText.transform.position = position + offset;

        // そのテキストの内容を引数の数値と同じにする
        floatText.Text.text = value.ToString();
    }
}