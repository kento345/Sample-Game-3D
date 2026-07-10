using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Collider playerCollider;
    [SerializeField] Enemy[] enemies;


    private void OnEnable()
    {
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
        
    }
}
