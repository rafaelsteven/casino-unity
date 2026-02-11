using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;
using UnityEngine.UI;
public class img_resultados : MonoBehaviour
{
    public UnityEngine.UI.Image[] image;
    public float fillSpeed = 0.5f;
    public bool activar_img = false;
    public ejecutor_movimiento_text ejecutar_Movimiento_Texto;
    
    private void Start()
    {
        for (int i = 0; i < image.Length; i++)
        {
            image[i].fillAmount = 0;
        }
    }

    private void FixedUpdate()
    {
        if (activar_img)
        {
            for (int i = 0; i < image.Length; i++)
            {   
                image[i].fillAmount = Mathf.MoveTowards(image[i].fillAmount, 1f, fillSpeed * Time.deltaTime);
                if(image[i].fillAmount == 1)
                {
                    ejecutar_Movimiento_Texto.funcion_mover_letras();
                    activar_img = false;
                }
            }
        }
    }
}
