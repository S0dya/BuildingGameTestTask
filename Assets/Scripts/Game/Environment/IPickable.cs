using System;
using UnityEngine;

public interface IPickable
{
    public PickableNameEnum PickableNameEnum { get; }

    public event Action<int> OnTrigger;


    public void PickedUp();
    public void PlacedDown();

    public void MoveToPosition(Vector3 position);

    public void ApplyMaterial(Material material);
}

