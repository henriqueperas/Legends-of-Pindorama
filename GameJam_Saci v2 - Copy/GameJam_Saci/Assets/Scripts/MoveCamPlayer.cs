using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamPlayer : MonoBehaviour
{

    public Transform player;  
    public float smoothSpeed = 1f;  
    public Vector2 offset; 

    void LateUpdate()
    {
      
        if (player != null)
        {
            // Posição desejada da câmera
            Vector2 desiredPosition = (Vector2)player.position + offset;
            // Suaviza o movimento da câmera
            Vector2 smoothedPosition = Vector2.Lerp((Vector2)transform.position, desiredPosition, smoothSpeed);
            // Atualiza a posição da câmera
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }
    }
}


