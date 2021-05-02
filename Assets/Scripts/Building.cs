using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Interactable
{
    [SerializeField] private bool isLocked = false;

    
    public override void Interact()
    {
        string feedback;
        base.Interact();
        feedback = checkIfLocked();

        Debug.Log(feedback);
    }

    private string checkIfLocked()
    {
        if (isLocked) { return ("The door is locked."); }
        return ("Enter the building.");
    }

    public void UnlockDoor() { isLocked = false; }

    public void LockDoor() { isLocked = true; }
}
