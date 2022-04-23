using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,

    Wander,

    Follow,

    Die
};

public class EnemyAi : Fighter
{
    // Logic
    GameObject player;
    public EnemyState currState = EnemyState.Idle;
    public float range;
    public float speed;
    private bool chooseDir = false;
    private bool dead = false;
    private Vector3 randomDir;
    public Enemy callback;

    // Detects if the enemy and room are in the same room
    public bool notInRoom = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        switch (currState)
        {
            case (EnemyState.Idle):
                Idle();
                break;

            case (EnemyState.Wander):

                Wander();

                break;

            case (EnemyState.Follow):

                Follow();

                break;

            case (EnemyState.Die):
                break;
        }

        if (!notInRoom)
        {

            if (IsPlayerInRange(range) && currState != EnemyState.Die)
            {

                currState = EnemyState.Follow;

            }
            else if (!IsPlayerInRange(range) && currState != EnemyState.Die)
            {

                currState = EnemyState.Wander;

            }
        }
        else
        {
            currState = EnemyState.Idle;
        }
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private IEnumerator ChooseDirection()
    {
        chooseDir = true;

        yield return new WaitForSeconds(Random.Range(2f, 8f));

        randomDir = new Vector3(0, 0, Random.Range(0, 360));

        Quaternion nextRotation = Quaternion.Euler(randomDir);

        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));

        chooseDir = false;
    }

    void Wander()
    {
        if (!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }

        transform.position += -transform.right * speed * Time.deltaTime;

        if (IsPlayerInRange(range))
        {

            currState = EnemyState.Follow;

        }

    }

    void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        // Add push vector, if any
        transform.position += pushDirection;

        //Reduce push force every frame, based on recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);
    }

    void Idle()
    {

    }

    public void Die()
    {
        callback.Die();
        Destroy(gameObject);
        dead = true;
    }

}