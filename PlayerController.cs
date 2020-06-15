using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions playerInputActions;
    public CharacterController controller;

    public float movementSpeed;
    public float rotateSpeed = .1f;
    public float gravity;
    public float jumpForce;
    float turnSmoothVelocity;
    public float turnSmoothTime = .1f;
    private Vector3 move;
    private Vector2 rotate;
    private Vector2 cameraRotate;
    private Vector2 moveSprint;

    public Transform cam;

    Vector2 lookPosition;
    // Start is called before the first frame update
    public void Awake()
    {
        playerInputActions = new PlayerInputActions();

        playerInputActions.Movement.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        playerInputActions.Movement.Movement.canceled += ctx => move = Vector2.zero;
        playerInputActions.Movement.LookRotation.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        playerInputActions.Movement.LookRotation.canceled += ctx => rotate = Vector2.zero;
        //playerInputActions.Movement.Jump.performed += ctx => move = ctx.ReadValue<Vector2>();
    }

    //public void OnLook(InputAction.CallbackContext context)
    //{
    //    rotate = context.ReadValue<Vector2>();
    //}

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        controller = new CharacterController();
        float horizontal = Input.GetAxisRaw("HorizontalMove");
        float vertical = Input.GetAxisRaw("VerticalMove");
        Vector3 rotateDirection = new Vector3(horizontal, 0f, vertical).normalized;

        //Debug.Log(horizontal + vertical);

        ////transform.rotation = Quaternion.LookRotation(rotateDirection);

        //if (rotateDirection.magnitude < 0.1f)
        //{
        //    float targetAngle = Mathf.Atan2(rotateDirection.x, rotateDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        //    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        //    transform.rotation = Quaternion.Euler(0f, angle, 0f);

        //    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        //    controller.Move(moveDir.normalized * rotateSpeed * Time.deltaTime);

        //}
        //OnRotate(rotate);
        OnMove(move);
    }

    private void OnMove(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;  
        var scaledMoveSpeed = movementSpeed * Time.deltaTime;
        var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);

        //transform.LookAt(cam);
        transform.LookAt(transform.position * 2 - Camera.main.transform.position);

        transform.position += move * scaledMoveSpeed;
        //transform.forward += move * scaledMoveSpeed;
        //transform.Rotate = move;


    }

    //private void OnRotate(Vector2 rotate)
    //{
    //    if (rotate.sqrMagnitude < 0.01)
    //        return;
    //    var scaledRotateSpeed = rotateSpeed * Time.deltaTime;
    //    rotate.y += rotate.x * scaledRotateSpeed;
    //    rotate.x -= Mathf.Clamp(rotate.x - rotate.y * scaledRotateSpeed, -89, 89);
    //    transform.localEulerAngles = rotate;
    //}

    private void OnRotate(Vector2 rotate)
    {
        if (rotate.sqrMagnitude < 0.001)
            return;
        float targetAngle = Mathf.Atan2(rotate.x, rotate.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    public void OnMoveCamera(InputAction.CallbackContext context)
    {
        if (move.sqrMagnitude < 0.01)
        {
            OnRotate(rotate);
        }
    }


        public void OnJump()
    {

    }

    void OnEnable()
    {
        playerInputActions.Movement.Movement.Enable();
        playerInputActions.Movement.LookRotation.Enable();
    }

    void OnDisable()
    {
        playerInputActions.Movement.Movement.Disable();
        playerInputActions.Movement.LookRotation.Disable();
    }
}
