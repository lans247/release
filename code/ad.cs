using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ad : MonoBehaviour
{

    public GameObject target;
    public Vector3 target_po; 

    public float speed;
    public float range;
    public string proper; //속성

    public GameObject abullet;
    public float attack_speed;

    public int damage = 1; //총알 데미지

    public int c;

    // Start is called before the first frame update
    void Start()
    {
        c = Random.Range(1, 3);
        target = GameObject.Find("player");
        range = Random.Range(9, 12);

        proper = this.gameObject.GetComponent<enemy>().proper;
        StartCoroutine(attack(5f-attack_speed));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        proper = this.gameObject.GetComponent<enemy>().proper;


        target = this.gameObject.GetComponent<enemy>().target;
        target_po = this.gameObject.GetComponent<enemy>().target_po; //타겟위치
        attack_speed = this.gameObject.GetComponent<enemy>().attack_speed; 
        speed = this.gameObject.GetComponent<enemy>().speed;
        move();
    }

    void move()
    {

        if (target_po != null)
        {
            this.transform.LookAt(target_po);
            if (Vector3.Distance(this.gameObject.transform.position, target_po) > range)
            {
                this.transform.Translate(new Vector3(0f, 0f, speed));
            }
        }
        //else if (Vector3.Distance(this.transform.position, target.transform.position) < range - 2)
        //{
        //    if (c == 1) { this.transform.Translate(new Vector3(speed, 0f, -speed + 0.02f)); }
        //    else { this.transform.Translate(new Vector3(-speed, 0f, -speed + 0.02f)); }
        //}
        else
        {
            if (c == 1) { this.transform.Translate(new Vector3(speed, 0f, 0f)); }
            else { this.transform.Translate(new Vector3(-speed, 0f, 0f)); }
        }
    }

    IEnumerator attack(float attack_shell)
    {
        if (Vector3.Distance(this.gameObject.transform.position, target_po) <= range)
        {
            for (int i = 0; i < 4; i++)
            {
                abullet.GetComponent<bullet>().damage = damage;
                abullet.GetComponent<bullet>().proper = proper;
                abullet.GetComponent<bullet>().master = this.gameObject;
                abullet.GetComponent<bullet>().shooting = 0.1f;
                Instantiate(abullet, this.gameObject.transform.position, this.gameObject.transform.rotation);
                yield return new WaitForSeconds(0.4f);
            }
        }
        yield return new WaitForSeconds(attack_shell);
        c = Random.Range(1, 3);
        StartCoroutine(attack(5f - attack_speed));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("background"))
        {
            if (c == 1) { c = 2; }
            else { c = 1; }
        }
    }
}

