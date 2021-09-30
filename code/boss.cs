using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss : MonoBehaviour
{
    public GameObject En_ad;
    public GameObject En_s;

    public GameObject target;
    public float speed;
    public float range;

    public GameObject bbullet;
    public float attack_speed;

    public int c;


    // Start is called before the first frame update
    void Start()
    {
        c = Random.Range(1, 3);
        target = GameObject.Find("player");
        range = Random.Range(9, 12);

        StartCoroutine(attack());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        target = this.gameObject.GetComponent<enemy>().target;
        attack_speed = this.gameObject.GetComponent<enemy>().attack_speed;
        speed = this.gameObject.GetComponent<enemy>().speed;

        //this.gameObject.transform.LookAt(target.transform);

        move();
    }

    void move()
    {
        transform.RotateAround(new Vector3(0f, 0.5f, 0f), new Vector3(0f,1f,0f), 0.3f);
    }

    IEnumerator attack()
    {
        int p = Random.Range(1, 5);
        if (p == 1) { StartCoroutine(p_1()); }
        else if ( p == 2){ StartCoroutine(p_2()); }
        //else if( p == 3) { StartCoroutine(p_3()); } //소환은 일시 중단
        else if( p == 4) { StartCoroutine(p_4()); }
        yield return new WaitForSeconds(3f);

        StartCoroutine(attack());
    }

    IEnumerator p_1()
    {
        Quaternion asap;
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < 12; i++)
            {
                asap = Quaternion.Euler(this.gameObject.transform.rotation.x, this.gameObject.transform.rotation.y + 30 * i, this.gameObject.transform.rotation.z);
                bbullet.GetComponent<bullet>().damage = 5;
                bbullet.GetComponent<bullet>().master = this.gameObject;
                bbullet.GetComponent<bullet>().proper = "fire";
                bbullet.GetComponent<bullet>().shooting = 0.1f;
                Instantiate(bbullet, this.gameObject.transform.position, asap);
            }
            yield return new WaitForSeconds(0.5f);
        }
        StopCoroutine(p_1());
    }


    IEnumerator p_2()
    {
        target.GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, -20f, ForceMode.Impulse);
        target.GetComponent<player>().stunh += 2;
        yield return new WaitForSeconds(2f);
        StopCoroutine(p_2());
    }

    IEnumerator p_3()
    {
        Debug.Log("패턴 3");
        Quaternion asap;
        int i;
        for (int j = 0; j < 3; j++)
        {
            asap = Quaternion.Euler(this.gameObject.transform.rotation.x, this.gameObject.transform.rotation.y + 60 * j, this.gameObject.transform.rotation.z);
            if (En_ad != null && En_s != null)   //&& Time.time < 8f
            {
                i = Random.Range(1, 3);
                if (i == 1)
                {
                    Instantiate(En_ad, this.gameObject.transform.position, asap);
                    Debug.Log("소환");
                }
                else if (i == 2)
                {
                    Instantiate(En_s, this.gameObject.transform.position, asap);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        StopCoroutine(p_3());
    }

    IEnumerator p_4()
    {
        Quaternion asap;
        for (int j = 0; j < 50; j++)
        {
            for (int i = 0; i < 24; i++)
            {
                transform.Rotate(Vector3.up);
                asap = Quaternion.Euler(this.gameObject.transform.rotation.x, this.gameObject.transform.rotation.y + 15 * i, this.gameObject.transform.rotation.z);

                bbullet.GetComponent<bullet>().damage = 1;
                bbullet.GetComponent<bullet>().proper = "none";
                bbullet.GetComponent<bullet>().master = this.gameObject;
                bbullet.GetComponent<bullet>().shooting = 0.2f;

                Instantiate(bbullet, this.gameObject.transform.position, asap);
            }
            yield return new WaitForSeconds(0.05f);
        }
        StopCoroutine(p_4());
    }
}
