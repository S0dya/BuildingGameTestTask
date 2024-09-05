using System;
using TMPro;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class PickableObject : MonoBehaviour, IPickable
{
    [Header("Settings")]
    [SerializeField] private PickableNameEnum pickableName;

    [Header("Components")]
    [SerializeField] private Collider objectCollider;
    [SerializeField] private MeshRenderer meshRenderer;

    public PickableNameEnum PickableNameEnum => pickableName;
    public event Action<int> OnTrigger;

    private Material _defaultMaterial;
    
    private int _curTriggeredAmount;
    private int _triggeredAmount
    {
        get { return _curTriggeredAmount; }
        set
        {
            _curTriggeredAmount = value;

            OnTrigger?.Invoke(_curTriggeredAmount);
        }
    }

    private float _curRotationY;

    private void Start()
    {
        _curRotationY = transform.rotation.eulerAngles.y;

        _defaultMaterial = meshRenderer.material;
    }

    public void PickedUp()
    {
        _curTriggeredAmount = 0;

        objectCollider.isTrigger = true;
    }

    public void PlacedDown()
    {
        meshRenderer.material = _defaultMaterial;

        objectCollider.isTrigger = false;
    }

    public void MoveRotateToPosition(Vector3 position, Vector3 surfaceNormal)
    {
        transform.position = position;
        transform.up = surfaceNormal;

        SetRotation();
    }
    public void Rotate(float rotationY)
    {
        _curRotationY += rotationY;

        SetRotation();
    }

    public void ApplyMaterial(Material material)
    {
        meshRenderer.material = material;
    }

    private void OnTriggerEnter(Collider other)
    {
        _triggeredAmount++;
    }
    private void OnTriggerExit(Collider other)
    {
        _triggeredAmount--;
    }

    private void SetRotation() => transform.Rotate(0, _curRotationY, 0);
}
