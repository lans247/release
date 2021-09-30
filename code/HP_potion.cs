using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_potion : MonoBehaviour
{
    public GameObject target;
    
    void Start()
    {
        target = GameObject.Find("player");
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            target.GetComponent<player>().HP += 10;
            Destroy(this.gameObject);
        }
    }

}
