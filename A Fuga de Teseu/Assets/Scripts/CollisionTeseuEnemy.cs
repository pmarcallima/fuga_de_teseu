using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTeseuEnemy : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            Destroy(gameObject);
        }
    }
}
