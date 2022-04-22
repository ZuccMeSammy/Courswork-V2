using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeTime;

    void Start()
    {
        StartCoroutine(DeathDelay());
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2d(Collider2D col)
    {
       if(col.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyAi>().Die();
            Destroy(gameObject);
        }
    }

}
