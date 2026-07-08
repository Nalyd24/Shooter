using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
   public static bool gameIsPaused = false;
    InputSystem_Actions inputActions;
    public InputAction pause;
    public GameObject pauseMenuUI;
  void Awake()
  {
    inputActions = new InputSystem_Actions();
    pause  = inputActions.Player.Pause;
  }
  private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

  void Update()
   {
      if (pause.triggered)
      {
         if (gameIsPaused)
         {
            Resume();
         }
         else
         {
            Pause();
         }
      }
   }

   public void Resume()
   {
      pauseMenuUI.SetActive(false);
      Time.timeScale = 1f;
      gameIsPaused = false;
      Cursor.lockState = CursorLockMode.Locked;
   }

   public void Pause()
   {
      pauseMenuUI.SetActive(true);
      Time.timeScale = 0f;
      gameIsPaused = true;
      Cursor.lockState = CursorLockMode.None;
   }

   public void LoadMenu()
   {
      SceneManager.LoadScene("MainMenuLoadScreen");
      Time.timeScale = 1f;
   }

   public void QuitGame()
   {
      SceneManager.LoadScene("QuitGameLoadScreen");
      Time.timeScale = 1f;
   }


}
