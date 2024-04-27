using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    
    public Rigidbody2D Rigid;
    public float Speed;
    private bool Groundbool = false;
    public static bool Stopbool = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Groundbool == true)
        {
            if (transform.localScale.x == 5)
            {
                transform.localScale = new Vector3(-5, 5, 5);
                Groundbool = false;
            }
            else if (transform.localScale.x == -5)
            {
                transform.localScale = new Vector3(5, 5, 5);
                Groundbool = false;
            }
        }
        if(Groundbool == false && Stopbool == false)
        {
            if (transform.localScale.x == 5)
            {
                Rigid.velocity = Vector2.right * Speed;
            }
            if (transform.localScale.x == -5)
            {
                Rigid.velocity = Vector2.left * Speed;
            }
        }

        if(Stopbool == true)
        {
            Rigid.velocity = Vector2.right * 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            Groundbool = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            Groundbool = false;
        }
    }


}
