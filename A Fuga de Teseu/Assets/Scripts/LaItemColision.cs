using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; // Importa o namespace do A* Pathfinding Project
using UnityEngine.Rendering.Universal;

public class LaItemCollision : MonoBehaviour
{
    public Light2D lightToDisable; // Luz da lâmpada
    public Transform exitPoint; // Ponto de saída do labirinto (a saída)
    public LineRenderer pathLine; // Linha para desenhar o caminho
    private Seeker seeker; // Referência ao Seeker do A* Pathfinding
    private AIPath aiPath; // Referência ao AIPath do A* Pathfinding Project

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

            gameObject.SetActive(false); // Desativar o item após uso
        }
    }

    void DisableLightTemporarily()
    {
        lightToDisable.enabled = true;
    }

    // Callback que será chamado quando o caminho for calculado
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            ShowPath(p.vectorPath);
        }
    }

    // Função para mostrar o caminho com uma linha ou outro indicador visual
    void ShowPath(List<Vector3> path)
    {
        if (pathLine != null && path.Count > 0)
        {
            pathLine.positionCount = path.Count;
            pathLine.SetPositions(path.ToArray());

            // Desativar a linha após 5 segundos, por exemplo
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
