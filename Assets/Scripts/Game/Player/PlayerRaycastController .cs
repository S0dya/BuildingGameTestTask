using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Zenject;

public class PlayerRaycastController : SubjectMonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float raycastDistance = 6;

    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private LayerMask buildingRelatedLayer;

    [Header("Other")]
    [SerializeField] private Camera raycastCamera;

    private BuildingManager _buildingManager;

    private Action _raycastAction;
    private Action _interactionAction;

    private RaycastHit _curHit;
    private LayerMask _curPickableLayerMask;
    private SurfaceTypeEnum[] _curValidSurfaces;

    private Vector3 _curPickableTargetPosition;

    private bool _curOnInteractable;

    [Inject]
    public void Construct(BuildingManager buildingManager)
    {
        _buildingManager = buildingManager;
    }

    private void Awake()
    {
        _raycastAction = RaycastForInteraction;
        _interactionAction = OnInteractWithInteractable;

        Init(new Dictionary<EventEnum, Action>
        {
            { EventEnum.InGameStartBuilding, OnStartBuilding },
            { EventEnum.InGameStopBuilding, OnStopBuilding },
        });
    }

    private void Update()
    {
        _raycastAction?.Invoke();
    }

    private void RaycastForInteraction()
    {
        if (Physics.Raycast(raycastCamera.transform.position, raycastCamera.transform.forward, out _curHit, raycastDistance, interactableLayer))
        {
            if (!_curOnInteractable)
            {
                _curOnInteractable = true;

            }
        }
        else if (_curOnInteractable)
        {
            _curOnInteractable = false;

        }
    }
    private void RaycastForBuilding()
    {
        if (Physics.Raycast(raycastCamera.transform.position, raycastCamera.transform.forward, out _curHit, raycastDistance, buildingRelatedLayer))
        {
            var surfaceType = _curHit.collider.gameObject.GetComponent<SurfaceType>();
            if (surfaceType != null)
            {
                _curPickableTargetPosition = _curHit.point + _curHit.normal;

                _buildingManager.SetOnPlacingLayer((_curPickableLayerMask & (1 << _curHit.collider.gameObject.layer)) != 0 
                    && _curValidSurfaces.Contains(surfaceType.SurfaceTypeEnum));
                _buildingManager.MovePickableToPosition(_curPickableTargetPosition, _curHit.normal);
            }
        }
        else
        {
            _curPickableTargetPosition = raycastCamera.transform.position + raycastCamera.transform.forward * raycastDistance;

            _buildingManager.SetOnPlacingLayer(false);
            _buildingManager.MovePickableToPosition(_curPickableTargetPosition, Vector3.up);
        }
    }

    public void InputInteract()
    {
        _interactionAction.Invoke();
    }
    private void OnInteractWithInteractable()
    {
        if (_curOnInteractable)
        {
            var interactableObject = _curHit.collider.GetComponent<Interactable>().Interact();

            switch (interactableObject)
            {
                case IPickable pickable:
                    _buildingManager.PickUpPickableStartBuilding(pickable, out _curPickableLayerMask, out _curValidSurfaces);

                    RaycastForBuilding();
                    break;
            }

            //_curOnInteractable = false;
        }
    }
    private void OnInteractInBuilding()
    {
        if (_buildingManager.CanPlacePickable()) _buildingManager.PutDownPickableStopBuilding();
    }

    private void OnStartBuilding()
    {
        _raycastAction = RaycastForBuilding;
        _interactionAction = OnInteractInBuilding;
    }
    private void OnStopBuilding()
    {
        _raycastAction = RaycastForInteraction;
        _interactionAction = OnInteractWithInteractable;
    }
}
