using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_move : MonoBehaviour
{
    public GameObject user;

    public float speed = 10.0f;
    public Transform cameraTarget;

    private Camera thiscamera;
    private Vector3 worldDefault;

    // Start is called before the first frame update
    void Start()
    {
        thiscamera = GetComponent<Camera>();
        worldDefault = transform.position;

        user = GameObject.Find("player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cameraTarget = user.transform;
        float scroll = Input.GetAxis("Mouse ScrollWheel") * speed;
        
        if(thiscamera.fieldOfView <= 20.0f && scroll < 0)
        {
            thiscamera.fieldOfView = 20.0f;
        }
        else if(thiscamera.fieldOfView >= 60.0f && scroll >0)
        {
            thiscamera.fieldOfView = 60.0f;
        }
        else
        {
            thiscamera.fieldOfView += scroll;
        }

        if(cameraTarget && thiscamera.fieldOfView <= 30.0f)
        {
            Vector3 view = new Vector3((cameraTarget.position - transform.position).x, transform.position.y, (cameraTarget.position - transform.position).z);
            transform.position = Vector3.Slerp(transform.position, view, 0.15f);
        }

        else
        {
            transform.position = Vector3.Slerp(transform.position, worldDefault, 0.15f);

        }
    }
}
