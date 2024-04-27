using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundOrWallCollider : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)    // 지면 닿음 시 점프가능
    {
        if (collision.collider.gameObject.CompareTag("Playerfeet"))
        {
            MainCharacterController.isgrounded = true;
            MainCharacterController.canjumptwicebool = false;
            animator.SetInteger("JumpInt", 0);
        }

    }

    private void OnCollisionStay2D(Collision2D collision)    // 지면 닿음 시 점프가능
    {
        if (collision.collider.gameObject.CompareTag("Playerfeet"))
        {
            MainCharacterController.isgrounded = true;
            MainCharacterController.canjumptwicebool = false;
            animator.SetInteger("JumpInt", 0);
        }

    }

    private void OnCollisionExit2D(Collision2D collision)    // 지면 닿음 시 점프가능
    {
        if (collision.collider.gameObject.CompareTag("Playerfeet"))
        {
            MainCharacterController.isgrounded = false;
            MainCharacterController.canjumptwicebool = true;
        }

    }
}
