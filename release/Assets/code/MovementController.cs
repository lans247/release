using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float movementSpeed = 3.0f;
    Vector3 movement = new Vector3();
    Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        Move();
        RotateToMouseDirection();
    }
    void Move()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        movement.Normalize();
        movement *= movementSpeed * Time.deltaTime;
        rigidbody.MovePosition(transform.position + movement);
    }
    void RotateToMouseDirection()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10.0f);

        float angle = Mathf.Atan2(
            this.transform.position.z - mouseWorldPosition.z,
            this.transform.position.x - mouseWorldPosition.x) * Mathf.Rad2Deg;

        angle = -(angle + 90f);
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, angle, 0f));
    }
}
