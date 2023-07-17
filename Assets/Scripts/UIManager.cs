using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private UIPanel startMenu;
    [SerializeField] private InputScriptableObject input;

    private bool startMenuOpen = false;
    public bool ContextMenuOpen = false;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
    }

    private void OnEnable()
    {
        input.StartMenuOpened += ToggleStartMenu;        
        startMenu.SetActive(false);
    }
    private void OnDisable()
    {
        input.StartMenuOpened -= ToggleStartMenu;
    }

    public void ToggleStartMenu()
    {
        if (ContextMenuOpen)
            return;

        input.EnableControls(startMenuOpen);
        startMenuOpen = !startMenuOpen;
        startMenu.SetActive(startMenuOpen);
    }
}
