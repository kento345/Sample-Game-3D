using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    //[SerializeField] private float speed = 0f;
    [SerializeField] private float acceleration = 0f;
    Vector2 inputVer;
    [SerializeField] private float rotSpeed = 20f; 
    private Vector3 targetRot;

    [SerializeField] float jumpForce = 12f;
    [SerializeField] bool isGrounded = false;
    [SerializeField] float groundNomalyze = 0.7f;
    [SerializeField] float airDanping = 0.2f;
    [SerializeField] float groundDanping = 8f;

    [SerializeField] GameObject firePrefab;
    [SerializeField] Vector3 offset;
    [SerializeField] float fireSpeed;

     [SerializeField] int hp = 2;
    [SerializeField] float invincibleTimeMax = 0.5f;
    float invincibleTime = 0;
    [SerializeField] float knockbackSpeed = 5;


    PlayerInput input;
    Rigidbody rb;
    Animator animetor;

    public void OnMove(InputAction.CallbackContext context)
    {
        inputVer = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context){
        if (context.performed)
        {
            Shot();
        }
    }

    public void OnCursor(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        animetor = GetComponentInChildren<Animator>();
        rb.sleepThreshold = -1;
        rb.linearDamping = groundDanping;
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            rb.linearDamping = groundDanping;
        }
        else
        {
            rb.linearDamping = airDanping;
        }

        isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 無敵時間を減らす
        if(invincibleTime > 0)
        {
            invincibleTime -= Time.deltaTime;
        }

        var cameraDir = input.camera.transform.forward;
        var cameraRigth = input.camera.transform.right;
        if (isGrounded)
        {
            Vector3 move = (cameraRigth.normalized * inputVer.x + cameraDir.normalized * inputVer.y) * acceleration;
            move.y = 0;

            rb.AddForce(move, ForceMode.Acceleration);

            if (move != Vector3.zero)
            {
                targetRot = move.normalized;
            }
        }
        Vector3 fowerd = transform.forward;
        transform.up = Vector3.up;
        transform.forward = Vector3.Slerp(fowerd, targetRot, rotSpeed * Time.deltaTime);
        
        float mag = rb.linearVelocity.magnitude;
        animetor.SetFloat("Speed", mag);
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y >= groundNomalyze)
            {
                isGrounded = true;
            }
        }
    
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
            rb.AddForce(knockbackVec,ForceMode.VelocityChange);
        }
    }

    void Shot()
    {
        var position = transform.position + transform.TransformVector(offset);
        var obj = Instantiate(firePrefab,position,Quaternion.identity);
        var frb = obj.GetComponent<Rigidbody>();
        if(frb != null)
        {
            frb.AddForce(input.camera.transform.forward * fireSpeed,ForceMode.Impulse);
        }
    }
}
