using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionTeseuEnemy : MonoBehaviour
{
    private PlayerNumbers playerNumbers;

    void Start()
    {
        // Obtém a referência ao PlayerNumbers para gravar os dados
        GameObject playerStats = GameObject.FindGameObjectWithTag("Stats");
        if (playerStats != null)
        {
            playerNumbers = playerStats.GetComponent<PlayerNumbers>();
        }

        if (playerNumbers == null)
        {
            Debug.LogError("PlayerNumbers não encontrado no GameObject com tag 'Stats'!");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (playerNumbers != null)
            {
                // Grava os dados antes de mudar para a cena de Game Over
                playerNumbers.FinalizarFase(false);
            }

            // Destroi o objeto Teseu e carrega a cena de Game Over
            Destroy(gameObject);
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
