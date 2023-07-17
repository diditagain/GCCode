using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName ="Input Events Scriptable Object")]
public class InputScriptableObject : ScriptableObject
{
    private InputManager inputManager;

    #region Events
    public event Action InteractionPerformed;
    public event Action InteractionCanceled;
    public event Action PickUpActionPerformed;
    public event Action StartMenuOpened;

    public event Action MovementPerformed;
    public event Action MovementCanceled;
    #endregion

    #region Event Listeners
    public void OnInteractionPerformed(InputAction.CallbackContext obj) { InteractionPerformed?.Invoke(); }
    public void OnInteractionCanceled(InputAction.CallbackContext obj) { InteractionCanceled?.Invoke(); }   
    public void OnStartMenuOpened(InputAction.CallbackContext obj) { StartMenuOpened?.Invoke(); }

    public void OnMovePerformed(InputAction.CallbackContext obj) { MovementPerformed?.Invoke(); }
    public void OnMoveCanceled(InputAction.CallbackContext obj) { MovementCanceled?.Invoke(); }


    public void SetInputManager(InputManager inputManager)
    {
        this.inputManager = inputManager;
    }
    public void ResetInput()
    {
        inputManager = null;
    }
    #endregion

    #region Values
    public Vector2 MovementVector
    {
        get 
        { 
            return inputManager == null ? Vector2.zero : inputManager.ReadMovementVector(); 
        }
    }
    #endregion

    public void EnableControls(bool enabled)
    {
        if (inputManager == null)
            return;

        inputManager.EnableControls(enabled);
    }

}
