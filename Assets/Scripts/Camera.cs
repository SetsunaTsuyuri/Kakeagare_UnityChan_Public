using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    // 追跡対象
    [SerializeField]
    Transform target = null;
    Transform Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
        }
    }

    // 対象を追跡する速度
    [SerializeField]
    float followSpeed = 2.0f;
    float FollowSpeed
    {
        get
        {
            return followSpeed;
        }
        set
        {
            followSpeed = value;
        }
    }

    // 追跡対象と自分自身(カメラ)の相対距離
    Vector3 Offset { get; set; } = Vector3.zero;

    private void Start()
    {
        // カメラと追跡対象の相対距離を求める
        Offset = transform.position - Target.position;
    }

    private void LateUpdate()
    {
        // カメラ追従処理
        Vector3 nextPosition = Target.position + Offset;
        transform.position = Vector3.Lerp(transform.position, nextPosition, FollowSpeed * Time.deltaTime);
    }

}
