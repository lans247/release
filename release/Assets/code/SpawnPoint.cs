using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject characterPrefab;
    public float repeatInterval;
    // Start is called before the first frame update
    void Start()
    {
       if(repeatInterval>0.0f)
        {
            InvokeRepeating("SpawnObject",0.0f, repeatInterval);
        }
    }

    // Update is called once per frame
    public GameObject SpawnObject()
    {
        if(characterPrefab != null)
        {
            return Instantiate(characterPrefab, transform.position, Quaternion.identity);
        }

        return null;
    }
}
