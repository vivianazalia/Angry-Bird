using UnityEngine;

public class YellowBird : Bird
{
    public GameObject explodeEffect;
    public float boostForce = 50;
    public bool hasBoost = false;
    public float radius = 1.5f;
    public float forceExplosion = 700;
    public LayerMask layerObj;

    public void Boost()
    {
        if (State == BirdState.Thrown && !hasBoost)
        {
            rigidbody.AddForce(rigidbody.velocity * boostForce);
            hasBoost = true;
        }
    }

    public void ExplosionBird()
    {
        GameObject particleExplode = (GameObject)Instantiate(explodeEffect, transform.position, transform.rotation);

        Collider2D[] nearbyObject = Physics2D.OverlapCircleAll(transform.position, radius, layerObj);

        foreach (Collider2D nearObj in nearbyObject)
        {
            Vector2 dir = nearObj.transform.position - transform.position;

            Rigidbody2D rbObj = nearObj.GetComponent<Rigidbody2D>();
            rbObj.AddForce(dir * forceExplosion);
        }
        
        Destroy(gameObject);
        Destroy(particleExplode, 1);
    }

    void Update()
    {
        if(State == BirdState.HitSomething)
        {
            ExplosionBird();
        }
    }

    public override void OnTap()
    {
        Boost();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    
}
