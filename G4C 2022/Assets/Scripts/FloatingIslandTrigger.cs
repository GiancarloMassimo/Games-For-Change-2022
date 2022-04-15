using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingIslandTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameMetrics.Instance.floatingIslandTriggered = true;
            GameMetrics.Instance.UpdateUnlocks();
        }
    }
}
