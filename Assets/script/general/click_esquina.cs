using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class click_esquina : MonoBehaviour
{
    private int click = 0; // Umbral de tiempo para detectar un doble clic
    private float clickTime; // Tiempo del último clic
    bool activar_tiempo = false;
    public funciones_scenas_principales funciones_Scenas_Principales;
    private void Start()
    {
        transform.SetAsLastSibling();
    }
    private void OnMouseDown()
    {
        click++;
        if (click >= 2)
        {
            // Doble clic detectado
            switch (rol.ROL.tipoRol)
            {
                case "ADMIN":
                    SceneManager.LoadScene("admin");
                    break;
                case "STAFF":
                    if (funciones_Scenas_Principales != null)
                    {
                        sumar_temp_tiempo.Instance.remplazar_tiempo(funciones_Scenas_Principales.tiempo);
                        sumar_temp_tiempo.Instance.cambiar_estado_tiempo(1);
                    }
                    
                    SceneManager.LoadScene("staff");
                    break;
                case "MGR":
                    SceneManager.LoadScene("mgr");
                    break;
            }
        }
        clickTime = 0;
        activar_tiempo = true;
    }

    private void FixedUpdate()
    {
        if (activar_tiempo)
        {
            clickTime += Time.deltaTime;
            if(clickTime > 5)
            {
                click = 0;
                activar_tiempo = false;
            }

        }
    }
}
