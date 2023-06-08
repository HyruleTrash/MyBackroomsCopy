using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuzzDistance : MonoBehaviour
{
    public AudioSource Buzz;
    public float Distance = 10;
    public GameObject Player;

    private void Start()
    {
        Player = GameObject.Find("Player");
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) < Distance)
        {
            Buzz.enabled = true;
        }
        else
        {
            Buzz.enabled = false;
        }
    }
}
