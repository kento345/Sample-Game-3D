using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 0f;
    Vector2 inputVer;

    PlayerInput input;
    Rigidbody rb;

    public void OnMove(InputAction.CallbackContext context)
    {
        inputVer = context.ReadValue<Vector2>();
    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        var cameraDir = input.camera.transform.forward;
        var cameraRigth = input.camera.transform.right;
        Vector3 move = (cameraRigth.normalized * inputVer.x + cameraDir.normalized * inputVer.y) * speed * Time.deltaTime;
        move.y = 0;
        transform.position += move;
    }
}
