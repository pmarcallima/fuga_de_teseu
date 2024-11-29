using UnityEngine;
using Pathfinding;
using System.Collections.Generic;  // Para usar Queue
using System.Collections;  // Necessário para corrotinas

public class ItemGenerator : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public Transform minotauro;
    public Transform objetivoFinal;
    public Grid grid;
    public AstarPath astarPath;
    public float raioGeracao = 1f;  // Raio reduzido para gerar itens mais rápido
    public int maxItems = 10;
    public float intervaloGeracao = 5f;
    public float intervaloTentativa = 1f; // Intervalo de tempo entre tentativas de gerar uma célula válida
    public int label;

    private Transform jogador;
    private int itemsGerados = 0;
    private float tempoUltimaGeracao = 0f;

    private float minotauroRaioSeguranca = 0f;

 

    private bool gerandoItens = false;

    void Start()
    {
	 GameObject obj = GameObject.Find("JoaoMadeira");

        if (obj != null)
        {
            // Acessa o script JoaoMadeira do GameObject
            LogisticRegression lr = obj.GetComponent<LogisticRegression>();

            if (lr != null)
            {
                // Chama a função dentro do script JoaoMadeira
                 label = lr.PredictFromFile(lr.testFilePath);
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
    
        jogador = GameObject.FindWithTag("Teseu").transform;

        if (astarPath == null)
        {
            astarPath = AstarPath.active;
        }

        Debug.Log("Iniciando ItemGenerator. Jogador encontrado: " + jogador.name);
    }

    void Update()
    {
        if (itemsGerados < maxItems && !gerandoItens)
        {
            Debug.Log("Iniciando geração de itens. Itens gerados: " + itemsGerados);
            StartCoroutine(GerarItensPertoDoJogador());
        }
    }

 IEnumerator GerarItensPertoDoJogador()
{
    gerandoItens = true;
    Debug.Log("Corrotina de geração de itens iniciada.");

    GridGraph gridGraph = astarPath.graphs[0] as GridGraph;

    if (gridGraph == null)
    {
        Debug.LogError("GridGraph não encontrado.");
        yield break;
    }

    // Tentativa de gerar células válidas ao longo do tempo
    while (itemsGerados < maxItems)
    {
        // Atualize a posição do jogador a cada iteração da corrotina
        Vector3 jogadorPos = jogador.position;
        Vector3Int cellPos = grid.WorldToCell(jogadorPos);

        // Agora gera uma célula aleatória com base na posição atual do jogador
        Vector3Int celulaAleatoria = GerarCelulaAleatoria(cellPos);

        if (celulaAleatoria == Vector3Int.zero)
        {
            Debug.LogError("Não foi possível gerar uma célula aleatória válida.");
            yield return new WaitForSeconds(intervaloTentativa); // Espera antes de tentar novamente
            continue; // Tenta novamente após o intervalo
        }

        Vector3 worldPosition = grid.CellToWorld(celulaAleatoria);
        GraphNode node = gridGraph.GetNearest(worldPosition).node;

        if (node != null && node.Walkable)
        {
            Debug.Log("Gerando item na posição: " + worldPosition);
            GerarItem(worldPosition);
            itemsGerados++;
            tempoUltimaGeracao = Time.time;
            yield return new WaitForSeconds(intervaloGeracao); // Espera entre as gerações de itens
        }
        else
        {
            Debug.Log("Célula aleatória não é válida (não caminhável). Tentando novamente.");
            yield return new WaitForSeconds(intervaloTentativa); // Espera antes de tentar novamente
        }
    }

    Debug.Log("Itens gerados: " + itemsGerados);
    gerandoItens = false;
}

   Vector3Int GerarCelulaAleatoria(Vector3Int jogadorPos)
{
    GridGraph gridGraph = astarPath.graphs[0] as GridGraph;

    if (gridGraph == null)
    {
        Debug.LogError("GridGraph não encontrado.");
        return Vector3Int.zero;
    }

    // Obter os limites do grid
    int gridWidth = gridGraph.width;
    int gridHeight = gridGraph.depth;

    // Gerar um ponto aleatório dentro do raio
    int xAleatorio = Random.Range(jogadorPos.x - (int)raioGeracao, jogadorPos.x + (int)raioGeracao);
    int yAleatorio = Random.Range(jogadorPos.y - (int)raioGeracao, jogadorPos.y + (int)raioGeracao);
    Debug.Log("xAleatorio: " + xAleatorio + ", yAleatorio: " + yAleatorio);
    Debug.Log("Posição do Jogador X: " + jogadorPos.x);
    Debug.Log("Raio de Geração: " + raioGeracao);



    // Verificar se está dentro do limite do grid
    if (xAleatorio < 0 || xAleatorio >= gridWidth || yAleatorio < 0 || yAleatorio >= gridHeight)
    {
        return Vector3Int.zero; // Se fora do grid, retorna Vector3Int.zero como sinal de erro
    }

    return new Vector3Int(xAleatorio, yAleatorio, 0);
}


    float CalcularDificuldade(float distMinotauro, float distObjetivo)
    {
        return (distMinotauro + distObjetivo) / 100;
    }

    bool ShouldGenerateItem(float fatorDificuldade)
    {
        return fatorDificuldade < 10f;
    }

    void GerarItem(Vector3 posicao)
    {
        GameObject item = Instantiate(itemPrefabs[Random.Range(0, itemPrefabs.Length)], posicao, Quaternion.identity);
        LaItemCollision laItem = item.GetComponent<LaItemCollision>();
        if (laItem != null)
        {
            objetivoFinal = GameObject.FindWithTag("Finish").transform;
            laItem.exitPoint = objetivoFinal;
            laItem.pathLine = FindObjectOfType<LineRenderer>();
        }

        Debug.Log("Item gerado na posição: " + posicao);
    }
}
