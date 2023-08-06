using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHelper : MonoBehaviour
{
    public static event Action OnRotate;
    public static PlayerInputHelper Instance { get; private set; }

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction rotateAction;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        rotateAction = playerInput.actions["Rotate"];

    }
    private void Start()
    {
        rotateAction.performed += RotateAction_performed;
    }
    private void OnDestroy()
    {
        rotateAction.performed -= RotateAction_performed;
    }
    private void RotateAction_performed(InputAction.CallbackContext obj)
    {
        OnRotate.Invoke();
    }

    public Vector2 GetMovementVector()
    {
        return moveAction.ReadValue<Vector2>();
    }
}
