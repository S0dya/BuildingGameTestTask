using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SubjectMonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float movementSpeed = 4;

    [Header("Components")]
    [SerializeField] private CharacterController characterController;

    [Header("Other")]
    [SerializeField] private Transform cameraTransform;


    private Vector2 _curMovementDirection;
    private Vector2 _curLookDirection;

    void Update()
    {
        characterController.Move((transform.forward * _curMovementDirection.y + transform.right * _curMovementDirection.x) * movementSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0.0f, _curLookDirection.x, 0.0f);
        cameraTransform.localRotation = Quaternion.Euler(-_curLookDirection.y, 0.0f, 0.0f); 
    }


    public void InputMove(Vector2 direction)
    {
        _curMovementDirection = direction;
    }
    public void InputMoveStopped()
    {
        _curMovementDirection = Vector2.zero;
    }

    public void InputLook(Vector2 direction)
    {
        _curLookDirection = new Vector2(_curLookDirection.x + direction.x, Mathf.Clamp(_curLookDirection.y + direction.y, -90, 90));
    }


}
