using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShooter : MonoBehaviour
{
    public CircleCollider2D collider;
    public LineRenderer trajectory;
    private Vector2 startPos;                           //posisi awal dari tali ditarik
    private Bird bird;

    [SerializeField] private float radius = 0.75f;      //panjang maksimal tali ditarik
    [SerializeField] private float throwSpeed = 30f;    //kecepatan awal yang diberikan katapel saat pertama kali melontarkan katapel

    void Start()
    {
        startPos = transform.position;
    }

    private void OnMouseUp()
    {
        collider.enabled = false;
        Vector2 velocity = startPos - (Vector2)transform.position;
        float distance = Vector2.Distance(startPos, transform.position);

        bird.Shoot(velocity, distance, throwSpeed);

        gameObject.transform.position = startPos;
        trajectory.enabled = false;
    }

    private void OnMouseDrag()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - startPos;

        if(dir.sqrMagnitude > radius)
        {
            dir = dir.normalized * radius;
        }

        transform.position = startPos + dir;

        float distance = Vector2.Distance(startPos, transform.position);

        if (!trajectory.enabled)
        {
            trajectory.enabled = true;
        }

        DisplayTrajectory(distance);
    }

    void DisplayTrajectory(float distance)
    {
        if(bird == null)
        {
            return;
        }

        Vector2 velocity = startPos - (Vector2)transform.position;
        int segmentCount = 5;
        Vector2[] segments = new Vector2[segmentCount];

        //posisi awal trajectory adalah posisi mouse bird saat ini 
        segments[0] = transform.position;

        //kecepatan awal
        Vector2 segVelocity = velocity * throwSpeed * distance;

        for(int i = 1; i < segmentCount; i++)
        {
            float elapsedTime = i * Time.deltaTime * 8;
            segments[i] = segments[0] + segVelocity * elapsedTime * 1 + Physics2D.gravity * Mathf.Pow(elapsedTime, 2);
        }

        trajectory.positionCount = segmentCount;

        for(int i = 0; i < segmentCount; i++)
        {
            trajectory.SetPosition(i, segments[i]);
        }
    }

    public void InitiateBird(Bird aBird)
    {
        bird = aBird;
        bird.MoveTo(gameObject.transform.position, gameObject);
        collider.enabled = true;
    }
}
