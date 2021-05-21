using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public int bullets;
    // Start is called before the first frame update
    void Start()
    {
        bullets = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("BulletBundle"))
        {
            bullets += 5;
            Debug.Log("you have" + bullets + "bullets");
            collision.gameObject.SetActive(false);
        }
    }
}
