using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mover_spliter : MonoBehaviour
{
    public float speed = 1f;
    public float archo = 10f;
    public float startPosition;
    void Start()
    {
        startPosition = transform.position.x;
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x >= archo) // Cambia el valor "10f" por el ancho máximo de tu escena
        {
            transform.position = new Vector3(-archo, transform.position.y, transform.position.z); // Cambia el valor "-10f" por el ancho negativo de tu escena
        }
    }
}
