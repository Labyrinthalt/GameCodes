using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public GameObject creditsPanel; // I-drag ang CreditsPanel dito sa Inspector
    public GameObject mainMenuPanel; // I-drag ang MainMenuPanel dito (optional)

    // Function para lumipat ng buong scene
    public void switchScenes(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Function para bumalik sa Main Menu mula sa Credits Panel
    public void BackToMain()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false); // Itatago ang Credits Panel
        }

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true); // Ipapakita pabalik ang Main Menu
        }
    }

    // Function para ipakita ang Credits Panel
    public void OpenCredits()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(true); // Ipapakita ang Credits Panel
        }

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false); // Itatago muna ang Main Menu
        }
    }
}