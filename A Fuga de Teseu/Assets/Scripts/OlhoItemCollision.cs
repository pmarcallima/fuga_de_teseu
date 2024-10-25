using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OlhoItemCollision : MonoBehaviour
{   
    public Light2D lightToDisable;

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Teseu") {
            if (lightToDisable != null) {
                lightToDisable.enabled = false;
                Invoke("DisableLightTemporarily", 2f);
                
            }
            gameObject.SetActive(false);
        }
    }

    void DisableLightTemporarily() {
        lightToDisable.enabled = true;
    }
    
}

