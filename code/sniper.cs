using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sniper : MonoBehaviour
{

    public GameObject target;
    public Vector3 target_po;

    public float speed;
    public float range;

    public GameObject sbullet;
    public float attack_speed;

    public int damage = 5;
    public string proper;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("player");
        range = Random.Range(25, 30);

        proper = this.gameObject.GetComponent<enemy>().proper;
        StartCoroutine(attack(8f - attack_speed));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        proper = this.gameObject.GetComponent<enemy>().proper;

        target_po = this.GetComponent<enemy>().target_po;
        attack_speed = this.GetComponent<enemy>().attack_speed;
        speed = this.GetComponent<enemy>().speed;
        target = this.GetComponent<enemy>().target;
        move();
    }

    IEnumerator attack(float attack_shell)
    {
        if (Vector3.Distance(this.transform.position, target_po) <= range)
        {
            sbullet.GetComponent<bullet>().damage = damage;
            sbullet.GetComponent<bullet>().proper = proper;
            sbullet.GetComponent<bullet>().master = this.gameObject;
            sbullet.GetComponent<bullet>().shooting = 0.2f;
            Instantiate(sbullet, this.transform.position, this.transform.rotation);
        }
        yield return new WaitForSeconds(attack_shell);
        StartCoroutine(attack(attack_shell));
    }

    void move()
    {
        if (target_po != null)
        {
            this.gameObject.transform.LookAt(target_po);
            if (Vector3.Distance(this.transform.position, target_po) > range)
            {
                this.transform.Translate(new Vector3(0f, 0f, speed));
            }
        }
        //else if (Vector3.Distance(this.transform.position, target.transform.position) < range - 15)
        //{
        //    this.transform.Translate(new Vector3(0f, 0f, -speed));
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("background"))
        {
            Debug.Log("충돌");
            this.transform.Translate(new Vector3(speed, 0f, 0f));
        }
    }
}
