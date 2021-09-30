using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapeon_part : MonoBehaviour
{
    //0: 보석, 1: 칼, 2: 손잡이, 3: 검촉

    public status wp;

    public int i; // 부위
    public int grade; //등급

    public GameObject sworrd; //게임 오브젝트: 검.
    
    // Start is called before the first frame update
    void Start()
    {
        sworrd = GameObject.Find("sword");
        i = Random.Range(0, 4);
        grade = Random.Range(1, 5);
        stat();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && sworrd != null)
        {
            get_item();
        }
    }

    void get_item()
    {
        for (int j = 0; j < 5; j++)
        {
            if (sworrd.GetComponent<swordcode>().inform[i, j].name == "")
            {
                sworrd.GetComponent<swordcode>().inform[i, j] = wp;
                Destroy(this.gameObject);
                return;
            }
        }

        Debug.Log("인벤토리가 가득 찼습니다.");
    }
    void stat()
    {
        
    }
}
