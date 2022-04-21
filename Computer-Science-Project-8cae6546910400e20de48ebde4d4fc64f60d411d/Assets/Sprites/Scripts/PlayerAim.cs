using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    
    private Transform aimTransform;

    private void Awake()
    {
        aimTransform = transform.Find("Aim");
    }

    private void update()
    {
        Vector3 mousePosition = GetMousePos();

        Vector3 aimDir = (mousePosition - transform.position).normalized;

        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Debug.Log(angle);
    }
    public static Vector3 GetMousePos()
    {
        Vector3 vec = GetMousePosWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMousePosWithZ()
    {
        return GetMousePosWithZ(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMousePosWithZ(Camera worldCamera)
    {
        return GetMousePosWithZ(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMousePosWithZ(Vector3 screenPos, Camera worldCamera)
    {
        Vector3 worldPos = worldCamera.ScreenToWorldPoint(screenPos);
        return worldPos;
    }
}
