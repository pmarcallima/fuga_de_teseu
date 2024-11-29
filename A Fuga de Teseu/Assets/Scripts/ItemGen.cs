using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Rendering.Universal;

public class ItemGenerator : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public Transform minotauro;
    public Transform objetivoFinal;
    public Grid grid;
    public AstarPath astarPath;
    public float raioGeracao = 1f; // Raio reduzido para gerar itens mais rápido
    public int maxItems = 10;
    public float intervaloGeracao = 5f;
    public float intervaloTentativa = 1f; // Intervalo de tempo entre tentativas de gerar uma célula válida
    public int label;

    private Transform jogador;
    private int itemsGerados = 0;
    private bool gerandoItens = false;
    private Seeker seeker; // Declaração da variável seeker

    void Start()
    {
        GameObject obj = GameObject.Find("JoaoMadeira");

        if (obj != null)
        {
            LogisticRegression lr = obj.GetComponent<LogisticRegression>();

            if (lr != null)
            {
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

        seeker = GetComponent<Seeker>(); // Inicializa o seeker
        if (seeker == null)
        {
            Debug.LogError("O componente Seeker não está anexado a este GameObject.");
        }

        Debug.Log("Iniciando ItemGenerator. Jogador encontrado: " + jogador.name);
    }

    void Update()
    {
        if (itemsGerados < maxItems && !gerandoItens)
        {
            StartCoroutine(GerarItensPertoDoJogador());
        }
    }

    IEnumerator GerarItensPertoDoJogador()
    {
        gerandoItens = true;

        GridGraph gridGraph = astarPath.graphs[0] as GridGraph;

        if (gridGraph == null)
        {
            Debug.LogError("GridGraph não encontrado.");
            yield break;
        }

        while (itemsGerados < maxItems)
        {
            Vector3 jogadorPos = jogador.position;
            Vector3Int cellPos = grid.WorldToCell(jogadorPos);

            Vector3Int celulaAleatoria = GerarCelulaAleatoria(cellPos);

            if (celulaAleatoria == Vector3Int.zero)
            {
                Debug.LogError("Não foi possível gerar uma célula aleatória válida.");
                yield return new WaitForSeconds(intervaloTentativa);
                continue;
            }

            Vector3 worldPosition = grid.CellToWorld(celulaAleatoria);

            if (ExisteCaminho(jogador.position, worldPosition))
            {
                // Calculando a chance de geração baseada na label
                float chanceDeGerar = CalcularChanceDeGerar(label);
                if (Random.value <= chanceDeGerar)  // A geração ocorre se a chance for cumprida
                {
                    GerarItem(worldPosition);
                    itemsGerados++;
                }
                else
                {
                    Debug.Log("Chance não cumprida, tentando novamente.");
                }

                yield return new WaitForSeconds(intervaloGeracao);
            }
            else
            {
                Debug.Log("Caminho inválido. Tentando novamente.");
                yield return new WaitForSeconds(intervaloTentativa);
            }
        }

        gerandoItens = false;
    }

    bool ExisteCaminho(Vector3 start, Vector3 end)
    {
        if (seeker == null)
        {
            Debug.LogError("Seeker não foi inicializado.");
            return false;
        }

        Path path = seeker.StartPath(start, end, null);
        AstarPath.BlockUntilCalculated(path);

        if (path.error)
        {
            Debug.LogWarning("Não há caminho até o destino.");
            return false;
        }

        return true;
    }

    Vector3Int GerarCelulaAleatoria(Vector3Int jogadorPos)
    {
        GridGraph gridGraph = astarPath.graphs[0] as GridGraph;

        if (gridGraph == null)
        {
            Debug.LogError("GridGraph não encontrado.");
            return Vector3Int.zero;
        }

        int gridWidth = gridGraph.width;
        int gridHeight = gridGraph.depth;

        int xAleatorio = Random.Range(jogadorPos.x - (int)raioGeracao, jogadorPos.x + (int)raioGeracao);
        int yAleatorio = Random.Range(jogadorPos.y - (int)raioGeracao, jogadorPos.y + (int)raioGeracao);

        if (xAleatorio < -48.188319 || xAleatorio >= 51.811681 || yAleatorio < -27.53517  || yAleatorio >= 72.46483)
        {
            return Vector3Int.zero;
        }

        return new Vector3Int(xAleatorio, yAleatorio, 0);
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

        OlhoItemCollision olhoItem = item.GetComponent<OlhoItemCollision>();
        if (olhoItem != null)
        {
            olhoItem.lightToDisable = GameObject.FindWithTag("pivotCamera").GetComponentInChildren<Light2D>();
        }
    }

    // Função para calcular a chance de gerar um item com base na label
    float CalcularChanceDeGerar(int label)
    {
        // 50% para noob (0), 30% para pro (1)
        float chance = (label == 0) ? 0.5f : 0.3f;

        return chance;
    }
}
