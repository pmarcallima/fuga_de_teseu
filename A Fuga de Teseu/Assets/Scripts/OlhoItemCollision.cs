using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OlhoItemCollision : MonoBehaviour
{   
    public Light2D lightToDisable;
    public GameObject player;

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Teseu") {
            if (lightToDisable != null) {
                lightToDisable.enabled = false;
                Invoke("DisableLightTemporarily", 2f);
                
            }
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

    void DisableLightTemporarily() {
        lightToDisable.enabled = true;
    }
    
}

