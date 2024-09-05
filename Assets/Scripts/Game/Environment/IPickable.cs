using System;
using UnityEngine;

public interface IPickable
{
    public PickableNameEnum PickableNameEnum { get; }

    public event Action<int> OnTrigger;


    public void PickedUp();
    public void PlacedDown();

    public void MoveRotateToPosition(Vector3 position, Vector3 surfaceNormal);
    public void Rotate(float rotationY);

    public void ApplyMaterial(Material material);
}

