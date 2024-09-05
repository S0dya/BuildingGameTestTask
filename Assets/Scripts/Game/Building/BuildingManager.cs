using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using System.Collections;

public enum PickableNameEnum
{
    Cube, 
    Sphere,
}
public enum SurfaceTypeEnum
{
    none,
    PickableObjectCube,
    PickableObjectSphere,
}

[System.Serializable]
class PickableObjectInfo
{
    [SerializeField] public PickableNameEnum PickableName;
    [SerializeField] public LayerMask PlaceableLayerMask;
    [SerializeField] public SurfaceTypeEnum[] ValidSurfaces;
    [SerializeField] public Vector3 PickableHeightOffset;
    [SerializeField] public float RotationLimit;
}

public class BuildingManager : SubjectMonoBehaviour
{
    [Header("Settigs")]
    [SerializeField] private PickableObjectInfo[] pickableObjectsInfos;

    [Header("Other")]
    [SerializeField] private Material cantBePlacedMaterial;
    [SerializeField] private Material canBePlacedMaterial;

    private IPickable _curPickable;
    private PickableObjectInfo _curPickableObjectInfo;

    private int _curPickableTriggeredAmount;
    private bool _curPickableHasValidRotation;
    private bool _curPickableOnLayerForPlacing;
    private bool _curPickableCanBePlaced;

    private void Awake()
    {

        Init(new Dictionary<EventEnum, Action>
        {
            { EventEnum.InGameStartBuilding, OnStartBuilding },
        });
    }

    public void PickUpPickableStartBuilding(IPickable pickable, 
        out LayerMask pickablePlaceableLayerMask, out SurfaceTypeEnum[] validSurfaces)
    {
        _curPickable = pickable;

        _curPickableObjectInfo = pickableObjectsInfos.First(x => x.PickableName == _curPickable.PickableNameEnum);
        pickablePlaceableLayerMask = _curPickableObjectInfo.PlaceableLayerMask;
        validSurfaces = _curPickableObjectInfo.ValidSurfaces;







        _curPickable.PickedUp();
        _curPickable.OnTrigger += OnPickableTriggered;
        _curPickable.OnRotated += OnPickableRotated;

        Observer.OnHandleEvent(EventEnum.InGameStartBuilding);
    }

    public void PutDownPickableStopBuilding()
    {
        _curPickable.PlacedDown();
        _curPickable.OnTrigger -= OnPickableTriggered;

        Observer.OnHandleEvent(EventEnum.InGameStopBuilding);
    }

    public void MovePickableToPosition(Vector3 position, Vector3 surfaceNormal)
    {
        _curPickable.MoveRotateToPosition(position + surfaceNormal * _curPickableObjectInfo.PickableHeightOffset.y, surfaceNormal);
    }
    public void RotatePickable(float rotationY)
    {
        _curPickable.Rotate(rotationY);
    }

    public void SetOnPlacingLayer(bool val)
    {
        if (_curPickableOnLayerForPlacing != val) CheckSignalPlayerAbleToPlace();

        _curPickableOnLayerForPlacing = val;
    }

    private void CheckSignalPlayerAbleToPlace()
    {
        if (_curPickableTriggeredAmount > 0 || !_curPickableOnLayerForPlacing || !_curPickableHasValidRotation)
        {
            _curPickable.ApplyMaterial(cantBePlacedMaterial);

            _curPickableCanBePlaced = false;
        }
        else if (_curPickableTriggeredAmount == 0 && _curPickableOnLayerForPlacing && _curPickableHasValidRotation)
        {
            _curPickable.ApplyMaterial(canBePlacedMaterial);

            _curPickableCanBePlaced = true;
        }
    }
    public bool CanPlacePickable() => _curPickableCanBePlaced;

    private void OnPickableTriggered(int triggersAmount)
    {
        _curPickableTriggeredAmount = triggersAmount;

        if (_curPickableTriggeredAmount == 0 || _curPickableTriggeredAmount == 1) CheckSignalPlayerAbleToPlace();
    }
    private void OnPickableRotated(Vector3 rotationValues)
    {
        float normalizedX = NormalizeRotation(rotationValues.x);
        float normalizedZ = NormalizeRotation(rotationValues.z);

        bool validRotation = _curPickableObjectInfo.RotationLimit > Mathf.Abs(normalizedX) &&
                             _curPickableObjectInfo.RotationLimit > Mathf.Abs(normalizedZ);

        if (_curPickableHasValidRotation != validRotation)
        {
            _curPickableHasValidRotation = validRotation;
            CheckSignalPlayerAbleToPlace();
        }
    }
    private float NormalizeRotation(float rotation) => Mathf.Repeat(rotation + 180f, 360f) - 180f;

    private IEnumerator ResetPickable()
    {
        yield return null;

        CheckSignalPlayerAbleToPlace();
    }

    private void OnStartBuilding() => StartCoroutine(ResetPickable());
}

