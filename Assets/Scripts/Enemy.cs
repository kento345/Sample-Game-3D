using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    private Vector3 targetRot;
    [SerializeField] private float rotSpeed = 20f;
    [SerializeField] int hp = 2;
    [SerializeField] float invincibleTimeMax = 0.5f;
    float invincibleTime = 0;
    [SerializeField] float knockbackSpeed = 5;

    Rigidbody rb;
    Animator animetor;

    public Collider playerCollider { get;set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animetor = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 無敵時間を減らす
        if(invincibleTime > 0)
        {
            invincibleTime -= Time.deltaTime;
        }
        var direction = playerCollider.bounds.center - rb.position;

        bool isSeenPlayer = true;
        if ((Physics.Raycast(rb.position,direction.normalized,out var hitInfo))){
            if (hitInfo.collider != playerCollider)
            {
                isSeenPlayer = false;
            }
        }
        if (playerCollider != null && isSeenPlayer && invincibleTime <= 0)
        {
            var subVec = playerCollider.bounds.center - rb.position;
            subVec.y = 0;
            if (subVec != Vector3.zero)
            {
                targetRot = subVec.normalized;
            }
            Vector3 fowerd = transform.forward;
            transform.up = Vector3.up;
            transform.forward = Vector3.Slerp(fowerd, targetRot, rotSpeed * Time.deltaTime);

            rb.linearVelocity = subVec.normalized * moveSpeed;
            float mag = rb.linearVelocity.magnitude;
            animetor.SetFloat("Speed", mag);
        }   
    }

    void OnCollisionStay(Collision collision)
    {
        var attacObj = collision.gameObject.GetComponent<AttackOnject>();
        if(attacObj != null && invincibleTime <= 0)
        {
            hp -= attacObj.power;
            invincibleTime = invincibleTimeMax;
            if(hp <= 0)
            {
                Destroy(gameObject);
            }
               // ノックバック
            var dir = transform.position - collision.transform.position;
            dir.y = 0;
            var knockbackVec = dir.normalized * knockbackSpeed;
            rb.linearVelocity = knockbackVec;
        }
    }
}
