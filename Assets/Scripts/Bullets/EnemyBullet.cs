using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーに命中した場合
        if (collision.CompareTag(TagName.Enemy))
        {
            // コンポーネント取得
            Player player = collision.GetComponent<Player>();

            // 動きを止める
            player.Stop();

            // ダメージを与える
            player.RecieveDamage(offsensePower);

            // 自分自身を破壊する
            Destroy(gameObject);
        }
    }
}
