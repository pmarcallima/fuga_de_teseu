using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string fase1;

    public void ChangeScene()
    {
        SceneManager.LoadScene(fase1, LoadSceneMode.Single);
    }
}
