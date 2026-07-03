using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //[SerializeField] private float speed = 0f;
    [SerializeField] private float acceleration = 0f;
    Vector2 inputVer;
    [SerializeField] private float rotSpeed = 0f; 
    private Vector3 targetRot;

    PlayerInput input;
    Rigidbody rb;
    Animator animetor;

    public void OnMove(InputAction.CallbackContext context)
    {
        inputVer = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        animetor = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var cameraDir = input.camera.transform.forward;
        var cameraRigth = input.camera.transform.right;
        Vector3 move = (cameraRigth.normalized * inputVer.x + cameraDir.normalized * inputVer.y ) * acceleration;
        move.y = 0;
        //transform.position += move;
        rb.AddForce(move, ForceMode.Acceleration);
        //rb.linearVelocity += move;
        if (move != Vector3.zero)
        {
            targetRot = move.normalized;
        }
        Vector3 fowerd = transform.forward;
        transform.up = Vector3.up;
        transform.forward = Vector3.Slerp(fowerd, targetRot, rotSpeed * Time.deltaTime);

        float mag = rb.linearVelocity.magnitude;
        animetor.SetFloat("Speed", mag);
    }
}
