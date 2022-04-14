using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonSpawner : MonoBehaviour
{
    [SerializeField] GameObject person;
    [SerializeField] int people;

    void Start()
    {
        for (int i = 0; i < people; i++)
        {
            GameObject p = Instantiate(person, (Vector2)transform.position + Vector2.right * Random.Range(-.75f, .75f), Quaternion.identity);
        }
    }
}
