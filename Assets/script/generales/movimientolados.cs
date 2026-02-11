using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimientolados : MonoBehaviour
{
    public GameObject objetosMover;
    public float anguloRotacion;
    public float valorangulo;
    public float limite;
    public bool entradaReg = true;
    public bool entradaLef = true;
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        valorangulo = objetosMover.transform.rotation.z;
        if (valorangulo < limite && entradaReg == true)
        {
            entradaLef = false;
            objetosMover.transform.Rotate(anguloRotacion * Vector3.forward, Space.World);
        }
        else {

            entradaLef = true;
        }

        if (valorangulo > -limite && entradaLef == true)
        {
            entradaReg = false;
            float temangular = -1 * anguloRotacion;
            objetosMover.transform.Rotate(temangular * Vector3.forward, Space.World);
        }
        else
        {
            entradaReg = true;
        }
        
        
    }
}
