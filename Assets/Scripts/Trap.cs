using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MovableObject
{
    // 攻撃力
    [SerializeField]
    int offensePower = 1;
    public int OffensePower
    {
        get
        {
            return offensePower;
        }
        private set
        {
            offensePower = value;
        }
    }

    // プレイヤーが触れると破壊されるか
    [SerializeField]
    bool isBreakable = true;
    bool IsBreakable
    {
        get
        {
            return isBreakable;
        }
        set
        {
            isBreakable = value;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーに触れた場合
        if (collision.CompareTag(TagName.Player))
        {
            // コンポーネント取得
            Player player = collision.GetComponent<Player>();
            
            // 動きを止める
            player.Stop();

            // ダメージを与える
            player.RecieveDamage(OffensePower);

            // 破壊されるタイプの場合
            if (IsBreakable)
            {
                // 消滅エフェクトを生成する
                InstantiateDestructionEffect();

                // 自壊する
                Destroy(gameObject);
            }
        }
    }
}
