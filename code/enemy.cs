using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : character
{
    public GameObject[] drop = new GameObject[10]; //드롭 아이템
    

    public GameObject target; //타겟팅
    public Vector3 target_po; //직선상 타겟 위치



    public int stunh; //스턴 상태이상
    public bool stun_s = false;

    public int slowth; //슬로우 상태이상
    public bool slowth_s = false;

    public int weth; //젖음 상태이상
    public bool wet_s = false;

    public int fireh; //화상 상태이상
    public bool fire_s = false;


    // Start is called before the first frame update
    void Start()
    {
        proper_list = new string[] { "none", "fire", "water", "dirt", "air"};
        target = GameObject.Find("player");

        scar.pro_d = 0; //초기 받은 데미지 초기화
        scar.pro_w = ""; //초기 받은 데미지 속성 초기화

        maxHP = Random.Range(80, 100); // 최대 HP
        HP = maxHP; //현재 HP

        speed = Random.Range(0.05f, 0.1f); //오브젝트의 이동속도
        attack_speed = Random.Range(2, 3); //오브젝트의 공격속도
        level = Random.Range(1, 10); //오브젝트 레벨 -> 추후 이동속도와 공격속도, HP와 연관.
        strengh = (level + (maxHP * 0.01f) + speed);
        proper = proper_list[Random.Range(0, 5)]; //오브젝트 속성
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        target_po = new Vector3(target.transform.position.x, 0.2f, target.transform.position.z);
        if (scar.pro_d != 0) { hit(scar.pro_d, scar.pro_w); }
        livecheck();
        substance();
    }

    void livecheck() //데미지를 바탕으로 한 체력 체크
    {
        if (HP < 0)
        {
            HP = 0;
            Destroy(this.gameObject);
            Item();
        }
    }

    void Item() //기본_드랍
    {
        int percent;
        //percent = Random.Range(0, 10);
        percent = 0;
        if (drop[percent] != null)
        {
            Instantiate(drop[percent], transform.position, transform.rotation);
        }
    }

    void hit(int d, string w) // 속성별 데미지 처리
    {
        int total_damage = 0;
        scar.pro_d = 0;
        scar.pro_w = "";
        switch(w) //공격들어온 속성에 대해서.
        {
            case "fire":
                switch(proper)//자신의 속성
                {
                    case "fire":
                        total_damage = (int)(d * 0.8f); //동속성 데미지 반감
                        break;
                    case "water":
                        total_damage = (int)(d * 0.5f); //물에게 데미지 반감
                        break;
                    case "dirt":
                        total_damage = d;
                        break;
                    case "air":
                        total_damage = (int)(d * 1.5f); //공기에게 추가 데미지
                        break;
                    default:
                        total_damage = d;
                        break;
                }
                break;

            case "water":
                switch (proper)//자신의 속성
                {
                    case "fire":
                        total_damage = (int)(d * 1.5f); //불에게 추가 데미지
                        break;
                    case "water":
                        total_damage = (int)(d * 0.8f); //동속성 데미지 반감
                        break;
                    case "dirt": 
                        total_damage = (int)(d * 0.5f); //흙에게 데미지 반감
                        break;
                    case "air":
                        total_damage = d; 
                        break;
                    default:
                        total_damage = d;
                        break;
                }
                break;

            case "dirt":
                switch (proper)//자신의 속성
                {
                    case "fire":
                        total_damage = d;
                        break;
                    case "water":
                        total_damage = (int)(d * 1.5f); //물에게 추가 데미지
                        break;
                    case "dirt":
                        total_damage = (int)(d * 0.8f); //동속성 데미지 반감
                        break;
                    case "air":
                        total_damage = (int)(d * 0.5f); //공기에게 데미지 반감
                        break;
                    default:
                        total_damage = d;
                        break;
                }
                break;

            case "air":
                switch (proper)//자신의 속성
                {
                    case "fire":
                        total_damage = (int)(d * 0.5f); //불에게 데미지 반감
                        break;
                    case "water":
                        total_damage = d;
                        break;
                    case "dirt":
                        total_damage = (int)(d * 1.5f); //흙에게 추가 데미지
                        break;
                    case "air":
                        total_damage = (int)(d * 0.8f); //동속성 데미지 반감
                        break;
                    default:
                        total_damage = d;
                        break;
                }
                break;

            default:
                total_damage = d;
                break;

        }
        deal(total_damage);
    }

    public void substance() // 상태이상 체크
    {
        if(fireh > 0 && fire_s == false)
        {
            fire_s = true;
            StartCoroutine(fire());
        }
        if(slowth > 0 && slowth_s == false)
        {
            slowth_s = true;
            StartCoroutine(slow());
        }
        if(weth > 0 && wet_s == false)
        {
            wet_s = true;
            StartCoroutine(wet());
        }
        if(stunh > 0 && stun_s == false)
        {
            stun_s = true;
            StartCoroutine(stun());
        }
    }
    IEnumerator stun()
    {
        float or_speed;
        or_speed = speed;
        speed = 0;
        while(stunh != 0)
        {
            yield return new WaitForSeconds(1f);
            stunh--;
        }
        speed = or_speed;
        stun_s = false;
        yield return null;
    }

    IEnumerator fire()
    {
        while (fireh != 0)
        {
            if (wet_s) { fireh = 0; break; }
            deal(1);
            yield return new WaitForSeconds(0.2f);
            fireh--;
        }
        fire_s = false;
        yield return null;
    }

    IEnumerator slow()
    {
        speed -= 0.03f;
        while(slowth != 0)
        {
            yield return new WaitForSeconds(1f);
            slowth--;
        }
        slowth_s = false;
        speed += 0.03f;
        yield return null;
    }

    IEnumerator wet()
    {
        attack_speed -= 0.1f;
        while(weth != 0)
        {
            yield return new WaitForSeconds(1f);
            weth--;
        }
        wet_s = false;
        attack_speed += 0.1f;
        yield return null;
    }

    void deal(int indi) //체력 감소 처리
    {
        HP -= indi;
        return;
    }

}
