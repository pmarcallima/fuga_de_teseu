using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelChange0304 : MonoBehaviour
{
void OnCollisionEnter2D(Collision2D collision)
{
    // Verifica se o objeto colidido é o "invisibleExit"
    if (collision.gameObject.name == "invisibleExit")
    {
        // Chama o método para gravar os dados antes de carregar a nova cena
        StartCoroutine(GravarDadosAntesDeMudarDeFase());
    }
}

// Coroutine para gravar dados antes de mudar de cena
IEnumerator GravarDadosAntesDeMudarDeFase()
{
    GameObject statsObject = GameObject.FindWithTag("Stats");

    if (statsObject == null)
    {
        Debug.LogError("GameObject com a tag 'Stats' não encontrado. Certifique-se de que ele está na cena.");
        yield break; // Encerra a coroutine para evitar mais erros
    }

    // Obtém o componente PlayerNumbers no GameObject "playerstats"
    PlayerNumbers playerNumbers = statsObject.GetComponent<PlayerNumbers>();

    // Grava os dados do jogador no CSV
    playerNumbers.GravarDadosCSV();

    // Aguarda um pequeno tempo para garantir que a escrita foi concluída
    yield return new WaitForSeconds(0.5f); // Ajuste o tempo conforme necessário

    // Agora que os dados foram gravados, muda a cena
    SceneManager.LoadScene("Labirinto-04");

    // Registra um evento quando a nova cena for carregada
    SceneManager.sceneLoaded += OnSceneLoaded;
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