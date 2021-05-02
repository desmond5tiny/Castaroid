using UnityEngine;

public class Interactable : MonoBehaviour
{

    public float interactionRadius = 1;
    public Transform interactionTransform;

    private void Awake()
    {
        if (interactionTransform == null) { interactionTransform = transform; }
    }

    public virtual void Interact()
    {
        Debug.Log("Interact with: " + transform.name);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, interactionRadius);
    }
}
