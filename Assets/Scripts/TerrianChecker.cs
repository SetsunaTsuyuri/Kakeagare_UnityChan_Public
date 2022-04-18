using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrianChecker : MonoBehaviour
{
    // 地形から離れたか
    public bool ExitTerrian { get; private set; } = false;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(TagName.Terrain))
        {
            ExitTerrian = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagName.Terrain))
        {
            ExitTerrian = false;
        }
    }
}
