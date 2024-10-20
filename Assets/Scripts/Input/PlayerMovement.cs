
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Rigidbody _rb;
    private Vector2 _movementDirection;
    [SerializeField] private float _movementSpeed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
    }
    
    private void FixedUpdate()
    {
        Vector2 normalizedMovementDirection = _movementDirection.normalized;
        
        
        _rb.velocity = new Vector3(
            normalizedMovementDirection.x * _movementSpeed,
        _rb.velocity.y,
        normalizedMovementDirection.y * _movementSpeed
        );
    }

    private void OnEnable()
    {
        _playerInput.actions["Move"].performed += GetMovementDirection;
        _playerInput.actions["Move"].canceled += GetMovementDirection;
    }

    private void OnDisable()
    {
        _playerInput.actions["Move"].performed -= GetMovementDirection;
        _playerInput.actions["Move"].canceled -= GetMovementDirection;
    }

    private void GetMovementDirection(InputAction.CallbackContext context)
    {
        _movementDirection = context.ReadValue<Vector2>();
    }
}
