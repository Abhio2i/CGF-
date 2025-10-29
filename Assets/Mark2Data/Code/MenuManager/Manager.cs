#region Script Info
//output: Exit the Simulator On calling the function Exit game.
#endregion
using UnityEngine;
using UnityEngine.SceneManagement;
public class Manager : MonoBehaviour
{
    private void Awake()
    {
        
    }
    #region Global Fun
    public void ExitGame()
    {
        int sceneCount = SceneManager.GetActiveScene().buildIndex - 1;
        if (sceneCount >= 0)
        {
            SceneManager.LoadScene(sceneCount);
        }
        else
        {
            Application.Quit();
            print("application end");
        }

    }
    #endregion
}
