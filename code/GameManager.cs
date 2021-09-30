using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject En_ad;
    public GameObject En_t;
    public GameObject En_s;

    public int i;
    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("make", 0, 1f);
    }

    // Update is called once per frame
    void make()
    {
        if (En_t != null && En_ad != null && En_s != null)   //&& Time.time < 8f
        {
            i = Random.Range(1, 4);
            if (i == 1)
            {
                Instantiate(En_t, new Vector3(Random.Range(-20, 20), 0.5f, Random.Range(-20, 20)), Quaternion.Euler(0f, 0f, 0f));
            }
            else if(i==2)
            {
                Instantiate(En_ad, new Vector3(Random.Range(-20, 20), 0.5f, Random.Range(-20, 20)), Quaternion.Euler(0f, 0f, 0f));
            }
            else
            {
                Instantiate(En_s, new Vector3(Random.Range(-20, 20), 0.5f, Random.Range(-20, 20)), Quaternion.Euler(0f, 0f, 0f));
            }
            
        }
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(10, 700, 50, 50), "소환")) { make(); }
    }
}
