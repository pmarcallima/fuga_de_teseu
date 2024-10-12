using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform teseu;

    private void FixedUpdate() {
        transform.position = Vector2.Lerp(transform.position, teseu.position, 0.5f);
    }
}