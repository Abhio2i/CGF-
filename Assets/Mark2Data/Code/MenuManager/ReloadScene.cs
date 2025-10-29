#region Script Info
//out: if Plane not able to take-off this script help's to reload the scene.  
#endregion

using UnityEngine;
using UnityEngine.SceneManagement;
public class ReloadScene : MonoBehaviour
{
    #region local Functions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerBody"))
        {
            ReloadTheCurrentScene();
        }
    }

    private void ReloadTheCurrentScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    #endregion
}
