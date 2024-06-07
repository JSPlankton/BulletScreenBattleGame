using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;        // ������ƶ��ٶ�
    public float rotationSpeed = 100f;  // �����ת���ٶ�
    public float scrollSpeed = 20f;     // ���ֹ����ٶ�
    public float minYAngle = -80f;      // ��С��ֱ��ת�Ƕ�
    public float maxYAngle = 80f;       // ���ֱ��ת�Ƕ�
    public float gearScrollSpeed = 5f;  // ���ֹ����ٶ�
    public float upDownSpeed = 10f;     // �����½��ٶ�

    private float rotationX ;
    private bool isRotating = false;
    private Vector3 lastMousePosition;
    private Quaternion Position;
    private Quaternion originalRotation;

    //��λ��¼
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
        // ��ȡ��������
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // ��ȡ�������벢����ǰ������
        verticalInput += scrollInput * gearScrollSpeed;

        // �����½��ٶ�
        float upDownVelocity = (Input.GetKey(KeyCode.E) ? 1f : 0f) - (Input.GetKey(KeyCode.Q) ? 1f : 0f);
        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput + Vector3.up * upDownVelocity * upDownSpeed;

        // �ƶ������
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // ���Ƴ��ָߵ�
        Vector3 cameraPosition = transform.localPosition;
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, 0f, 200f); // ��ֹ������������
        transform.localPosition = cameraPosition;

        // ��ס����Ҽ�������ת
        if (Input.GetMouseButtonDown(1))
        {


            isRotating = true;
            lastMousePosition = Input.mousePosition;

        }

        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
            originalRotation = transform.rotation; // ��̧���Ҽ�ʱ��¼��ǰ����ת�Ƕ�
        }

        if (isRotating)
        {
            // �������λ��
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            // ˮƽ��ת
            transform.Rotate(Vector3.up * mouseDelta.x * rotationSpeed * Time.deltaTime);

            // ��ֱ��ת
            rotationX -= mouseDelta.y * rotationSpeed * Time.deltaTime;
            rotationX = Mathf.Clamp(rotationX, minYAngle, maxYAngle);
            transform.localRotation = Quaternion.Euler(rotationX, transform.localEulerAngles.y, 0f);
        }
    }
}
