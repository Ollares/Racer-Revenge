using System;
using System.Collections;
using System.Collections.Generic;
using PoolSystem;
using UnityEngine;
using static EnemyController;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] int minCountEnemy = 3;
    [SerializeField] int maxCountEnemy = 6;
    [SerializeField] private float radiusSpawn = 5f;
    [SerializeField] Transform[] points;
    
    private void Start() {
        ActiveSpawner();
    }
    public void ActiveSpawner()
    {
        InitializeEnemies();
    }
    void InitializeEnemies()
    {
        int rndEnemy = Random.Range(minCountEnemy, maxCountEnemy);
        for (int i = 0; i < rndEnemy; i++)
        {
            var position = GetPositionSpawn();

            ActiveEnemy(EnemyType.Default, position);
        }
    }

    void ActiveEnemy(EnemyType enemyType, Vector3 position)
    {
        var enemy = PoolController.Instance.GetEnemy(enemyType);
        if (enemy)
        {
            enemy.transform.position = position;
            enemy.transform.rotation = Quaternion.Euler(0,Random.Range(-180f,180f),0);
            enemy.gameObject.SetActive(true);
            enemy.Initialize();
        }
    }
    Vector3 GetPositionSpawn()
    {
        var point = points[Random.Range(0, points.Length)];
        var rndPos = point.transform.position + Random.insideUnitSphere * radiusSpawn;
        rndPos.y = point.transform.position.y;
        
        return rndPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawWireSphere(points[i].transform.position, radiusSpawn);
        }
    }
        
}