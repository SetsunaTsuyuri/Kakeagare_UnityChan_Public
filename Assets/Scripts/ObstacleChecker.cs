using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleChecker : MonoBehaviour
{
    // 障害物に当たったか
    public bool HitObstacle { get; private set; } = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 地形、敵に触れた場合、フラグON
        if (collision.CompareTag(TagName.Terrain) || collision.CompareTag(TagName.Enemy))
        {
            HitObstacle = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 地形、敵から離れた場合、フラグOFF
        if (collision.CompareTag(TagName.Terrain) || collision.CompareTag(TagName.Enemy))
        {
            HitObstacle = false;
        }
    }
}
