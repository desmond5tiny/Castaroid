using UnityEngine;
using UnityEngine.Tilemaps;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    private InputManager controls;
    [SerializeField] private float speed = 10f;
    private Rigidbody2D rigBod;
    [SerializeField] private Tilemap groundMap;
    [SerializeField] private Tilemap collMap;

    private void Awake()
    {
        controls = new InputManager();
        rigBod = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 movementInput = controls.Movement.Move.ReadValue<Vector2>();
        transform.position += (Vector3)movementInput * speed * Time.deltaTime;
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}
