using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : Fighter
{
    //[SerializeField] private LayerMask dashLayerMask;
 
    public float moveSpeed = 5f; //The speed of the players movement
    
    public Rigidbody2D rb; //acts as the motor to move the player
    
    Vector2 movement; //stores the x and y coordinates from void update so it can be called elsewhere
    
    public Animator animator; //references the animated component 
    
    public Transform interactor; //handles player interaction
    
    public Vector3 position;
    
    private bool dashClick;
    
    private Vector3 moveDir;
    
    public Text collectedText;

    public static int collectedAmount = 0;

    private RaycastHit2D hit;

    private BoxCollider2D boxCollider;

    void Update() //Don't put movement in here as it will tie physics to framerate but use for inputs
    {
        movement.x = Input.GetAxisRaw("Horizontal"); //Gives a value of 1 or -1 depending on keybaord input (left and right arrows)
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if(Input.GetAxisRaw("Horizontal")==1 || Input.GetAxisRaw("Horizontal")==-1 || Input.GetAxisRaw("Vertical")==1 || Input.GetAxisRaw("Vertical")==-1) {
            animator.SetFloat("LastHorizontal", Input.GetAxisRaw("Horizontal"));
            animator.SetFloat("LastVertical", Input.GetAxisRaw("Vertical"));
        }
        
        if(Input.GetAxisRaw("Horizontal") > 0)
        {
            interactor.localRotation = Quaternion.Euler(0, 0, 90);
        }
       
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            interactor.localRotation = Quaternion.Euler(0, 0, -90);
        }
        
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            interactor.localRotation = Quaternion.Euler(0, 0, 180);
        }
        
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            interactor.localRotation = Quaternion.Euler(0, 0, 0);
        }
        //Corresponding to the position of the player the position of the interactor will follow

        moveDir = new Vector3 (movement.x, movement.y).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dashClick = true;
        }

        collectedText.text = "Item Collected: " + collectedAmount;
        
    }

    private void FixedUpdate() //executed 50 times a second making it more reliable for phsycis
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);  //Rb.MovePosition moves the rigid body to a new position and handles collision detection 
        //rb.position is the current position, Add the movement and times by the speed, The Time.fixedeltatime is the time elapsed since the function was last called creating a constant movement speed. 

        // Add push vector, if any
        transform.position += pushDirection;

        //Reduce push force every frame, based on recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        if (dashClick == true)
        {
            float dashAmount = 3f;
            
            Vector3 dashPosition = (transform.position + moveDir * dashAmount);
            
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, moveDir, dashAmount);
            
            if (raycastHit2D.collider != null)
            {
                dashPosition = raycastHit2D.point;
            }
            
            rb.MovePosition(transform.position + moveDir * dashAmount);
            
            dashClick = false;
        }
/*
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDir.y), Mathf.Abs(moveDir.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking")); //49 mins in
        if (hit.collider == null)
        {
            transform.Translate(0, moveDir.y * Time.deltaTime, 0);
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDir.x, 0), Mathf.Abs(moveDir.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking")); //49 mins in
        if (hit.collider == null)
        {
            transform.Translate(moveDir.x * Time.deltaTime, 0, 0);
        }
*/
    }
    
}
