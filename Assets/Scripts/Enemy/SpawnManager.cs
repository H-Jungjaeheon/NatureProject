using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Enemy;

    private float nowSpawnCount;
    public float maxSpawnCount;
    private bool isSpawn;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EnemySpawn();
    }
    private void EnemySpawn()
    {
        if (isSpawn == false)
        {
            nowSpawnCount += Time.deltaTime;
            if (nowSpawnCount >= maxSpawnCount)
            {
                Enemy.SetActive(true);
                isSpawn = true;
            }
        }
    }
}
