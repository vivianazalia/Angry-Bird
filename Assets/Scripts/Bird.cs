using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{
    public enum BirdState{ Idle, Thrown, HitSomething}
    public GameObject parent;
    public Rigidbody2D rigidbody;
    public CircleCollider2D collider;

    private BirdState state;
    private float minVelocity = 0.05f;
    private bool flagDestroy = false;

    public UnityAction OnBirdDestroyed = delegate { };
    public UnityAction<Bird> OnBirdShot = delegate { };

    public BirdState State { get { return state; } }            //getter setter BirdState

    void Start()
    {
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        collider.enabled = false;
        state = BirdState.Idle;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        state = BirdState.HitSomething;
    }

    private void FixedUpdate()
    {
        if(state == BirdState.Idle && rigidbody.velocity.sqrMagnitude >= minVelocity)
        {
            state = BirdState.Thrown;
        }

        if((state == BirdState.Thrown || state == BirdState.HitSomething) && rigidbody.velocity.sqrMagnitude <= minVelocity && !flagDestroy)
        {
            flagDestroy = true;
            StartCoroutine(DestroyAfter(2));
        }
    }

    IEnumerator DestroyAfter(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    public void MoveTo(Vector2 target, GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = target;
    }

    public void Shoot(Vector2 velocity, float distance, float speed)
    {
        collider.enabled = true;
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        rigidbody.velocity = velocity * distance * speed;
        OnBirdShot(this);
    }

    void OnDestroy()
    {
        if(state == BirdState.Thrown || state == BirdState.HitSomething)
        {
            OnBirdDestroyed();
        }
    }

    public virtual void OnTap()
    {
        //tidak melakukan apa-apa karena pada saat burung merah di lempar memang tidak terjadi apa-apa
        //kata kunci virtual berarti fungsi ini dapat di-override di class inheritance-nya
    }
}
