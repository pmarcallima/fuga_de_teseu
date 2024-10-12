using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger1 : MonoBehaviour
{
    public string SampleScene;

    public void ChangeScene()
    {
        SceneManager.LoadScene(SampleScene, LoadSceneMode.Single);
    }
}
