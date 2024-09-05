using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : SubjectMonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float movementSpeed = 4;

    [SerializeField] private float mouseWheelRotateSpeed = 4;

    [Header("Components")]
    [SerializeField] private PlayerRaycastController raycastController;

    [SerializeField] private CharacterController characterController;

    [Header("Other")]
    [SerializeField] private Transform cameraTransform;

    private BuildingManager _buildingManager;

    private Action _interactAction;
    private Action<float> _rotatePickableAction;

    private Vector2 _curMovementDirection;
    private Vector2 _curLookDirection;

    [Inject]
    public void Construct(BuildingManager buildingManager)
    {
        _buildingManager = buildingManager;
    }

    private void Awake()
    {
        _interactAction = InteractForInteraction;

        Init(new Dictionary<EventEnum, Action>
        {
            { EventEnum.InGameStartBuilding, OnStartBuilding },
            { EventEnum.InGameStopBuilding, OnStopBuilding },
        });
    }

    private void Update()
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

    public void InputInteract()
    {
        _interactAction.Invoke();
    }

    public void InputRotate(float rotationDirection)
    {
        _rotatePickableAction?.Invoke(rotationDirection);
    }

    private void InteractForInteraction()
    {
        raycastController.InputInteract();
    }
    private void InteractForBuilding()
    {
        raycastController.InputInteract();
    }

    public void RotatePickableForBuilding(float rotationDirection)
    {

        _buildingManager.RotatePickable(rotationDirection * mouseWheelRotateSpeed);
    }

    private void OnStartBuilding()
    {
        _interactAction = InteractForBuilding;
        _rotatePickableAction = RotatePickableForBuilding;
    }
    private void OnStopBuilding()
    {
        _interactAction = InteractForInteraction;
        _rotatePickableAction = null;
    }
}
