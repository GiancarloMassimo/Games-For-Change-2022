using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableCar : InteractionTrigger
{
    [SerializeField] Collider2D triggerCollider;
    Vector2 desiredPos = new Vector2(3.75f, 0);

    private void Update()
    {
        triggerCollider.offset = -transform.localPosition;

        if (Input.GetKeyDown(KeyCode.E) && indicator.Showing && touching)
        {
            Action();
            indicator.ToggleIndicator(false);
        }

        transform.localPosition = Vector2.MoveTowards(transform.localPosition, desiredPos, Time.deltaTime);
    }

    void Action()
    {
        desiredPos = transform.localPosition.x > 0 ? new Vector2(-3.75f, 0) : new Vector2(3.75f, 0);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(Vector2.Distance(collision.transform.position, transform.position) < 1f ? transform : null);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
