using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tank : MonoBehaviour
{
    public GameObject target;
    public Vector3 target_po;

    public float speed;
    public float range;

    // Start is called before the first frame update
    void Start()
    {
        range = Random.Range(1, 3);
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        target_po = this.GetComponent<enemy>().target_po;
        speed = this.GetComponent<enemy>().speed;
        tanker();
    }

    void tanker()
    {
        if (target_po != null)
        {
            this.transform.LookAt(target_po);
            if (Vector3.Distance(this.transform.position, target_po) > range)
            {
                this.transform.Translate(new Vector3(0f, 0f, speed));
            }
        }
    }
}
