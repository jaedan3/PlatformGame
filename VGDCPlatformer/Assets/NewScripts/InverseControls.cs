﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseControls : MonoBehaviour {
    PlayerMovementBeginner player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementBeginner>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && player.getInverse() == false)
        {
            StartCoroutine(Wait());   
        }
    }

    IEnumerator Wait()
    {
        player.changeInverse(true);
        StartCoroutine(Wait2());
        yield return new WaitForSecondsRealtime(5);
        player.changeHInput("Horizontal");
        player.changeInverse(false);
    }

    IEnumerator Wait2()
    {
        player.changeHInput("Horizontal");
        yield return new WaitForSecondsRealtime(1);
        player.changeHInput("Horizontal2");
    }
}
