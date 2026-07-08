using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(QuittingGame());
    }

    
    IEnumerator QuittingGame()
    {
        yield return new WaitForSeconds(5f);
        Application.Quit();
    }
}
