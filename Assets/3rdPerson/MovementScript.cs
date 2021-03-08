using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;
    public Transform cam;
    public float walkingSpeed = 5.0f;
    public float turningSpeed = 0.1f;
    float turnVelocity;
    float jumpSpeed = 10.0f;
    Vector3 direction = Vector3.zero;
    float gravity = 20.0f;

    [HideInInspector]
    public bool canMove = true;

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float veritcalInput = Input.GetAxisRaw("Vertical");
        if (controller.isGrounded)
        {
            Debug.Log("Here");
            float curSpeedX = canMove ? walkingSpeed * veritcalInput : 0;
            float curSpeedY = canMove ? walkingSpeed * horizontalInput : 0;
            direction = (Vector3.forward * curSpeedX) + (Vector3.right * curSpeedY);
            if (Input.GetButton("Jump") && canMove)
            {
                direction.y = jumpSpeed;
            }
        }
        direction.y -= gravity * Time.deltaTime;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turningSpeed);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        controller.Move(direction * Time.deltaTime);

        #region animation
        if (!controller.isGrounded)
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
        if (veritcalInput > 0)
        {
            animator.SetBool("isWalking_Left", false);
            animator.SetBool("isWalking_Right", false);
            animator.SetBool("isWalking_F", true);
            animator.SetBool("isWalking_Back", false);
        }
        else if (veritcalInput < 0)
        {
            animator.SetBool("isWalking_Left", false);
            animator.SetBool("isWalking_Right", false);
            animator.SetBool("isWalking_F", false);
            animator.SetBool("isWalking_Back", true);
        }
        if (horizontalInput > 0)
        {
            animator.SetBool("isWalking_Left", false);
            animator.SetBool("isWalking_Right", true);
            animator.SetBool("isWalking_F", false);
            animator.SetBool("isWalking_Back", false);
        }
        else if (horizontalInput < 0)
        {
            animator.SetBool("isWalking_Left", true);
            animator.SetBool("isWalking_Right", false);
            animator.SetBool("isWalking_F", false);
            animator.SetBool("isWalking_Back", false);
        }

        if(veritcalInput==0 & horizontalInput == 0 || animator.GetBool("isJumping"))
        {
            animator.SetBool("isWalking_Left", false);
            animator.SetBool("isWalking_Right", false);
            animator.SetBool("isWalking_F", false);
            animator.SetBool("isWalking_Back", false);
        }

        #endregion animation

    }

}
