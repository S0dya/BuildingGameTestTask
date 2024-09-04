using UnityEngine;

public class InteractablePickable : Interactable
{
    [SerializeField] private PickableObject pickableObject;

    public override object Interact()
    {
        return pickableObject.GetComponent<IPickable>();
    }
}