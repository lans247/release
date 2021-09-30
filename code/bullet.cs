using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{

    public string proper;

    public int damage; //총알 데미지
    public GameObject master; //총알의 주인, 발사한 사람.
    public float shooting; // 총알 이동 속도, 보통 0.2f;

    public string m_tag ; //총알의 적 아군 구분.

    // Start is called before the first frame update

    void Start()
    {
        m_tag = master.tag;
        //proper = master.GetComponent<enemy>().proper;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Translate(0.0f, 0f, shooting);

        if (damage >= 2) { this.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f); }
        else if(damage >= 10) { this.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); }

        if (this.gameObject.transform.position.y < -10 || this.gameObject.transform.position.y > 15) { Destroy(this.gameObject); }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(m_tag == "enemy" && other.tag == "Player")
        {
            if (other.gameObject.GetComponent<player>().parrying == true) //플레이어가 패링 활성화 중
            {
                damage *= 2;
                shooting = -0.2f;
                master = other.gameObject;
                m_tag = master.tag;
            }
            else
            {
                if (Random.Range(0, 100) < other.gameObject.GetComponent<player>().evasion) { Destroy(this.gameObject); } //0~100 가 발동할 경우, 피해 없음
                else
                { 
                    other.gameObject.GetComponent<player>().scar.pro_d = damage;
                    other.gameObject.GetComponent<player>().scar.pro_w = proper;
                    Destroy(this.gameObject);
                }
            }
        }
        else if(m_tag == "Player" && other.gameObject.tag == "enemy")
        {
            other.gameObject.GetComponent<enemy>().scar.pro_d = damage;
            other.gameObject.GetComponent<enemy>().scar.pro_w = proper;
            Destroy(this.gameObject);
        }
        else if(other.CompareTag("background"))
        {
            Destroy(this.gameObject);
        }
        
    }


}
