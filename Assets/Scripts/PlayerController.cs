using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;
using UnityEngine.UIElements;
using TMPro;


public class PlayerController : MonoBehaviour
{
    [Header("Input Actions")]
    InputAction _IA;
    InputActions _playerInputs;

    [Header("Movement Related")]
    private Rigidbody _rb;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDir = Vector3.zero;


    [Header("Camera")]
    [SerializeField]
    private Camera _playerCam;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInputs = new InputActions();
    }

    private void OnEnable()
    {
        _playerInputs.Player.Enable();
        _IA = _playerInputs.Player.Movement;   
        

    }
    private void OnDisable()
    {
        _playerInputs.Player.Disable();
    }
    private void Update()
    {

    }
    private void FixedUpdate()
    {
        forceDir += _IA.ReadValue<Vector2>().x * GetCameraRight(_playerCam) * movementForce;
        forceDir += _IA.ReadValue<Vector2>().y * GetCameraForward(_playerCam) * movementForce;
        
        _rb.AddForce(forceDir, ForceMode.Impulse);
        forceDir = Vector3.zero;

        if (_rb.velocity.y < 0f)
        {
            _rb.velocity += Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;
        }

        Vector3 horizontalVelo = _rb.velocity;
        horizontalVelo.y = 0f;
        if (horizontalVelo.sqrMagnitude > maxSpeed * maxSpeed)
            _rb.velocity = horizontalVelo.normalized * maxSpeed + Vector3.up * _rb.velocity.y;

        LookAt();
    }

    private void LookAt()
    {
        //TODO: Take control of the player's rotation
        Vector3 lookDir = _rb.velocity;
        lookDir.y = 0f;
        if (_IA.ReadValue<Vector2>().sqrMagnitude > 0.1f && lookDir.sqrMagnitude > 0.1f)
        {
            this._rb.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
        }
        else
        {
            _rb.angularVelocity = Vector3.zero;
        }

    }
    private Vector3 GetCameraForward(Camera playerCam)
    {
        Vector3 forward = _playerCam.transform.forward;
        forward.y = 0;
        return forward.normalized; 
    }

    private Vector3 GetCameraRight(Camera playerCam)
    {
        Vector3 right = _playerCam.transform.right;
        right.y = 0;
        return right.normalized;
    }

   
    

  


   

 
}
