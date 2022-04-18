using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownLogic : MonoBehaviour
{
    // カウントダウン秒数
    [SerializeField]
    float seconds = 3.5f;
    public float Seconds
    {
        get
        {
            return seconds;
        }
        private set
        {
            if (value < 0.0f)
            {
                value = 0.0f;
                Stop();
            }
            seconds = value;
        }
    }

    // カウント停止中か
    public bool IsStopping { get; private set; } = false;

    private void Update()
    {
        UpdateCountDown();
    }

    private void UpdateCountDown()
    {
        if (!IsStopping)
        {
            Seconds -= Time.deltaTime;
        }
    }

    public void StartCount()
    {
        IsStopping = false;
    }

    public void Stop()
    {
        IsStopping = true;
    }
}
