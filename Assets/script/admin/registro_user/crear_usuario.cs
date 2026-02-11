using EasyUI.Ventana;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class crear_usuario : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject menutablausuario;
    public GameObject crearusuario;
    [Header("input crear")]
    public TMP_InputField nom_usuari;
    public TMP_InputField pass_usuario;
    public TMP_InputField ip_usuario;
    public TMP_Dropdown Cdroprol;
    public TMP_Dropdown Cdropstate;
    [Header("Archivos")]
    //public ventanaEmergente ventanaemergente;
    public tabla_usuarios Tabla_Usuarios;

    public void funcion_crear_usuario()
    {
        StartCoroutine(Crear_usuario());
    }
    IEnumerator Crear_usuario()
    {
        DateTime currentDateTime = DateTime.Now;
        string formattedDateTime = currentDateTime.ToString("yy-MM-dd HH:mm:ss");

        int valor_stado = Cdropstate.value;

        string stado_usuario = "";
        if (valor_stado == 0)
        {
            stado_usuario = "A";
        }
        else if (valor_stado == 1)
        {
            stado_usuario = "D";
        }

        int valor_rol = Cdroprol.value;

        string rol_usuario = "";
        if (valor_rol == 0)
        {
            rol_usuario = "STAFF";
        }
        else if (valor_rol == 1)
        {
            rol_usuario = "MGR";
        }
        string url = "http://localhost/unity_apis/empresa.php";
        WWWForm form = new WWWForm();
        form.AddField("nom_usuario", nom_usuari.text);
        form.AddField("password", pass_usuario.text);
        form.AddField("ip_us", ip_usuario.text);
        form.AddField("fecha_creacion", formattedDateTime);
        form.AddField("st_usuario", stado_usuario);
        form.AddField("st_uso", "N");
        form.AddField("rol", rol_usuario);
        form.AddField("accion", "crear_usuario");
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log(responseText);
            datosResponse response = JsonUtility.FromJson<datosResponse>(responseText);

            if (response.codigo == 200)
            {
                menutablausuario.SetActive(true);
                crearusuario.SetActive(false);
                ventanaUI.Instance
                 .SetTitle("SUCCESS")
                 .SetMessage(response.mensaje)
                 .SetImagen("listo")
                 .SetColor("#33d038")
                 .Show(0);
                Tabla_Usuarios.datos_usuarios_cargar();
            }
            else
            {
                ventanaUI.Instance
              .SetTitle("ERROR")
              .SetMessage(response.mensaje)
              .SetImagen("error")
              .SetColor("#F50801")
              .Show(0);
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
    }
}
