using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using EasyUI.Ventana;

public class editar_eliminar_user : MonoBehaviour
{
    public GameObject panel_principal;
    public GameObject panel_editar;
    [Header("datos txt")]
    public TextMeshProUGUI txtid;
    public TextMeshProUGUI txtnom_usuari;
    public TextMeshProUGUI txtpass_usuario;
    public TextMeshProUGUI txtip_usuario;
    public TextMeshProUGUI txtdroprol;
    public TextMeshProUGUI txtdropstate;
    [Header("input crear")]
    public TMP_InputField Inom_usuari;
    public TMP_InputField Ipass_usuario;
    public TMP_InputField Iip_usuario;
    public TMP_Dropdown Idropstate;

    public void activar_edicio()
    {
        panel_editar.SetActive(true);
        Inom_usuari.text = txtnom_usuari.text;
        Ipass_usuario.text = txtpass_usuario.text;
        Iip_usuario.text = txtip_usuario.text;

        int tem_estado = 0;
        if (txtdropstate.text == "A")
        {
            tem_estado = 0;
        }
        else if (txtdropstate.text == "D")
        {
            tem_estado = 1;
        }

        Idropstate.value = tem_estado;
    }

    public void funcion_btn_eliminar()
    {
        ventanaUI.Instance
        .SetTitle("INFORMATION")
        .SetMessage("Are you sure you want to delete the selected user? Once deleted, you will not be able to recover it.")
        .SetImagen("ayuda")
        .SetColor("#007bff")
        .SiOnclass(funcion_eliminar_usuario)
        .Show(1);
    }
    public void funcion_eliminar_usuario()
    {
        StartCoroutine(Eliminar_usuario());
    }
    public void funcion_crear_usuario()
    {
        StartCoroutine(editar_usuario());
    }
    IEnumerator editar_usuario()
    {
        int valor_stado = Idropstate.value;

        string stado_usuario = "";
        if (valor_stado == 0)
        {
            stado_usuario = "A";
        }
        else if (valor_stado == 1)
        {
            stado_usuario = "D";
        }

       /* int valor_rol = Idroprol.value;

        string rol_usuario = "";
        if (valor_rol == 0)
        {
            rol_usuario = "STAFF";
        }
        else if (valor_rol == 1)
        {
            rol_usuario = "MANAG";
        }*/
        string url = "http://localhost/unity_apis/empresa.php";
        WWWForm form = new WWWForm();
        form.AddField("id_usuario", txtid.text);
        form.AddField("nom_usuario", Inom_usuari.text);
        form.AddField("password", Ipass_usuario.text);
        form.AddField("ip_usuario", Iip_usuario.text);
        form.AddField("st_usuario", stado_usuario);
        //form.AddField("rol", rol_usuario);
        form.AddField("accion", "editar_usuario");

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log(responseText);
            datosResponse response = JsonUtility.FromJson<datosResponse>(responseText);

            if (response.codigo == 200)
            {
                panel_editar.SetActive(false);


                txtnom_usuari.text = Inom_usuari.text;
                txtpass_usuario.text = Ipass_usuario.text;
                txtip_usuario.text = Iip_usuario.text;
                txtdropstate.text = stado_usuario;

                ventanaUI.Instance
                .SetTitle("SUCCESS")
                .SetMessage(response.mensaje)
                .SetImagen("listo")
                .SetColor("#33d038")
                .Show(0);
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
    IEnumerator Eliminar_usuario()
    {


        string url = "http://localhost/unity_apis/empresa.php";
        WWWForm form = new WWWForm();
        form.AddField("cod_usuario", txtid.text);
        form.AddField("accion", "eliminar_usuario");

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log(responseText);
            datosResponse response = JsonUtility.FromJson<datosResponse>(responseText);

            if (response.codigo == 200)
            {
                panel_editar.SetActive(false);
                ventanaUI.Instance
                .SetTitle("SUCCESS")
                .SetMessage(response.mensaje)
                .SetImagen("listo")
                .SetColor("#33d038")
                .Show(0);
                Destroy(panel_principal);
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
    public void back_edicio()
    {
        panel_editar.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
