﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if(tag == "Bird" || tag == "Enemy" || tag == "Obstacle")
        {
            Destroy(collision.gameObject);
        } 
    }
}
