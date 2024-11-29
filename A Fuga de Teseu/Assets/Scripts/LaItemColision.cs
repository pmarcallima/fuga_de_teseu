using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; // Importa o namespace do A* Pathfinding Project
using UnityEngine.Rendering.Universal;

public class LaItemCollision : MonoBehaviour
{
    public Light2D lightToDisable; // Luz da l�mpada
    public Transform exitPoint; // Ponto de sa�da do labirinto (a sa�da)
    public LineRenderer pathLine; // Linha para desenhar o caminho
    private Seeker seeker; // Refer�ncia ao Seeker do A* Pathfinding
    private AIPath aiPath; // Refer�ncia ao AIPath do A* Pathfinding Project
    public GameObject player;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();
    }

    void OnCollisionEnter2D(Collision2D collision)
{
if (collision.gameObject.tag == "Teseu")
{
    if (lightToDisable != null)
    {
        lightToDisable.enabled = false;
        Invoke("DisableLightTemporarily", 2f);
    }

    // Iniciar o cálculo do caminho até a saída
    seeker.StartPath(collision.transform.position, exitPoint.position, OnPathComplete);
       player = GameObject.FindWithTag("Stats");
 	PlayerNumbers playerNumbers = player.GetComponent<PlayerNumbers>();

        if (playerNumbers != null)
        {
            // Chama o método ColetarItem do script PlayerNumbers
            playerNumbers.ColetarItem();
            Debug.Log("Item atualizado no PlayerNumbers!");
        }
        else
        {
            Debug.LogError("PlayerNumbers não encontrado no GameObject especificado!");
        }

    
    gameObject.SetActive(false); 
}
}

    void DisableLightTemporarily()
    {
        lightToDisable.enabled = true;
    }

    // Callback que ser� chamado quando o caminho for calculado
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            ShowPath(p.vectorPath);
        }
    }

    // Fun��o para mostrar o caminho com uma linha ou outro indicador visual
    void ShowPath(List<Vector3> path)
    {
        if (pathLine != null && path.Count > 0)
        {
            pathLine.positionCount = path.Count;
            pathLine.SetPositions(path.ToArray());

            // Desativar a linha ap�s 5 segundos, por exemplo
            Invoke("ClearPath", 5f);
        }
    }

    void ClearPath()
    {
        if (pathLine != null)
        {
            pathLine.positionCount = 0;
        }
    }
}
