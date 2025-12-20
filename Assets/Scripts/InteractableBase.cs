using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InteractableBase : MonoBehaviour
{
    // modify this text in the inspector to change the prompt above the player's head
    [SerializeField] private string promptText = "Press E";

    public static TMP_Text promptTMP;
    private static GameObject playerPromptUI;
    private bool playerInside = false;

    // grabs the InteractUI from the player and disables it at start
    protected virtual void Awake()
    {
        if (playerPromptUI == null)
        {
            playerPromptUI = GameObject.FindGameObjectWithTag("InteractUI");

            if (playerPromptUI == null)
            {
                Debug.LogError("You didn't give the InteractUI object under the player the InteractUI tag you bozo");
                return;
            }
           
             playerPromptUI.SetActive(false);

            promptTMP = playerPromptUI.GetComponentInChildren<TMP_Text>(true);

            if (promptTMP == null)
                Debug.LogError("No TMP_Text found under InteractUI.");

        }
    }

    // calls show prompt when player enters trigger
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        ShowPrompt();
        OnPlayerEnter(other.gameObject);
    }

    // hides the prompt when player exits trigger
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        HidePrompt();
        OnPlayerExit(other.gameObject);
    }

    // checks if the player pressed E while inside the trigger. You can ignore by leaving OnInteract empty/override this method
    protected virtual void Update()
    {
        if (!playerInside) return;

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            OnInteract();
        }
    }

    // shows the prompt UI
    protected void ShowPrompt()
    {
        if (playerPromptUI == null) return;

        promptTMP.text = promptText;


        playerPromptUI.SetActive(true);

    }

    // hides the prompt UI
    protected void HidePrompt()
    {
        if (playerPromptUI == null) return;

        playerPromptUI.SetActive(false);
    }

    protected string GetPromptText()
    {
        return promptText;
    }

    // whats happens when the player presses E while inside the trigger unless Update was overridden
    protected abstract void OnInteract();

    // checks for player entering/exiting trigger
    protected virtual void OnPlayerEnter(GameObject player) {
        playerInside = true;
    }
    protected virtual void OnPlayerExit(GameObject player) {
        playerInside = false;
}
}