using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE : MonoBehaviour
{
    public AudioSource AudioSource { get; private set; } = null;

    private void Awake()
    {
        // AudioScourceコンポーネントを取得する
        AudioSource = GetComponent<AudioSource>();

        // 効果音再生した後にDestroyする。音の再生はPlay On Awake
        Destroy(gameObject, AudioSource.clip.length);
    }
}