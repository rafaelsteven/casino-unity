using EasyUI.Ventana;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class mgs_tabla_usuarios : MonoBehaviour
{
    //public ventanaEmergente ventanaemergente;
    public GameObject datosUsuario;

    public void datos_usuarios_cargar()
    {
        StartCoroutine(accion_datos_usuarios_cargar());
    }

    IEnumerator accion_datos_usuarios_cargar()
    {
        //GameObject objetoDatosUsuario = transform.GetChild(0).gameObject;
        //GameObject objetoPrincipal = Instantiate(Resources.Load<GameObject>("objetos"));
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        string url = "http://localhost/unity_apis/empresa.php";

        WWWForm form = new WWWForm();
        form.AddField("accion", "datos_usuario_managers");
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log(responseText);
            datosResponse response = JsonUtility.FromJson<datosResponse>(responseText);
            if (response.codigo == 400)
            {
                //ventanaemergente.activar_ventana("ERROR", response.mensaje, 0);

            }
            else if (response.codigo == 200)
            {
                foreach (var dato_arry in response.datos)
                {

                    GameObject g = Instantiate(datosUsuario, transform);
                    //usuario
                    g.transform.Find("username").GetComponent<TextMeshProUGUI>().text = dato_arry.usuario;
                    //password
                    g.transform.Find("Password").GetComponent<TextMeshProUGUI>().text = dato_arry.password;
                    g.transform.Find("userip").GetComponent<TextMeshProUGUI>().text = dato_arry.ip;
                    g.transform.Find("rol").GetComponent<TextMeshProUGUI>().text = dato_arry.rol;
                    g.transform.Find("Status").GetComponent<TextMeshProUGUI>().text = dato_arry.st_usuario;
                    g.transform.Find("id").GetComponent<TextMeshProUGUI>().text = dato_arry.cod_usuario;
                    if (dato_arry.st_usuario == "A")
                    {
                        g.transform.Find("Disable").gameObject.SetActive(true);
                        g.transform.Find("Activate").gameObject.SetActive(false);
                    }
                    else
                    {
                        g.transform.Find("Activate").gameObject.SetActive(true); // Ocultar el botón
                        g.transform.Find("Disable").gameObject.SetActive(false);
                    }
                    Debug.Log(dato_arry.usuario);
                }
                //Destroy(datosUsuario);
            }
            else
            {
                //ventanaemergente.activar_ventana("ERROR", response.mensaje, 0);
                Debug.LogError(response.mensaje);
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
            public string cod_usuario;
            public string usuario;
            public string password;
            public string rol;
            public string st_usuario;
            public string ip;
        }
    }
}
