using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 4f;
    public float cameraRotateSpeed = 120f;

    private Camera mainCamera;
    private CharacterController characterController;
    private bool isDragging = false;
    private float dragYPosition;
    private Vector3 dragOffset;

    private PlayerInputActions inputActions;
    private Vector2 moveInput;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled -= ctx => moveInput = Vector2.zero;
        inputActions.Player.Disable();
    }

    void Start()
    {
        mainCamera = Camera.main;
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleInstrumentInput();
        HandlePlayerMovement();
        HandleCameraRotation();
    }

    void HandleInstrumentInput()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                InstrumentData instrument = hit.collider.GetComponentInParent<InstrumentData>();
                if (instrument != null)
                {
                    isDragging = true;
                    dragYPosition = instrument.transform.position.y;

                    Plane dragPlane = new Plane(Vector3.up, new Vector3(0, dragYPosition, 0));
                    if (dragPlane.Raycast(ray, out float distance))
                    {
                        Vector3 hitPoint = ray.GetPoint(distance);
                        dragOffset = new Vector3(
                            instrument.transform.position.x - hitPoint.x,
                            0f,
                            instrument.transform.position.z - hitPoint.z
                        );
                    }

                    GameManager.Instance.SelectInstrument(instrument);
                }
                else
                {
                    GameManager.Instance.DeselectInstrument();
                }
            }
            else
            {
                GameManager.Instance.DeselectInstrument();
            }
        }

        if (isDragging)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            Plane dragPlane = new Plane(Vector3.up, new Vector3(0, dragYPosition, 0));
            if (dragPlane.Raycast(ray, out float distance))
            {
                Vector3 targetPoint = ray.GetPoint(distance);
                targetPoint = new Vector3(
                    targetPoint.x + dragOffset.x,
                    dragYPosition,
                    targetPoint.z + dragOffset.z
                );
                GameManager.Instance.DragSelectedInstrument(targetPoint);
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;
            GameManager.Instance.TryPlaceInstrument();
        }
    }

    void HandlePlayerMovement()
    {
        if (moveInput == Vector2.zero) return;

        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x);
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        characterController.Move(Vector3.down * 9.81f * Time.deltaTime);
    }

    void HandleCameraRotation()
    {
        if (Mouse.current.rightButton.isPressed)
        {
            Vector2 delta = Mouse.current.delta.ReadValue();
            Vector3 currentRotation = mainCamera.transform.eulerAngles;

            float newX = currentRotation.x - delta.y * cameraRotateSpeed * Time.deltaTime;
            float newY = currentRotation.y + delta.x * cameraRotateSpeed * Time.deltaTime;

            if (newX > 180f) newX -= 360f;
            newX = Mathf.Clamp(newX, -80f, 80f);

            mainCamera.transform.eulerAngles = new Vector3(newX, newY, 0f);
        }
    }

    public void ResetDragState()
    {
        isDragging = false;
    }
}