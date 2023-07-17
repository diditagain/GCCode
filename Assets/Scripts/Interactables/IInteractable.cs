using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IInteractable
{
    public Vector3 Position { get; }
    public event Action InteractionStarted;
    public event Action InteractionEnded;
    public void StartInteraction(PlayerUI playerUI);
    public void StopInteraction();
    public void Select();
    public void Deselect();
}

