using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    public int bullets;
    public Image hpImage;
    public Text hpText;
    
    
    // Start is called before the first frame update
    void Start()
    {
        bullets = 0;
        hpImage = GameObject.Find("HP").GetComponent<Image>();
        hpText = GameObject.Find("HPText").GetComponent<Text>();
    }
    void FixedUpdate()
    {
        setHealthBar();
    }
    // Update is called once per frame

    void Update()
    {
        
    }
    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("BulletBundle"))
        {
            bullets += 5;
            Debug.Log("you have" + bullets + "bullets");
            collision.gameObject.SetActive(false);
        }
    }
    void setHealthBar()
    {
        hpImage.fillAmount = (float)hitPoints / maxHitPoints;
        hpText.text = "HP:" + (hpImage.fillAmount * maxHitPoints);
    }
}
