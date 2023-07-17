using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private InputScriptableObject input;

    [SerializeField] private List<IInteractable> interactableObjects = new List<IInteractable>();
    private IInteractable selectedObject;
    private Coroutine interactionCoroutine;

    private Inventory inventory = new Inventory();

    private Player player;
    private PlayerUI uiManager;

    private void Awake()
    {
        uiManager = GetComponent<PlayerUI>();
        player = GetComponent<Player>();
    }


    private void OnEnable()
    {
        input.InteractionPerformed += OnInteractionPerformed;
        input.InteractionCanceled += OnInteractionCanceled;
    }
    private void OnDisable()
    {
        input.InteractionPerformed -= OnInteractionPerformed;
        input.InteractionCanceled -= OnInteractionCanceled;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactableObject = other.GetComponent<IInteractable>();

        if (interactableObject == null)
            return;


        interactableObjects.Add(interactableObject);

        if (interactionCoroutine == null)
            interactionCoroutine = StartCoroutine(InteractionCoroutine());
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactableObject = other.GetComponent<IInteractable>();

        if (interactableObject == null)
            return;

        interactableObjects.Remove(interactableObject);
        
        if (interactableObject == selectedObject)
        {
            selectedObject.Deselect();
            selectedObject.StopInteraction();
            selectedObject.InteractionStarted -= OnInteractionPerformed;
            
            selectedObject = null;
        }

        if (interactableObjects.Count == 0)
        {
            if(interactionCoroutine != null)
            {
                StopCoroutine(interactionCoroutine);
                interactionCoroutine = null;
            }
        }
    }

    #region event listeners
    private void OnInteractionPerformed()
    {
        if (selectedObject == null)
            return;

        player.HideHint();
        selectedObject.StartInteraction(uiManager);

        selectedObject.InteractionEnded += OnInteractionCanceled;

        if (selectedObject is Grave)
        {
            (selectedObject as Grave).Quest.ShowHint += OnShowHint;
        }

        if (interactionCoroutine != null)
        {
            StopCoroutine(interactionCoroutine);
            interactionCoroutine = null;
        }
        if (selectedObject != null)
        {
            selectedObject.Deselect();            
        }

        input.EnableControls(false);
    }
    public void OnInteractionCanceled()
    {
        
        if (selectedObject == null)
            return;

        selectedObject.InteractionEnded -= OnInteractionCanceled;

        selectedObject.StopInteraction();

        if (selectedObject is Grave)
        {            
            (selectedObject as Grave).Quest.ShowHint -= OnShowHint;
        }

        if (interactionCoroutine == null)
            interactionCoroutine = StartCoroutine(InteractionCoroutine());

        uiManager.CloseContextMenu();
        input.EnableControls(true);

        EventSystem.current.SetSelectedGameObject(null);
    }
    private void OnShowHint(TextScriptableObject hint)
    {
        player.ShowHint(hint);        
    }    
    #endregion


    #region Selecting interactable object
    IEnumerator InteractionCoroutine()
    {
        selectedObject = null;
        while (true)
        {
            if(interactableObjects == null || interactableObjects.Count == 0)
                yield break;

            IInteractable closestObj = FindClosestObject();

            if (closestObj != selectedObject)
            {
                if (selectedObject != null)
                    selectedObject.InteractionStarted -= OnInteractionPerformed;
                
                if(closestObj != null)
                    closestObj.InteractionStarted += OnInteractionPerformed;
            }
                
            SelectNewObject(closestObj);            

            yield return null;
        }
    }

    private IInteractable FindClosestObject()
    {
        float minSqrDist = float.MaxValue;
        IInteractable closestObj = null;

        foreach (IInteractable obj in interactableObjects)
        {
            float sqrDistToObj = Vector2.SqrMagnitude(obj.Position - transform.position);
            if (sqrDistToObj < minSqrDist)
            {
                closestObj = obj;
                minSqrDist = sqrDistToObj;
            }
        }

        return closestObj;
    }

    private void SelectNewObject(IInteractable closestObj)
    {
        //deselect old object
        if (selectedObject != null && closestObj != selectedObject)
        {
            selectedObject.Deselect();
            selectedObject.StopInteraction();
        }


        if (closestObj != null && selectedObject != closestObj)
            closestObj.Select();

        selectedObject = closestObj;
    }
    #endregion

    #region UI Interaction button event listeners
    public void OnItemInspect()
    {
        (selectedObject as Item).Inspect();
    }
    public void OnItemPickUp()
    {
        inventory.PickUp(selectedObject as Item);        
    }
    public void OnGraveInteraction()
    {
        (selectedObject as Grave).Interact();
    }
    public void OnGraveQuestHandIn()
    {        
        inventory.HandInItem(selectedObject as Grave);
    }

    #endregion
}
