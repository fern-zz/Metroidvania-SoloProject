using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject optionsMenu;

    public void PlayGame()
    {
        SceneManager.LoadScene("L1");
    }

    public void OpenOptions()
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void BackToMenu()
    {
        optionsMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
