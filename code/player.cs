using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : character
{
    public bool revel; //계시 상태가 진행중인가
    public int revel_stat; // 계시 스탯

    public int shield; //쉴드
    public int shield_max; //쉴드 최대치
    public float shield_gen = 1; //쉴드 회복 속도 작을 수록 빠름

    public float HP_gen = 10; //체력 회복 속도

    public int at_check; //비전투 상태
    public int bt_check; //비피해 상태

    public int rebel; //반항 수치

    public bool[] onhit = new bool[4] { false, false, false, false }; //온힛 발동중

    public float p_range = 2.0f; //플레이어의 능력치
    public float p_speed = 0.1f;
    public float p_attack_speed = 4f;
    public int p_damage = 10;
    public int p_evasion = 0;

    public float s_range; //검에 의한 추가 능력치
    public float s_speed;
    public float s_attack_speed;
    public int s_damage;
    public int s_evasion;

    public float ex_range; //스킬, 상태이상에 대한 변화 능력치
    public float ex_speed;
    public float ex_attack_speed;
    public int ex_damage;
    public int ex_evasion;

    public bool parrying = false; // 패링 스킬 추가, 현재 수정 고안중

    public GameObject target;
    public Camera main_camera;

    public int buff;

    public int stunh; //스턴 상태이상
    public bool stun_s = false;

    public Vector3 movep;

    // Start is called before the first frame update
    void Start()
    {

        this.gameObject.transform.position = new Vector3(0f, 0.5f, 0f);
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

        proper = "none";
        buff = 0;


        maxHP = 100;
        HP = maxHP;
        shield = 0;

        cti = "n";
        level = 1;
        exp = 0;

        StartCoroutine(regeneration());
        StartCoroutine(attack(6.0f - attack_speed)); //공격
        StartCoroutine(revelation()); //계시 상태
        StartCoroutine(check_s()); //공격&휴식 상태 체크

    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        range = p_range + s_range + ex_range;
        if (!stun_s) { speed = p_speed + s_speed + ex_speed; }
        evasion = p_evasion + s_evasion + ex_evasion;
        damage = p_damage + s_damage + ex_damage;
        attack_speed = p_attack_speed + s_attack_speed + ex_attack_speed;

        Move();

        if (Input.GetMouseButton(1)) { ClickMove(); } //움직임, 적변경
        if (Input.GetMouseButton(2) && revel_stat > 1) { revel_stat -= 1; stunh+=4; } //정지

        if (target == null) { findtarget(); } //적 찾기

        if (scar.pro_d != 0) { hit(scar.pro_d, scar.pro_w); } //데미지 감지

        if (proper == "air" && buff == 5) { StartCoroutine(air_buff()); buff = -1; } //풍속성일때 5중첩이 쌓이면 한번발동

        substance();// 상태이상 체크
    }

    void Move() //움직임
    {
        if (target && !revel)
        {
            movep = new Vector3(target.transform.position.x, 0.2f, target.transform.position.z);
            this.gameObject.transform.LookAt(movep);

            if (Vector3.Distance(this.gameObject.transform.position, movep) >= range)
            {
                this.transform.Translate(new Vector3(0f, 0f, speed));
            }
            else if (Vector3.Distance(this.gameObject.transform.position, movep) < 0.1f)
            {
                revel = false;
                this.transform.Translate(new Vector3(0f, 0f, -speed));
            }
        }
        else if (revel)
        {
            transform.position = Vector3.MoveTowards(transform.position, movep, speed);
            if (Vector3.Distance(this.gameObject.transform.position, movep) < 0.1f) { revel = false; }
        }
    }

    IEnumerator stun()//스턴
    {
        speed = 0;
        while (stunh != 0)
        {
            yield return new WaitForSeconds(1f);
            stunh--;
        }
        stun_s = false;
        yield return null;
    }

    private void findtarget() //타겟지정
    {
        int c = 0;
        GameObject[] many;
        many = GameObject.FindGameObjectsWithTag("enemy");

        if (many.Length == 0)
        {
            target = null;
            return;
        }

        float mo = 0.0f;

        if (cti == "brave") { mo = 100f; }
        if (cti == "near") { mo = 100f; }
        if (cti == "long") { mo = 0f; }

        for (int i = 0; i < many.Length; i++)
        {

            if (cti == "brave")
            {
                if (mo >= many[i].GetComponent<enemy>().strengh) //작은거
                {
                    mo = many[i].GetComponent<enemy>().strengh;
                    c = i;
                }
            }

            else if (cti == "cowad")
            {
                if (mo <= many[i].GetComponent<enemy>().strengh) //큰거
                {
                    mo = many[i].GetComponent<enemy>().strengh;
                    c = i;
                }
            }

            else if (cti == "near")
            {
                if (mo >= Vector3.Distance(this.gameObject.transform.position, many[i].transform.position)) //가까운 것
                {
                    mo = Vector3.Distance(this.gameObject.transform.position, many[i].transform.position);
                    c = i;
                }
            }
            else if (cti == "long")
            {
                if (mo <= Vector3.Distance(this.gameObject.transform.position, many[i].transform.position)) //가까운 것
                {
                    mo = Vector3.Distance(this.gameObject.transform.position, many[i].transform.position);
                    c = i;
                }
            }

        }
        target = many[c];
    }


    IEnumerator attack(float delay_t) //공격
    {
        //findtarget();

        if (target)
        {
            if (Vector3.Distance(target.transform.position, this.transform.position) <= range)
            {
                int zq = 0;
                at_check = 0; //공격으로 인한 상태 해제
                target.GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, -10f, ForceMode.Impulse);
                this.gameObject.GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, -5f, ForceMode.Impulse);
                target.GetComponent<enemy>().scar.pro_d = damage;
                target.GetComponent<enemy>().scar.pro_w = proper;
                while (zq != 4) { onhit[zq] = false; zq++; } //온힛꺼짐
                if (proper == "air" && buff != -1) { buff++; } // 풍속성 스택 증가
            }
        }
        yield return new WaitForSeconds(delay_t);
        StartCoroutine(attack(6.0f - attack_speed));

    }



    private void OnGUI() //편의 UI
    {
        if (GUI.Button(new Rect(10, 10, 60, 40), "겁쟁이")) { cti = "brave"; findtarget(); }
        if (GUI.Button(new Rect(70, 10, 60, 40), "귀찮음")) { cti = "near"; findtarget(); }
        if (GUI.Button(new Rect(10, 50, 60, 40), "용감한")) { cti = "cowad"; findtarget(); }
        if (GUI.Button(new Rect(70, 50, 60, 40), "원거리 혐오")) { cti = "long"; findtarget(); }

        GUI.Box(new Rect(10, 110, 60, 40), shield.ToString());
        GUI.Box(new Rect(70, 110, 60, 40), HP.ToString());

        if (target != null) { GUI.Box(new Rect(10, 170, 100, 50), target.name + "\n" + target.GetComponent<enemy>().HP + "\n" + target.GetComponent<enemy>().proper); }
        else { GUI.Box(new Rect(10, 170, 100, 50), "탐색중"); }

        GUI.Box(new Rect(10, 220, 100, 50), "damage" + "\n" + damage.ToString());
        if (GUI.Button(new Rect(110, 220, 40, 50), "↑")) { p_damage++; }
        if (GUI.Button(new Rect(150, 220, 40, 50), "↓")) { if (p_damage > 0) { p_damage--; } }

        GUI.Box(new Rect(10, 270, 100, 50), "attack_speed" + "\n" + attack_speed.ToString());
        if (GUI.Button(new Rect(110, 270, 40, 50), "↑")) { if (p_attack_speed < 6) { p_attack_speed += 0.1f; } }
        if (GUI.Button(new Rect(150, 270, 40, 50), "↓")) { if (p_attack_speed > 0) { p_attack_speed -= 0.1f; } }

        GUI.Box(new Rect(10, 320, 100, 50), "speed" + "\n" + speed.ToString());
        if (GUI.Button(new Rect(110, 320, 40, 50), "↑")) { p_speed += 0.1f; }
        if (GUI.Button(new Rect(150, 320, 40, 50), "↓")) { if (p_speed > 0) { p_speed -= 0.1f; } }

        if (GUI.Button(new Rect(10, 420, 50, 50), "무속성")) { proper = "none"; }
        if (GUI.Button(new Rect(10, 470, 50, 50), "화속성")) { proper = "fire"; }
        if (GUI.Button(new Rect(10, 520, 50, 50), "수속성")) { proper = "water"; }
        if (GUI.Button(new Rect(10, 570, 50, 50), "풍속성")) { proper = "air"; }
        if (GUI.Button(new Rect(10, 620, 50, 50), "토속성")) { proper = "dirt"; }

        GUI.Box(new Rect(10, 670, 50, 30), "계시" + revel_stat.ToString());

    }

    void ClickMove() //위치가 클릭되었을때.
    {
        Vector3 desti = new Vector3(0f, 0f, 0f);
        Ray ray = main_camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000f)) { desti = new Vector3(hit.point.x, 0.5f, hit.point.z); }
        if (hit.collider.CompareTag("enemy") && revel_stat >= 3) { revel_stat -= 3; target = hit.collider.gameObject; }
        if (hit.collider.CompareTag("floor") && revel_stat >= 2) { revel_stat -= 2; movep = desti; revel = true; }
    }

    IEnumerator revelation() //시간에 따른 계시 스탯에 대한 회복
    {
        if (revel_stat <= 5) { revel_stat += 1; }
        yield return new WaitForSeconds(2f);
        StartCoroutine(revelation());
    }

    IEnumerator shielding() //시간에 따른 쉴드 회복
    {
        for (int i = 0; ; i++)
        {
            if (bt_check < 10 || at_check < 10 || shield >= shield_max) { break; } //비전투, 피해 상태 해제시 쉴드 충전 초기화
            shield += 1;
            yield return new WaitForSeconds(shield_gen); //쉴드 회복 속도 만큼 회복. 작을 수록 빨리 회복. 최대 0.1
        }
        yield return null;
    }

    IEnumerator regeneration() //시간에 따른 회복
    {
        for (int i = 0; ; i++)
        {
            if (HP < maxHP) { HP += 1; }
            yield return new WaitForSeconds(HP_gen); //회복 속도 만큼 회복. 작을 수록 빨리 회복. 최대 0.1
        }
    }

    void substance() //상태이상 처리 함수
    {
        if (stunh > 0 && stun_s == false)
        {
            stun_s = true;
            StartCoroutine(stun());
        }
    }

    IEnumerator check_s() //비전투 상태 돌입 함수
    {
        for (int i = 0; ; i++)
        {
            if(at_check >= 10 && bt_check >= 10) { StartCoroutine(shielding()); }
            at_check += 1;
            bt_check += 1;
            yield return new WaitForSeconds(1f);
        }
    }

    void hit(int d, string w) // 속성별 데미지 처리
    {
        scar.pro_d = 0;
        scar.pro_w = "";
        if (w == "none")
        {
            deal(d);
        }
        else if(w == "fire")
        {
            StartCoroutine(fire(d));
        }
    }

    IEnumerator fire(int d) //화염 틱 데미지
    {
        for(int Qa = 0; Qa < d; Qa++)
        {
            deal(1);
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    void deal(int indi) //체력 감소 처리
    {
        bt_check = 0; //공격을 받았는지 체크
        if (shield >= indi)
        {
            shield -= indi;
        }
        else if(shield < indi)
        {
            indi -= shield;
            shield = 0;
            HP -= indi;
        }
        return;
    }

     IEnumerator air_buff() //풍속성 5중첩 발동
    {
        ex_attack_speed += 0.5f;
        ex_speed += 0.25f;

        yield return new WaitForSeconds(5f);

        ex_attack_speed -= 0.5f;
        ex_speed -= 0.25f;

        buff = 0;
    }
}

