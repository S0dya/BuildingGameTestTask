using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour, IPickable
{
    [Header("Settings")]
    [SerializeField] private PickableNameEnum pickableName;

    [Header("Components")]
    [SerializeField] private Collider objectCollider;
    [SerializeField] private MeshRenderer meshRenderer;


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

    public PickableNameEnum PickableNameEnum => pickableName;
    public event Action<int> OnTrigger;

    private void Start()
    {
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

    public void MoveToPosition(Vector3 position)
    {
        transform.position = position;
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

}
