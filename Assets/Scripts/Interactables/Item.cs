using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IInteractable, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ItemScriptableObject itemPreset;
    public int ItemId { get => itemPreset.Id; }

    //[SerializeField] private GameObject itemNameGO;
    [SerializeField] private TextMeshProUGUI itemNameText;

    [SerializeField] private UIPanel itemDescriptionPanel;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    [SerializeField] private SpriteRenderer itemSpriteRenderer;

    [SerializeField] private Transform cameraFocusPoint;

    public event Action InteractionStarted;
    public event Action InteractionEnded;

    public Vector3 Position => transform.position;

    private void Awake()
    {
        itemNameText.gameObject.SetActive(false);
        itemDescriptionPanel.SetActive(false);

        itemNameText.text = itemPreset.Name;
        itemDescriptionText.text = itemPreset.Description;
        itemSpriteRenderer.sprite = itemPreset.ItemSprite;        
    }

    private void OnValidate()
    {
        itemSpriteRenderer.sprite = itemPreset.ItemSprite;
    }

    public void StartInteraction(PlayerUI playerUI)
    {
        CameraController.Instance.ZoomIn(cameraFocusPoint);
        itemNameText.gameObject.SetActive(true);
        playerUI.ShowItemContextMenu();
    }

    public void StopInteraction()
    {
        CameraController.Instance.ZoomOut();
        itemDescriptionPanel.SetActive(false);
        itemNameText.gameObject.SetActive(false);
    }

    public void Select()
    {
        itemSpriteRenderer.material.SetFloat("_ToggleBorder", 1);
    }

    public void Deselect()
    {
        itemSpriteRenderer.material.SetFloat("_ToggleBorder", 0);
    }
    public void Inspect()
    {
        itemDescriptionPanel.SetActive(true);
    }
    public void ItemPickedUp()
    {
        InteractionEnded?.Invoke();
        gameObject.SetActive(false);
    }    
    public void ItemDropped(Vector3 dropLocation)
    {
        transform.position = dropLocation;
        gameObject.SetActive(true);
    }
    public void CloseDescriptionBox()
    {
        InteractionEnded?.Invoke();
    }

    public void DisableInteraction()
    {
        Destroy(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        InteractionStarted?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //TODO show a different border colour for hover and selected.
        Select();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Deselect();
    }
}
