using System.Collections;
using System.Collections.Generic;
using System.IO; // Necessário para usar StreamWriter
using UnityEngine;

public class PlayerNumbers : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public Transform jogador;
    public Transform objetivoFinal; // Referência ao ponto de "Finish"
    
    private float tempoInicio;
    private float tempoFinal;
    private int itensPegos = 0;
    private bool venceu = false; // Se o jogador venceu a fase
    private float distanciaParaVitoria = 2f; // Distância necessária para vencer (ajuste conforme necessário)

    // Caminho do arquivo CSV
    static private string projectPath = Application.dataPath;
    public string testFilePath = Application.streamingAssetsPath + @"/dados_player.csv"; // Caminho atualizado

    // Variáveis públicas para exibir os dados
    public int tempoPercorrido; // Tempo total em segundos
    public int quantidadeItensPegos;
    public bool jogadorVenceu;

    void Start()
    {
        tempoInicio = Time.time; // Marca o início da contagem de tempo
        itensPegos = 0;
        venceu = false;
        jogador = GameObject.FindWithTag("Teseu").transform; // Ou ajuste para o nome correto do jogador

        // Encontre o objetivo (ponto de "Finish") com base na tag
        objetivoFinal = GameObject.FindWithTag("Finish").transform;

        if (objetivoFinal == null)
        {
            Debug.LogError("Objetivo Final (Finish) não encontrado na cena!");
        }

        // Verifique se o arquivo já existe, se não, crie
        if (!File.Exists(testFilePath))
        {
            // Cria o arquivo com cabeçalho
            using (StreamWriter sw = File.CreateText(testFilePath))
            {
                sw.WriteLine("Ganhou,TimePlayed,ItemCount"); // Cabeçalho
            }
        }
    }

    void Update()
    {
        // Verifica se o jogador está perto da saída para vencer
        if (!venceu && objetivoFinal != null)
        {
            ChecarVitoria();
        }

        // Simula a coleta de um item para teste
        if (Input.GetKeyDown(KeyCode.I)) // Pressione "I" para simular a coleta de item
        {
            ColetarItem(); // Incrementa o contador de itens
        }
    }

    // Método para checar a proximidade com a saída e finalizar a fase (vitória)
    void ChecarVitoria()
    {
        // Verifica a distância entre o jogador e o objetivo de "Finish"
        if (Vector3.Distance(jogador.position, objetivoFinal.position) <= distanciaParaVitoria)
        {
            FinalizarFase(true);
        }
    }

    // Método para finalizar a fase (vitória ou derrota)
    public void FinalizarFase(bool venceu)
    {
        if (this.venceu) return; // Evita que a fase seja finalizada mais de uma vez

        tempoFinal = Time.time; // Registra o tempo ao final da fase
        tempoPercorrido = Mathf.RoundToInt(tempoFinal - tempoInicio);
        jogadorVenceu = venceu; // Define se o jogador venceu ou perdeu

        // Exibe ou armazena os dados conforme necessário
        Debug.Log("Tempo Percorrido: " + tempoPercorrido.ToString("F2") + " segundos.");
        Debug.Log("Itens Coletados: " + itensPegos);
        Debug.Log("Vitória: " + (venceu ? "Sim" : "Não"));

        // Grava os dados no arquivo CSV
        GravarDadosCSV();
    }

    // Método para coletar um item
    public void ColetarItem()
    {
        itensPegos++;
        Debug.Log($"Item coletado! Total agora: {itensPegos}");
    }

    // Método para sobrescrever os dados no arquivo CSV mantendo o cabeçalho
    public void GravarDadosCSV()
    {
        // Lê todas as linhas do arquivo
        List<string> linhas = new List<string>(File.ReadAllLines(testFilePath));

        // Se o arquivo já contiver dados, sobrescreva-os mantendo o cabeçalho
        if (linhas.Count > 1)
        {
            // Remove as linhas de dados existentes (apenas mantendo o cabeçalho)
            linhas.RemoveRange(1, linhas.Count - 1);
        }

        // Adiciona os dados mais recentes
        string linhaDados = $"{(jogadorVenceu ? 1 : 0)},{tempoPercorrido},{itensPegos}";
        linhas.Add(linhaDados);

        // Grava as linhas novamente no arquivo, sobrescrevendo o conteúdo
        File.WriteAllLines(testFilePath, linhas);

        Debug.Log("Dados gravados no arquivo CSV.");
    }
}
