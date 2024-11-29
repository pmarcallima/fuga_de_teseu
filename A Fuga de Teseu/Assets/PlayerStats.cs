using UnityEngine;
using System.IO; // Para manipular arquivos
using UnityEngine.SceneManagement;

public class PlayerStatsTracker : MonoBehaviour
{
    public int itemsCollected = 0; // Contador de itens coletados
    private float elapsedTime = 0f; // Tempo percorrido
    private bool gameWon = false; // Se o jogador ganhou ou perdeu
    private bool gameEnded = false; // Para evitar salvar múltiplas vezes
    
    // Caminho do arquivo CSV
    private static string projectPath = Application.dataPath;
    private string testFilePath = projectPath + @"\Scripts\dados_player.csv";

    void Start()
    {
        // Cria o arquivo CSV se ainda não existir e adiciona cabeçalho
        if (!File.Exists(testFilePath))
        {
            File.WriteAllText(testFilePath, "Tempo,Itens Coletados,Ganhou\n");
        }
    }

    void Update()
    {
        // Atualiza o tempo se o jogo ainda não acabou
        if (!gameEnded)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    // Método chamado ao coletar um item
    public void CollectItem()
    {
        itemsCollected++;
    }

    // Método chamado quando o jogador ganha o jogo
    public void OnWin()
    {
        if (!gameEnded)
        {
            gameWon = true;
            gameEnded = true;
            SaveStats();
        }
    }

    // Método chamado quando o jogador perde o jogo
    public void OnLose()
    {
        if (!gameEnded)
        {
            gameWon = false;
            gameEnded = true;
            SaveStats();
        }
    }

    // Salva os dados no arquivo CSV
    private void SaveStats()
    {
        string line = $"{elapsedTime:F2},{itemsCollected},{gameWon}\n";
        File.AppendAllText(testFilePath, line);
        Debug.Log($"Estatísticas salvas: {line}");
    }

    // Detecta colisões para verificar vitória ou derrota
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            OnWin();
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            OnLose();
        }
    }
}
