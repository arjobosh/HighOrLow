using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public void StartBtn()
    {
        SceneManager.LoadScene(1);
    }

    public void BackBtn()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitBtn()
    {
        Application.Quit();
    }
}
