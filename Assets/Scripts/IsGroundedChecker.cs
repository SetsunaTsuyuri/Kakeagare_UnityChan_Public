using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGroundedChecker : MonoBehaviour
{
    // 接地しているか
    bool IsGrounded { get; set; } = false;

    // 入った
    bool IsGroundedEnter { get; set; } = false;

    // 途中
    bool IsGroundedStay { get; set; } = false;

    // 出た
    bool IsGroundedExit { get; set; } = false;

    // 接地判定
    public bool UpdateIsGrounded()
    {
        if (IsGroundedEnter || IsGroundedStay)
        {
            IsGrounded = true;
        }
        else if (IsGroundedExit)
        {
            IsGrounded = false;
        }

        IsGroundedEnter = false;
        IsGroundedStay = false;
        IsGroundedExit = false;

        return IsGrounded;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagName.Terrain)
        {
            IsGroundedEnter = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == TagName.Terrain)
        {
            IsGroundedStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == TagName.Terrain)
        {
            IsGroundedExit = true;
        }
    }
}
