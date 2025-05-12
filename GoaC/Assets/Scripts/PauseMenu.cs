using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject optionsMenu;
    public GameObject playerObject;

    private bool isPaused = false;
    private PlayerInput playerInput;

    void Start()
    {
        if (playerObject != null)
            playerInput = playerObject.GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                Pause();
            else
                Resume();
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        optionsMenu.SetActive(false);

        if (playerInput != null)
            playerInput.enabled = false;
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        optionsMenu.SetActive(false);

        if (playerInput != null)
            playerInput.enabled = true;
    }

    public void ShowOptions()
    {
        Debug.Log("Options Button Clicked");
        optionsMenu.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void GoToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void CloseOptionsAndReturnToPause()
    {
        optionsMenu.SetActive(false);
        pausePanel.SetActive(true);
    }
}
