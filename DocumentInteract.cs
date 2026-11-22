using UnityEngine;
using TMPro;

public class DocumentInteract : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject promptObject;  
    [SerializeField] private GameObject documentPanel;  
    [SerializeField] private TMP_Text documentText;

    [Header("Content")]
    [TextArea(5, 20)]
    [SerializeField] private string documentContent;

    private bool playerInRange;
    private bool isOpen;

    void Start()
    {
        if (promptObject) promptObject.SetActive(false);
        if (documentPanel) documentPanel.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isOpen) CloseDocument();
            else OpenDocument();
        }

        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
            CloseDocument();
    }

    void OpenDocument()
    {
        isOpen = true;
        documentText.text = documentContent;
        documentPanel.SetActive(true);
        if (promptObject) promptObject.SetActive(false);
      
    }

    void CloseDocument()
    {
        isOpen = false;
        documentPanel.SetActive(false);
        if (promptObject && playerInRange) promptObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
        if (promptObject && !isOpen) promptObject.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        if (promptObject) promptObject.SetActive(false);
        if (isOpen) CloseDocument();
    }
}