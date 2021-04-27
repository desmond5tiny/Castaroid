using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float cameraOffset;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float zoomMin = 5f, zoomMax = 10f;
    [SerializeField] private bool smoothCamera = false;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Transform target;

    private InputManager controls;

    private void Awake()
    {
        controls = new InputManager();
    }
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    private void FixedUpdate()
    {
        Vector3 offSet = new Vector3(0, 0, cameraOffset);
        if (smoothCamera)
        {
            Vector3 desiredPos = target.position + offSet;
            Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed); //smooth camera
            transform.position = smoothPos;
        }
        else
        {
            transform.position = target.position + offSet;
        }

        float zoom = Mathf.Clamp(controls.Camera.Zoom.ReadValue<float>(), -1f, 1f);
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + (zoom * zoomSpeed * Time.deltaTime), zoomMin, zoomMax);
    }
}
