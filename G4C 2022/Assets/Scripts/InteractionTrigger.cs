using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    [SerializeField] int activationTime = 10;
    [SerializeField] protected string message;
    bool activated = false;
    protected bool touching;

    protected InteractionIndicator indicator;

    void Start()
    {
        indicator = GameObject.FindGameObjectWithTag("InteractionIndicator").GetComponent<InteractionIndicator>();
        Invoke(nameof(TurnOn), activationTime);
    }

    IEnumerator Show()
    {
        yield return new WaitForSeconds(0.5f);
        indicator.ToggleIndicator(true, message);
    }

    void TurnOn()
    {
        activated = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && activated)
        {
            touching = true;
            StartCoroutine(Show());
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopAllCoroutines();
            touching = false;
            indicator.ToggleIndicator(false);
        }
    }
}
