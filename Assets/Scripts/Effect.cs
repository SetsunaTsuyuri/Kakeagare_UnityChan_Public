using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    // エフェクト発生時に再生する効果音のプレファブ
    [SerializeField]
    GameObject audioPrefab = null;

    private void Awake()
    {
        // 効果音を再生する
        Instantiate(audioPrefab);
    }
}