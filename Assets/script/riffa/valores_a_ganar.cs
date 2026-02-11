using EasyUI.Ventana;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class valores_a_ganar : MonoBehaviour
{
    public GameObject datosValores;
    public void datos_valores( string tipo)
    {
        StartCoroutine(accion_datos_valores(tipo));
    }

    IEnumerator accion_datos_valores(string tipo)
    {
        //GameObject objetoDatosUsuario = transform.GetChild(0).gameObject;
        //GameObject objetoPrincipal = Instantiate(Resources.Load<GameObject>("objetos"));
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        string url = "http://localhost/unity_apis/empresa.php";

        WWWForm form = new WWWForm();
        form.AddField("tipo", tipo);
        form.AddField("accion", "valores_premios");

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log(responseText);
            datosResponse response = JsonUtility.FromJson<datosResponse>(responseText);
            if (response.codigo == 400)
            {
                ventanaUI.Instance
                .SetTitle("ERROR")
                .SetMessage(response.mensaje)
                .SetImagen("error")
                .SetColor("#F50801")
                .Show(0);

            }
            else if (response.codigo == 200)
            {
                int temo_con = 0;
                foreach (var dato_arry in response.datos)
                {
                    temo_con++;
                    GameObject g = Instantiate(datosValores, transform);
                    //usuario
                    g.transform.Find("num").GetComponent<TextMeshProUGUI>().text = "#"+temo_con.ToString();
                    g.transform.Find("valor").GetComponent<TextMeshProUGUI>().text = "$"+dato_arry.valor_premio;
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
            public string valor_premio;

        }
    }
}
