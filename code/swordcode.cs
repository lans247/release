using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct status
{
    public int rarity; //등급
    public string name; //이름
    public string proper; //속성
    public int damage; //공격력
    public float attack_speed; //공격속도
    public float speed; //이동속도
    public float range; //사거리
    public long code; // [스킬(103)][레벨(1)][쿨타임(030)][영구사용(0,1)]
    public int rebel; //반항수치
    public int evasion; //회피율
}

public class swordcode : MonoBehaviour
{
    public int[] cooltime = new int[] { 0, 0, 0, 0 }; //스킬당 개별 쿨타임

    public status[,] inform = new status[4, 5]; // inventory에 있는 아이템들. 0: 보석, 1: 칼, 2: 손잡이, 3: 검집

    public status[] eq = new status[4]; //장착한 아이템들. 0: 보석, 1: 칼, 2: 손잡이, 3: 검집

    public status no;

    public GameObject user;

    public float s_range; //검자체의 스탯
    public float s_speed;
    public float s_attack_speed;
    public int s_damage;
    public int s_evation;

    public float ex_range; //스킬등에 의한 추가 스탯
    public float ex_speed;
    public float ex_attack_speed;
    public int ex_damage;
    public int ex_evation;

    // Start is called before the first frame update
    void Start()
    {
        cooltime[0] = 0; cooltime[1] = 0; //쿨타임 초기화
        cooltime[2] = 0; cooltime[3] = 0;

        no.name = ""; no.proper = ""; //공백
        no.speed = 0; no.damage = 0; no.code = 0000000000;
        no.attack_speed = 0; no.range = 0; no.evasion = 0;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                inform[i, j] = no;
            }
        }
        for (int j = 0; j < 4; j++) { eq[j] = no; } //착용칸 초기화


        user = GameObject.Find("player");

        inform[0, 0].name = "berserk";
        inform[0, 0].code = 00130301;

        inform[0, 1].name = "onhit";
        inform[0, 1].code = 00840050;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        user.GetComponent<player>().s_range = s_range;
        user.GetComponent<player>().s_speed = s_speed;
        user.GetComponent<player>().s_damage = s_damage;
        user.GetComponent<player>().s_attack_speed = s_attack_speed;
        user.GetComponent<player>().s_evasion = s_evation;

        user.GetComponent<player>().ex_range = ex_range;
        user.GetComponent<player>().ex_speed = ex_speed;
        user.GetComponent<player>().ex_damage = ex_damage;
        user.GetComponent<player>().ex_attack_speed = ex_attack_speed;
        user.GetComponent<player>().ex_evasion = ex_evation;
    }

    private void OnGUI() //0: 보석, 1: 검날, 2: 손잡이, 3: 검집
    {
        for (int i = 0; i < 5; i++)
        {
            if (GUI.Button(new Rect(Screen.width - 40, (40 * i), 40, 40), inform[0, i].name)) { if (inform[0, i].name != "") { swipe(0, i); } }
        }
        for (int i = 0; i < 5; i++)
        {
            if (GUI.Button(new Rect(Screen.width - 80, (40 * i), 40, 40), inform[1, i].name)) { if (inform[1, i].name != "") { swipe(1, i); } }
        }
        for (int i = 0; i < 5; i++)
        {
            if (GUI.Button(new Rect(Screen.width - 120, (40 * i), 40, 40), inform[2, i].name)) { if (inform[2, i].name != "") { swipe(2, i); } }
        }
        for (int i = 0; i < 5; i++)
        {
            if (GUI.Button(new Rect(Screen.width - 160, (40 * i), 40, 40), inform[3, i].name)) { if (inform[3, i].name != "") { swipe(3, i); } }
        }

        GUI.Box(new Rect(Screen.width - 40, 200, 40, 40), eq[0].name);
        GUI.Box(new Rect(Screen.width - 80, 200, 40, 40), eq[1].name);
        GUI.Box(new Rect(Screen.width - 120, 200, 40, 40), eq[2].name);
        GUI.Box(new Rect(Screen.width - 160, 200, 40, 40), eq[3].name);

        GUI.Box(new Rect(Screen.width - 40, 600, 40, 40), cooltime[0].ToString());
        GUI.Box(new Rect(Screen.width - 80, 600, 40, 40), cooltime[1].ToString());
        GUI.Box(new Rect(Screen.width - 120, 600, 40, 40), cooltime[2].ToString());
        GUI.Box(new Rect(Screen.width - 160, 600, 40, 40), cooltime[3].ToString());
    }



    void proper_test() //속성체크, 속성 아이템이 2개 넘어가야 해당 속성 인정
    {
        int[] pro = new int[] { 0, 0, 0, 0 };
        string t = "none";

        for (int z = 0; z < 4; z++)
        {
            if (eq[z].proper == "air") { pro[0]++; }
            if (eq[z].proper == "fire") { pro[1]++; }
            if (eq[z].proper == "dirt") { pro[2]++; }
            if (eq[z].proper == "water") { pro[3]++; }
        }
        int M = Mathf.Max(pro);
        for (int z = 0; z < 4; z++)
        {
            if (M == pro[z])
            {
                if (z == 0) { t = "air"; }
                if (z == 1) { t = "fire"; }
                if (z == 2) { t = "dirt"; }
                if (z == 3) { t = "water"; }
            }
        }
        if (M > 2) { user.GetComponent<player>().proper = t; }
        else { user.GetComponent<player>().proper = "none"; }
    }



    IEnumerator s_use(int p, long code) //p 스킬을 발동한 파츠의 위치, 스킬 사용시 발동하는 함수, 부위당 한번에 한개만 가능.
    {
        int skill; bool o_use; int lv; int Cool; long C;

        C = code;
        skill = (int)(C / 100000);

        C = C % 100000;
        lv = (int)(C / 10000);

        C = C % 10000;
        Cool = (int)(C / 10);

        C = C % 10;
        if (C == 1) { o_use = true; }
        else { o_use = false; }

        if (cooltime[p] == 0) //쿨타임 적용 중에는 불가
        {
            cooltime[p] = Cool;
            //해당 스킬의 시전이 끝나면, 쿨타임이 돌기 시작함.
            if (skill == 0) { StopCoroutine(s_use(p, code));}
            else if (skill == 1) { yield return StartCoroutine(berserk(lv)); }
            else if (skill == 2) { nukbuk(lv); }
            else if (skill == 3) { dash(lv); }
            else if (skill == 4) { airborn(lv); }
            else if (skill == 5) { yield return StartCoroutine(undie(lv)); }
            else if (skill == 6) { yield return StartCoroutine(parrying(lv)); }
            else if (skill == 7) { telep(lv); }
            else if (skill == 8) { yield return StartCoroutine(onhit_d(p, lv)); }
            else if (skill == 9) { blow(); }
            else if (skill == 10) { yield return StartCoroutine(sucking(p, lv)); }

            //1회용인 경우, 사용후 바로 종료.
            if (o_use == true) { eq[p] = no; StopCoroutine(s_use(p, code)); }

            //1초씩 쿨타임이 감소
            while (cooltime[p] != 0)
            {
                yield return new WaitForSeconds(1f);
                cooltime[p]--;
            }

            //쿨타임 종료시, 현재 착용 부위의 스킬을 발동.
            StartCoroutine(s_use(p, eq[p].code));
        }
    }


    IEnumerator berserk(int lv) //1번 스킬, 버서커, 1~4 레벨(스탯 차이)
    {
        if (lv == 1)
        {
            ex_speed += 0.4f;
            ex_attack_speed += 0.3f;
            ex_damage += 10;
            yield return new WaitForSeconds(5f);
            ex_speed -= 0.4f;
            ex_attack_speed -= 0.3f;
            ex_damage -= 10;
        }
        else if (lv == 2)
        {
            ex_speed += 0.5f;
            ex_attack_speed += 0.4f;
            ex_damage += 15;
            yield return new WaitForSeconds(5f);
            ex_speed -= 0.5f;
            ex_attack_speed -= 0.4f;
            ex_damage -= 15;
        }
        else if (lv == 3)
        {
            ex_speed += 0.6f;
            ex_attack_speed += 0.5f;
            ex_damage += 20;
            yield return new WaitForSeconds(5f);
            ex_speed -= 0.6f;
            ex_attack_speed -= 0.5f;
            ex_damage -= 20;
        }
        else if(lv == 4)
        {
            ex_speed += 0.8f;
            ex_attack_speed += 0.6f;
            ex_damage += 25;
            yield return new WaitForSeconds(5f);
            ex_speed -= 0.8f;
            ex_attack_speed -= 0.6f;
            ex_damage -= 25;
        }
    }



    void nukbuk(int lv) //2번 스킬, 넉백, 1~2 레벨(거리와 스턴 시간)
    {
        GameObject[] all;
        all = GameObject.FindGameObjectsWithTag("enemy");

        for (int j = 0; j < all.Length; j++)
        {
            if (Vector3.Distance(user.transform.position, all[j].transform.position) < 5f)
            {
                if (lv == 1)
                {
                    all[j].GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, -10f, ForceMode.Impulse);
                    all[j].GetComponent<enemy>().stunh += 1;
                }
                else if (lv == 2)
                {
                    all[j].GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, -25f, ForceMode.Impulse);
                    all[j].GetComponent<enemy>().stunh += 3;
                }

            }
        }

    }



    void dash(int lv) //3번 스킬, 대쉬, 1레벨
    {
        user.transform.Translate(0f, 0f, 8f);
    }



    void airborn(int lv) // 4번 스킬, 에어본, 1~3 레벨(스턴과 높이 달라짐)
    {
        GameObject[] all;
        all = GameObject.FindGameObjectsWithTag("enemy");

        for (int j = 0; j < all.Length; j++)
        {
            if (lv == 1)
            {
                all[j].GetComponent<Rigidbody>().AddRelativeForce(0f, 2f, 0f, ForceMode.Impulse);
                all[j].GetComponent<enemy>().stunh += 1;
            }
            else if (lv == 2)
            {
                all[j].GetComponent<Rigidbody>().AddRelativeForce(0f, 4f, 0f, ForceMode.Impulse);
                all[j].GetComponent<enemy>().stunh += 2;
            }
            else if (lv == 3)
            {
                all[j].GetComponent<Rigidbody>().AddRelativeForce(0f, 8f, 0f, ForceMode.Impulse);
                all[j].GetComponent<enemy>().stunh += 3;
            }
        }
    }



    IEnumerator undie(int lv) //5번 스킬, 무적, 1~5레벨(1~5초)
    {
        int or_h = user.GetComponent<player>().HP;
        int or_s = user.GetComponent<player>().shield;

        for (int eq = 0; eq < lv * 100; eq++)
        {
            user.GetComponent<player>().HP = or_h;
            user.GetComponent<player>().shield = or_s;
            yield return new WaitForSeconds(0.01f);
        }

    }



    IEnumerator parrying(int lv) //6번 스킬, 패링, 1~5레벨(1~5초)
    {
        user.GetComponent<player>().parrying = true;
        user.GetComponent<player>().stunh += lv;
        yield return new WaitForSeconds(lv);
        user.GetComponent<player>().parrying = false;

    }

    void telep(int lv) //7번 스킬, 1레벨. 적에게 위치 이동
    {
        GameObject target = user.GetComponent<player>().target;
        user.transform.position = new Vector3(target.transform.position.x, 0.5f, target.transform.position.z) + (target.transform.position - user.transform.position).normalized;
    }

    IEnumerator onhit_d(int p, int lv) //8번 스킬, 온힛 데미지. 1~5레벨(공격력)
    {
        int c = 30;
        int plus_damage = (int)(user.GetComponent<player>().damage * 0.1f * lv); //10~40% 추가뎀

        user.gameObject.GetComponent<player>().onhit[p] = true;
        ex_damage += plus_damage;
        while (true)
        {
            if (c < 0 || user.GetComponent<player>().onhit[p] == false) { break; } //3초동안 온힛 상태 유지
            yield return new WaitForSeconds(0.1f);
            c--;
        }
        user.GetComponent<player>().onhit[p] = false; //온힛 꺼짐
        ex_damage -= plus_damage;
    }

    void blow() //9번 스킬, 1명의 적을 밀쳐냄, 1레벨
    {
        user.GetComponent<player>().target.GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, -20f, ForceMode.Impulse);
    }

   
    IEnumerator sucking(int p, int lv) //10번 스킬, 온힛 기반 흡혈, 1~5레벨
    {
        int c = 30;
        user.gameObject.GetComponent<player>().onhit[p] = true;
        while (true)
        {
            if (c < 0 || user.GetComponent<player>().onhit[p] == false) { break; } //3초동안 온힛 상태 유지
            yield return new WaitForSeconds(0.1f);
            c--;
        }
        user.GetComponent<player>().onhit[p] = false; //온힛 꺼짐
        if (c >= 0) { user.GetComponent<player>().HP += (int)(user.GetComponent<player>().damage * 0.02f * lv); } //타격 성공시 가한 데미지 2~10%만큼 체력 회복

    }











    void purification() //정화
    {

    }






    void swipe(int p, int E) // 0: 보석, 1: 칼, 2: 손잡이, 3: 검촉  , 검 부위가 스왑될떄만 발동
    {
        status ex;

        ex = inform[p, E];
        inform[p, E] = eq[p];
        eq[p] = ex;


        s_range = eq[0].range + eq[1].range + eq[2].range + eq[3].range;
        s_speed = eq[0].speed + eq[1].speed + eq[2].speed + eq[3].speed;
        s_damage = eq[0].damage + eq[1].damage + eq[2].damage + eq[3].damage;
        s_attack_speed = eq[0].attack_speed + eq[1].attack_speed + eq[2].attack_speed + eq[3].attack_speed;


        StartCoroutine(s_use(p, eq[p].code)); //실제 게임에서는 P가 0일때만 실행될 것.
        proper_test(); //속성 검사

    }


}
