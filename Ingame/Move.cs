using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointA.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;
        if(currentPoint == pointA.transform)
        {
            rb.velocity = new Vector2(-speed, 0);
        }else{
            rb.velocity = new Vector2(speed, 0);
        }
        if(Vector2.Distance(transform.position, currentPoint.position) < 0.2f && currentPoint == pointA.transform){
            flip();
            currentPoint = pointB.transform;
        }
        if(Vector2.Distance(transform.position, currentPoint.position) < 0.2f && currentPoint == pointB.transform){
            flip();
            currentPoint = pointA.transform;
        }
    }
    
    private void flip(){
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    
    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(pointA.transform.position,0.2f);
        Gizmos.DrawWireSphere(pointB.transform.position,0.2f);
        Gizmos.DrawLine(pointA.transform.position,pointB.transform.position);
    }
}



