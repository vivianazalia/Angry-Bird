using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public float health = 50;
    private bool isHit = false;

    public UnityAction<GameObject> OnEnemyDestroyed = delegate { };

    private void OnDestroy()
    {
        if (isHit)
        {
            OnEnemyDestroyed(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Rigidbody2D>() == null)
        {
            return;
        }

        if(collision.gameObject.tag == "Bird" || collision.gameObject.tag == "Border")
        {
            isHit = true;
            Destroy(gameObject);
        } 
        else if(collision.gameObject.tag == "Obstacle")
        {
            float damage = collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
            health -= damage;

            if (health <= 0)
            {
                isHit = true;
                Destroy(gameObject);
            }
        } 
    }
}
