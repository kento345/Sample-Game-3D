using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    private Vector3 targetRot;
    [SerializeField] private float rotSpeed = 20f;

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
        var direction = playerCollider.bounds.center - rb.position;

        bool isSeenPlayer = true;
        if ((Physics.Raycast(rb.position,direction.normalized,out var hitInfo))){
            if (hitInfo.collider != playerCollider)
            {
                isSeenPlayer = false;
            }
        }
        if (playerCollider != null && isSeenPlayer)
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
}
