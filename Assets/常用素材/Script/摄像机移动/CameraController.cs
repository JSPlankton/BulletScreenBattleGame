using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;        // 摄像机移动速度
    public float rotationSpeed = 100f;  // 摄像机转向速度
    public float scrollSpeed = 20f;     // 滚轮滚动速度
    public float minYAngle = -80f;      // 最小垂直旋转角度
    public float maxYAngle = 80f;       // 最大垂直旋转角度
    public float gearScrollSpeed = 5f;  // 齿轮滚动速度
    public float upDownSpeed = 10f;     // 上升下降速度

    private float rotationX ;
    private bool isRotating = false;
    private Vector3 lastMousePosition;
    private Quaternion Position;
    private Quaternion originalRotation;

    //复位记录
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public bool istrue;
    void Start()
    {
        rotationX = transform.rotation.eulerAngles.x;
        originalRotation = transform.rotation;

        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }


    void Update()
    {

        HandleInput();
        if (Input.GetKeyDown(KeyCode.Y))
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
    }

    private void HandleInput()
    {
        // 获取键盘输入
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // 获取滚轮输入并控制前进后退
        verticalInput += scrollInput * gearScrollSpeed;

        // 上升下降速度
        float upDownVelocity = (Input.GetKey(KeyCode.E) ? 1f : 0f) - (Input.GetKey(KeyCode.Q) ? 1f : 0f);
        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput + Vector3.up * upDownVelocity * upDownSpeed;

        // 移动摄像机
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // 控制齿轮高低
        Vector3 cameraPosition = transform.localPosition;
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, 0f, 200f); // 防止摄像机陷入地面
        transform.localPosition = cameraPosition;

        // 按住鼠标右键进行旋转
        if (Input.GetMouseButtonDown(1))
        {


            isRotating = true;
            lastMousePosition = Input.mousePosition;

        }

        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
            originalRotation = transform.rotation; // 在抬起右键时记录当前的旋转角度
        }

        if (isRotating)
        {
            // 计算鼠标位移
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            // 水平旋转
            transform.Rotate(Vector3.up * mouseDelta.x * rotationSpeed * Time.deltaTime);

            // 垂直旋转
            rotationX -= mouseDelta.y * rotationSpeed * Time.deltaTime;
            rotationX = Mathf.Clamp(rotationX, minYAngle, maxYAngle);
            transform.localRotation = Quaternion.Euler(rotationX, transform.localEulerAngles.y, 0f);
        }
    }
}
