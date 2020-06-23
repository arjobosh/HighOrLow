using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public void StartBtn()
    {
        SceneManager.LoadScene(1);
    }

    public void RulesBtn()
    {

    }

    public void QuitBtn()
    {
        Application.Quit();
    }
}
