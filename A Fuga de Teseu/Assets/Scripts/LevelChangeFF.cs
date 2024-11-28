using UnityEngine;
using UnityEngine.SceneManagement; // Importa o SceneManager para troca de cenas

public class LevelChangeFF : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o objeto colidido é o "invisibleExit"
        if (collision.gameObject.name == "invisibleExit")
        {
            // Troca a cena para o final
            SceneManager.LoadScene("Final-Saida");
        }
    }
}
