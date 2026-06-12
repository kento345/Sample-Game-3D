using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform lookTarget; //注視点の位置
    [SerializeField] Vector3 offset;       //注視点の補正
    [SerializeField] float targetDistance; //カメラの距離

    [SerializeField] float rotSpeed;

    float pitch; //縦
    float yaw;　 //横

    Vector2 inputVer;

    public void Look(InputAction.CallbackContext context)
    {
        inputVer = context.ReadValue<Vector2>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        yaw = 90f;
        pitch = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        var rot = new Vector3(inputVer.x, inputVer.y, 0) * rotSpeed * Time.deltaTime;
        yaw += rot.x;
        pitch += rot.y;

        var target = lookTarget.position + offset;
        var rotation = Quaternion.Euler(-pitch, yaw, 0);
        var position = rotation * new Vector3(0, 0, -targetDistance) + target;

        transform.rotation = rotation;
        transform.position = position;
    }
}
