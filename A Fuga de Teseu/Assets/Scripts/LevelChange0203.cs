using UnityEngine;
using UnityEngine.SceneManagement; // Importa o SceneManager para troca de cenas

public class LevelChange0203 : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o objeto colidido é o "invisibleExit"
        if (collision.gameObject.name == "invisibleExit")
        {
            // Troca a cena para "Labirinto-03"
            SceneManager.LoadScene("Menu");
        }
    }
}
