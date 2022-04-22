using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter
{
    public int maxHealth = 100;
    private int currentHealth;
    public Animator animator;
    private Rigidbody2D rb;

    public Transform Player;
    private Vector2 movement;
    public float moveSpeed = 5f;

    public PlayerCombat attacker;

    void Start()
    {
        currentHealth = maxHealth; //Starting health of the enemy
        rb = this.GetComponent<Rigidbody2D>();//Allows us to manipulate the movement and rotation of the enemy

        //attacker.attack();
    }

    private void Update()
    {
        /*
        Vector3 direction = Player.position - transform.position;//Calculates the enemy's position in relation to the player
        //Debug.Log(direction); //real time test to see if the enemy is following the player's movement
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //calculates the angle between the enemy and the player
        rb.rotation = angle; 
        direction.Normalize(); //makes the direction value -1<x<1
        movement = direction; //defines the direction which the enemy needs to head to
        */
    }

    /*
    private void FixedUpdate()
    {
        movecharacter(movement); //Declares the movecharacter function
        if (moveSpeed > 0)
        {

        }
    }

    void movecharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2) transform.position + (direction * moveSpeed * Time.deltaTime));//moves the enemy in the direction of the player
    }
    */

    public void takeDamage(int damage) //Public as it needs to be called from the PLayerCombat script
    {
        currentHealth -= damage; //Subtracts the current health from the players damage

        //Here is where the damage animation will be called
        animator.SetTrigger("Hurt");

        if(currentHealth <= 0)
        {
            Die(); //die function called when health reaches zero
        }
    }
    public void Die()
    {
        Debug.Log("Enemy Died"); //Placeholder until animations are finished
        animator.SetBool("IsDead", true);
        Destroy(gameObject);

        GetComponent<Collider2D>().enabled = false; //Stops the player from colliding with the enemy's corpse
        this.enabled = false; //disables the script
    }

}
