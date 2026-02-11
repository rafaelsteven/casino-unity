using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimientoPuntosVueltas : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private Transform[] puntosMovimientos;
    [SerializeField] private float distancia;

    private int numeromovimiento = 0;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Girar();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, puntosMovimientos[numeromovimiento].position, velocidad * Time.deltaTime);
        if(Vector2.Distance(transform.position,puntosMovimientos[numeromovimiento].position) < distancia)
        {
            numeromovimiento++;
            if(numeromovimiento >= puntosMovimientos.Length)
            {
                numeromovimiento = 0;
            }
            Girar();
        }
    }
    private void Girar()
    {
        if(transform.position.x < puntosMovimientos[numeromovimiento].position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
}
