using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public GameObject crosshairs;
    public GameObject player;
    private Vector3 target;
    public GameObject swordPrefab;
    public float swingSpeed = 60.0f;
   
    void Start()
    {
        Cursor.visible = false;//hides the mouse cursor from the screen leaving only the crosshair
    }

    void Update()
    {
        target = transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));//Gives the location of the mouse in the game

        crosshairs.transform.position = new Vector2(target.x, target.y);//assigns the crosshair position to the mouse position

        Vector3 difference = target - player.transform.position;//calculates the differnce between the mouse position and player position

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; //

        player.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }
}
