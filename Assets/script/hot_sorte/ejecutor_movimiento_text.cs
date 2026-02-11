using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ejecutor_movimiento_text : MonoBehaviour
{
    public movimiento_letras movimiento_LetrasD1;

    public movimiento_letras movimiento_LetrasD2;

    public movimiento_letras movimiento_LetrasZ1;

    public movimiento_letras movimiento_LetrasZ2;

    public funcion_sorteo Funcion_Sorteo;

    float tiempo = 0;
    bool activar_tiempo = false;
    public void funcion_mover_letras()
    {
       movimiento_LetrasD1.activar_movimiento = true;
       movimiento_LetrasD2.activar_movimiento = true;
       movimiento_LetrasZ1.activar_movimiento = true;
       movimiento_LetrasZ2.activar_movimiento = true;
       activar_tiempo = true;
    }
    private void Update()
    {
        if (activar_tiempo)
        {
            tiempo += Time.deltaTime;
            if(tiempo > 20)
            {
                Funcion_Sorteo.iniciar_scenas();
                activar_tiempo = false;
            }
        }
    }
}
