using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLineMovement : MonoBehaviour
{
    public float targetY = 14.6f;   // Primeira posição Y alvo
    public float targetX = 30.2f;   // Posição X alvo
    public float finalY = -20.5f;   // Segunda posição Y alvo (após alcançar targetX)
    public float lastX = -5.8f;     // Posição X final após alcançar finalY
    public float finalYTarget = 8f; // Nova posição Y após alcançar lastX
    public float speed = 2f;        // Velocidade de movimento
    public float tolerance = 0.01f; // Tolerância para considerar que chegou ao alvo

    // Definição dos estados de movimento
    private enum MovementState { MovingToTargetY, MovingToTargetX, MovingToFinalY, MovingToLastX, MovingToFinalYTarget, Idle }
    private MovementState currentState = MovementState.MovingToTargetY; // Estado inicial

    private void Update()
    {
        Vector3 position = transform.position;

        // Controla o movimento com base no estado atual
        switch (currentState)
        {
            case MovementState.MovingToTargetY:
                // Movimenta o personagem na direção Y até chegar em targetY
                if (Mathf.Abs(position.y - targetY) > tolerance)
                {
                    float step = speed * Time.deltaTime;
                    position.y = Mathf.MoveTowards(position.y, targetY, step);
                }
                else
                {
                    position.y = targetY;
                    currentState = MovementState.MovingToTargetX; // Transição para o próximo estado
                }
                break;

            case MovementState.MovingToTargetX:
                // Move na direção X até chegar em targetX
                if (Mathf.Abs(position.x - targetX) > tolerance)
                {
                    float step = speed * Time.deltaTime;
                    position.x = Mathf.MoveTowards(position.x, targetX, step);
                }
                else
                {
                    position.x = targetX;
                    currentState = MovementState.MovingToFinalY; // Transição para o próximo estado
                }
                break;

            case MovementState.MovingToFinalY:
                // Move na direção Y até chegar em finalY (descendo)
                if (Mathf.Abs(position.y - finalY) > tolerance)
                {
                    float step = speed * Time.deltaTime;
                    position.y = Mathf.MoveTowards(position.y, finalY, step);
                }
                else
                {
                    position.y = finalY;
                    currentState = MovementState.MovingToLastX; // Transição para o próximo estado
                }
                break;

            case MovementState.MovingToLastX:
                // Move na direção X para a posição lastX após chegar em finalY
                if (Mathf.Abs(position.x - lastX) > tolerance)
                {
                    float step = speed * Time.deltaTime;
                    position.x = Mathf.MoveTowards(position.x, lastX, step);
                }
                else
                {
                    position.x = lastX;
                    currentState = MovementState.MovingToFinalYTarget; // Transição para o próximo estado
                }
                break;

            case MovementState.MovingToFinalYTarget:
                // Move na direção Y para a posição finalYTarget após alcançar lastX
                if (Mathf.Abs(position.y - finalYTarget) > tolerance)
                {
                    float step = speed * Time.deltaTime;
                    position.y = Mathf.MoveTowards(position.y, finalYTarget, step);
                }
                else
                {
                    position.y = finalYTarget;
                    currentState = MovementState.Idle; // Chegou ao destino final
                }
                break;

            case MovementState.Idle:
                // Estado ocioso, nada acontece
                break;
        }

        // Atualiza a posição do transform
        transform.position = position;
    }
}
