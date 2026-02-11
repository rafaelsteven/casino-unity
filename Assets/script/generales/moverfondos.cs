using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moverfondos : MonoBehaviour
{
    public Vector2 velocidadMovimiento;
    public Vector2 offset;
    public Material material;
    void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        offset = velocidadMovimiento * Time.deltaTime;
        material.mainTextureOffset += offset;
    }
}
