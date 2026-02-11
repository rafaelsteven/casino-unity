using EasyUI.Ventana;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class tabla_valores : MonoBehaviour
{
    public GameObject datosValores;
    public GameObject error_objeto;
    public void datos_valores()
    {
        StartCoroutine(accion_datos_valores());
    }

    IEnumerator accion_datos_valores()
    {
        //GameObject objetoDatosUsuario = transform.GetChild(0).gameObject;
        //GameObject objetoPrincipal = Instantiate(Resources.Load<GameObject>("objetos"));
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        string url = "http://localhost/unity_apis/empresa.php";

        WWWForm form = new WWWForm();
        string validacion_toles = rol.ROL.tipoRol;
        if (validacion_toles == "ADMIN" || validacion_toles == "MGR")
        {
            form.AddField("accion", "datos_total_valores");
        }
        else if (validacion_toles == "STAFF")
        {
            form.AddField("accion", "datos_total_valores_raffle");
        }
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log(responseText);
            datosResponse response = JsonUtility.FromJson<datosResponse>(responseText);
            if (response.codigo == 400)
            {
                GameObject g = Instantiate(error_objeto, transform);

            }
            else if (response.codigo == 200)
            {
                foreach (var dato_arry in response.datos)
                {

                    GameObject g = Instantiate(datosValores, transform);
                    //usuario
                    g.transform.Find("Inputvalues").GetComponent<TMP_InputField>().text = dato_arry.valor_premio;
                    //password
                    int tipo_valor = 0;
                    if(dato_arry.tipo == "S")
                    {
                        tipo_valor = 0;
                    }
                    else if (dato_arry.tipo == "R")
                    {
                        tipo_valor = 1;
                    }
                    g.transform.Find("Droptype").GetComponent<TMP_Dropdown>().value = tipo_valor;
                    g.transform.Find("id").GetComponent<TextMeshProUGUI>().text = dato_arry.id_valores;
                }
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
            public string id_valores;
            public string valor_premio;
            public string tipo;
        }
    }
}
