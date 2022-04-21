using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    
    //PlayerMovement
    public float moveSpeed = 5f; //The speed of the players movement
    public Rigidbody2D rb; //acts as the motor to move the player
    Vector2 movement; //stores the x and y coordinates from void update so it can be called elsewhere
    public Animator animator; //references the animated component 


    //PlayerCombat and interaction
    public Vector3 position;
    private bool dashClick;
    private Vector3 moveDir;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float lastFire;
    public float fireDelay;
    public Pointer crosshair;
    public bool endOfAiming;
    public Transform attackPoint; //references the attack point in unity
    public LayerMask enemyLayers; //Add all the enemies to the enemy layer then all the objects in this layer will be detected by the layer mask (hit detection)
    public Enemy damageFunction;
    public int maxHealth = 100;
    private int currentHealth;

    public Text collectedText;
    public static int collectedAmount = 0;
    public Transform interactor;
    
  
 

    void Update() //Don't put movement in here as it will tie physics to framerate but use for inputs
    {
        //Dealing with player movement
        movement.x = Input.GetAxisRaw("Horizontal"); //Gives a value of 1 or -1 depending on keybaord input (left and right arrows)
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            animator.SetFloat("LastHorizontal", Input.GetAxisRaw("Horizontal"));
            animator.SetFloat("LastVertical", Input.GetAxisRaw("Vertical"));
        }

        if (Input.GetAxisRaw("Horizontal") > 0)
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

        moveDir = new Vector3(movement.x, movement.y).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dashClick = true;
        }


        //Dealing with Player Combat
        float shootHor = Input.GetAxis("ShootHorizontal");
        float shootVer = Input.GetAxis("ShootVertical");

        if ((shootHor != 0 || shootVer != 0) && Time.time > lastFire + fireDelay)
        {
            //Shoot(shootHor, shootVer);
            lastFire = Time.time;
        }

        collectedText.text = "Item Collected: " + collectedAmount;

    }

    private void FixedUpdate() //executed 50 times a second making it more reliable for phsycis
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);  //Rb.MovePosition moves the rigid body to a new position and handles collision detection 
        //rb.position is the current position, Add the movement and times by the speed, The Time.fixedeltatime is the time elapsed since the function was last called creating a constant movement speed. 

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
    }

    void newShoot()
    {
        Vector2 shootingDirection = crosshair.transform.localPosition;
        shootingDirection.Normalize();
    }
}
