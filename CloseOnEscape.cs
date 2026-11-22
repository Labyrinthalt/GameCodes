using UnityEngine;
using UnityEngine.InputSystem;

public class CloseOnEscape : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            gameObject.SetActive(false);
        }
    }
}