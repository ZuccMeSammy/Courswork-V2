using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    // Public fields
    public int hitpoint;
    public int maxHitpoint;
    public float pushRecoverySpeed = .2f;

    // Immunity
    protected float immuneTime = 1.0f;
    protected float lastImmune;

    //Push
    protected Vector3 pushDirection;

    public EnemyAi enemy;

    // Recieving damage
    protected virtual void RecieveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            hitpoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            Debug.Log(dmg.damageAmount);

            if (hitpoint <= 0)
            {
                hitpoint = 0;
                Destroy(gameObject);
            }
        }

    }

}
