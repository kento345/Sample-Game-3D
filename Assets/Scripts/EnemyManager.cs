using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Collider playerCollider;
    [SerializeField] GameObject enemy;
    [SerializeField] List<Enemy> enemies = new();

    private void OnEnable()
    {
        while(enemies.Count < 10) {
            var obj = Instantiate(enemy, new Vector3(Random.Range(-6, 75), -10, Random.Range(16, -22)), Quaternion.identity);
            Enemy e = obj.GetComponent<Enemy>();
            enemies.Add(e);
        }
        foreach (Enemy enemy in enemies)
        {
            enemy.playerCollider = playerCollider;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemies.RemoveAll(e => e == null);
        while (enemies.Count < 10)
        {
            var obj = Instantiate(enemy, new Vector3(Random.Range(-6, 75), -10, Random.Range(16, -22)), Quaternion.identity);
            Enemy e = obj.GetComponent<Enemy>();
            enemies.Add(e);
        }
    }
}
