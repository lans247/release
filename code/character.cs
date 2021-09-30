using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct proper
{
    public string pro_w;
    public int pro_d;
}   //받은 속성 데미지를 반영

public class character : MonoBehaviour
{
    public string[] proper_list = { "none", "fire", "water", "dirt", "air" }; //속성
    public proper scar; //받은 데미지. 속성과 데미지
    public float range; //사거리
    public int maxHP; //최대체력
    public int HP; //현재 체력
    public float speed; //이동속도
    public string cti; // 성격
    public string proper; // 속성
    public int level; // 레벨
    public float exp; // 경험치
    public float strengh; // 총 강함
    public float attack_speed; //공격속도
    public int damage; //공격력
    public int evasion; //회피율
}
