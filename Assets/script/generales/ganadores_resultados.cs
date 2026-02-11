using EasyUI.Ventana;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ganadores_resultados : MonoBehaviour
{
    public GameObject datosValores;
    List<GameObject> componentesCreados = new List<GameObject>();
    public GameObject contenedor;
    public GameObject panel_tabla_vista;
    public GameObject[] panel_tabla_ocultar;
    public float tiempo_vista = 10f;
    string accion_estado = "M";
    public funciones_scenas_principales funciones_Scenas_Principales;
    public float tiempo = 0;

    public void FixedUpdate()
    {
        float tiempo_pre_vista =  funciones_Scenas_Principales.tiempo_tabla_ganadores;
        if(tiempo_pre_vista > 0)
        {
            tiempo += Time.deltaTime;
            if(tiempo > tiempo_pre_vista && accion_estado == "M")
            {
                datos_valores();
                tiempo = 0;
                accion_estado = "V";
            }
            else if (tiempo > tiempo_vista && accion_estado == "V")
            {
                oculpar_mostrar(2);
                tiempo = 0;
                accion_estado = "M";
            }
            
        }

    }
    void oculpar_mostrar(int accion)
    {
        if(accion == 1)
        {
            for(int i = 0; i < panel_tabla_ocultar.Length; i++)
            {
                panel_tabla_ocultar[i].SetActive(false);
            }
            panel_tabla_vista.SetActive(true);
            Debug.Log("33");
        } else if (accion == 2)
        {
            panel_tabla_vista.SetActive(false);
            for (int i = 0; i < panel_tabla_ocultar.Length; i++)
            {
                panel_tabla_ocultar[i].SetActive(true);
            }
            
        }
    }
    public void datos_valores()
    {
        StartCoroutine(accion_datos_valores());
    }

    IEnumerator accion_datos_valores()
    {
        Debug.Log("22");
        foreach (Transform child in contenedor.transform)
        {
            Destroy(child.gameObject);
        }
        string url = "http://localhost/unity_apis/empresa.php";

        WWWForm form = new WWWForm();
        form.AddField("tipo", "S");
        form.AddField("estado", "G");
        form.AddField("cantidad", "4");
        form.AddField("accion", "ultima_actividad_multi");
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log(responseText);
            datosResponse response = JsonUtility.FromJson<datosResponse>(responseText);
            if (response.codigo == 400)
            {
               

            }
            else if (response.codigo == 200)
            {
                oculpar_mostrar(1);
                foreach (var dato_arry in response.datos)
                {
                    GameObject g = Instantiate(datosValores, transform);
                    g.transform.Find("valor").GetComponent<TextMeshProUGUI>().text = "$" + dato_arry.valor_acumulado_actual;

                    g.transform.Find("ganador").GetComponent<TextMeshProUGUI>().text = dato_arry.num_ganador;

                    // Se agrega el GameObject creado a la lista
                    g.transform.SetParent(contenedor.transform); 
                }

                // Se agrega cada GameObject de la lista al contenedor deseado
                /*foreach (GameObject componente in componentesCreados)
                {
                    componente.transform.SetParent(contenedor.transform);
                }*/
                //Destroy(datosUsuario);
            }
            else
            {
                ventanaUI.Instance
                .SetTitle("ERROR")
                .SetMessage(response.mensaje)
                .SetImagen("error")
                .SetColor("#F50801")
                .Show(0);
            }
        }
        else
        {
            ventanaUI.Instance
            .SetTitle("ERROR")
            .SetMessage("Error with database communication. Please check if the servers are working or contact support.")
            .SetImagen("error")
            .SetColor("#F50801")
            .Show(0);
        }

    }

    [System.Serializable]
    public class datosResponse
    {
        public int codigo;
        public string mensaje;
        public Datos[] datos;

        [System.Serializable]
        public class Datos
        {
            public string id_registro;
            public string valor_ganado;
            public string valor_acumulado_ant;
            public string num_ganador;
            public string valor_acumulado_actual;
            public string tipo_registro;
            public string datoextra1;
            public string datoextra2;
            public string fecha_creacion;
        }
    }
}
