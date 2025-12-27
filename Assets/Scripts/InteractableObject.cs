using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObject : InteractableBase
{
    protected override void OnInteract()
    {
        Debug.LogError("You're interacting with this object BOZO");
        promptTMP.text = "Oh no! I'm literally dumb!";
    }
}