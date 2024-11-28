using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    private static PersistentObject instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Faz o GameObject persistir entre as cenas
        }
        else
        {
            Destroy(gameObject); // Garante que não haja duplicatas
        }
    }
}
