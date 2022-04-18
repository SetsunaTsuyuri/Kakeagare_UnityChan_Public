using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    // Playerコンポーネント
    Player player = null;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // 敵に命中した場合
        if (collision.CompareTag(TagName.Enemy))
        {
            // コンポーネント取得
            Enemy enemy = collision.GetComponent<Enemy>();

            // 動きを止める
            enemy.Stop();

            // ダメージを与える
            enemy.RecieveDamage(offsensePower);

            // 死亡した場合
            if (enemy.State == CreatureState.Dead)
            {
                // その敵はプレイヤーによって倒されたものとする
                enemy.IsDefeatedByPlayer = true;

                // プレイヤーが得点を得る
                player.AddScore(enemy.Score);
            }

            // 自分自身を破壊する
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// プレイヤーからコンポーネントを受け取る
    /// </summary>
    /// <param name="player"></param>
    public void RecievePlayerComponentByPlayer(Player player)
    {
        this.player = player;
    }
}