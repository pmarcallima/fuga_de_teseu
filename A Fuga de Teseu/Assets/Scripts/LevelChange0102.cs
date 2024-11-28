using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange0102 : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o objeto colidido é o "invisibleExit"
        if (collision.gameObject.name == "invisibleExit")
        {
            SceneManager.LoadScene("Labirinto-02");

            // Registra um evento quando a nova cena for carregada
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    // Método chamado quando a cena for carregada
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Desvincula o evento para evitar chamadas repetidas
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // Agora você pode verificar se o GameObject "JoaoMadeira" existe na nova cena
        GameObject obj = GameObject.Find("JoaoMadeira");

        if (obj != null)
        {
            // Acessa o script JoaoMadeira do GameObject
            LogisticRegression lr = obj.GetComponent<LogisticRegression>();

            if (lr != null)
            {
                // Chama a função dentro do script JoaoMadeira
                lr.PredictFromFile(lr.testFilePath);
            }
            else
            {
                Debug.Log("O script JoaoMadeira não está anexado ao GameObject.");
            }
        }
        else
        {
            Debug.Log("O GameObject JoaoMadeira não está na cena.");
        }
    }
}
