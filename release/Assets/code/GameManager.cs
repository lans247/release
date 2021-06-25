using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance = null;
    public SpawnPoint playerSpawnPoint;
    public SpawnPoint enemySpawnPoint;
    public CameraManager cameraManager;
    // Start is called before the first frame update
    private void Awake()
    {
        if(sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }
    }

    void Start()
    {
        SetupScene();
    }
    public void SetupScene()
    {
        SpawnPlayer();
        Spawnenemy();
    }
    public void SpawnPlayer()
    {
        if(playerSpawnPoint != null)
        {
            GameObject player = playerSpawnPoint.SpawnObject();
            cameraManager.virtualCamera.Follow = player.transform;
        }
    }
    public void Spawnenemy()
    {
        if (enemySpawnPoint != null)
        {
            GameObject enemy = enemySpawnPoint.SpawnObject();
        }
    }

    // Update is called once per frame
}
