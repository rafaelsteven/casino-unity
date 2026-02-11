using EasyUI.Ventana;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class datos_empresa : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject MenuCrearDatosEmpresa;
    public GameObject MenuVistaDatosEmpresa;
    [Header("input crear")]
    public TMP_InputField Cinput_nom_empresa;
    public TMP_InputField Cinput_hora_inicio;
    public TMP_InputField Cinput_hora_final;
    public TMP_InputField Cinput_tiempo_repeticion;
    public TMP_InputField Cinput_maquinas;
    public TMP_InputField Cinput_resulttime;
    public TMP_Dropdown Cdropscenes;
    public Toggle CProbabilidad;
    public Toggle CTrifa;
    public TMP_Dropdown C24HORAS;
    public Toggle CTacumulacion;

    [Header("input vista")]
    public TextMeshProUGUI txtid_empresa;
    public TMP_InputField Vinput_nom_empresa;
    public TMP_InputField Vinput_hora_inicio;
    public TMP_InputField Vinput_hora_final;
    public TMP_InputField Vinput_tiempo_repeticion;
    public TMP_InputField Vinput_maquinas;
    public TMP_InputField Vinput_resulttime;
    public TMP_Dropdown Vdropscenes;
    public Toggle VTrifa;
    public Toggle VProbabilidad;
    public TMP_Dropdown V24HORAS;
    public Toggle VTacumulacion;

    public ventanaEmergente ventanaemergente;
    public botonMenu BotonMenu;
    public void crear_datos_empresa()
    {
        StartCoroutine(validar_datos_empresa());
    }
    IEnumerator validar_datos_empresa()
    {

        string url = "http://localhost/unity_apis/empresa.php";

        int valor_actual = Cdropscenes.value;

        string scena_valor = "";
        if (valor_actual == 0)
        {
            scena_valor = "US";
        }
        else if (valor_actual == 1)
        {
            scena_valor = "OR";
        }
        else if (valor_actual == 2)
        {
            scena_valor = "JU";
        }

        string valor_acumulacion = "N";
        if(CTacumulacion.isOn == true)
        {
            valor_acumulacion = "S";
        }

        string valor_rifa = "N";
        if (CTrifa.isOn == true)
        {
            valor_rifa = "S";
        }

        string valor_probabilidad = "N";
        if (CProbabilidad.isOn == true)
        {
            valor_probabilidad = "S";
        }

        string valor_24horas = "";
        int valor_actual_tipo = V24HORAS.value;
        //N = Normal;
        //H = Nocturna;
        //S = 24 horas;
        if (valor_actual_tipo == 0)
        {
            valor_24horas = "N";
        }
        else if (valor_actual_tipo == 1)
        {
            valor_24horas = "H";
        }
        else if (valor_actual_tipo == 2)
        {
            valor_24horas = "S";
        }
        Debug.Log(valor_24horas);
        DateTime currentDateTime = DateTime.Now;
        string formattedDateTime = currentDateTime.ToString("yy-MM-dd HH:mm:ss");
        WWWForm form = new WWWForm();
        form.AddField("nombre", Cinput_nom_empresa.text);
        form.AddField("tiempo_inicial", Cinput_hora_inicio.text);
        form.AddField("tiempo_final", Cinput_hora_final.text);
        form.AddField("tiempo_accion", Cinput_tiempo_repeticion.text);
        form.AddField("num_maquinas", Cinput_maquinas.text);
        form.AddField("result_time", Cinput_resulttime.text);

        form.AddField("fecha_creacion", formattedDateTime);
        form.AddField("scene", scena_valor);
        form.AddField("st_probabilidad", valor_probabilidad);
        form.AddField("rifa", valor_rifa);
        form.AddField("horas24", valor_24horas);
        form.AddField("acumulacion", valor_acumulacion);
        form.AddField("accion", "crear");
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
                .SetImagen("listo")
                .SetColor("#F50801")
                .Show(0);

            }
            else if (response.codigo == 200)
            {
                ventanaUI.Instance
                .SetTitle("SUCCESS")
                .SetMessage(response.mensaje)
                .SetImagen("listo")
                .SetColor("#33d038")
                .Show(0);
                BotonMenu.ir_datos_empresa();
                MenuCrearDatosEmpresa.SetActive(false);
                MenuVistaDatosEmpresa.SetActive(true);
            }
            else
            {
                ventanaUI.Instance
               .SetTitle("ERROR")
               .SetMessage(response.mensaje)
               .SetImagen("listo")
               .SetColor("#F50801")
               .Show(0);
                Debug.LogError(response.mensaje);
            }
        }
        else
        {
            //ventanaemergente.activar_ventana("ERROR", request.error, 0);
            ventanaUI.Instance
            .SetTitle("ERROR")
            .SetMessage("Error with database communication. Please check if the servers are working or contact support.")
            .SetImagen("listo")
            .SetColor("#F50801")
            .Show(0);
        }

    }
    public void editar_datos_empresa()
    {
        StartCoroutine(ejecutar_editar_datos_empresa());
    }
    IEnumerator ejecutar_editar_datos_empresa()
    {

        string url = "http://localhost/unity_apis/empresa.php";

        int valor_actual = Vdropscenes.value;

        string scena_valor = "";
        if (valor_actual == 0)
        {
            scena_valor = "US";
        }
        else if (valor_actual == 1)
        {
            scena_valor = "OR";
        }
        else if (valor_actual == 2)
        {
            scena_valor = "JU";
        }

        string valor_acumulacion = "N";
        if (VTacumulacion.isOn == true)
        {
            valor_acumulacion = "S";
        }

        string valor_rifa = "N";
        if (VTrifa.isOn == true)
        {
            valor_rifa = "S";
        }
        string valor_prob = "N";
        if (VProbabilidad.isOn == true)
        {
            valor_prob = "S";
        }
        string valor_24HORAS = "";
        int valor_actual_tipo = V24HORAS.value;
        //N = Normal;
        //H = Nocturna;
        //S = 24 horas;
        if (valor_actual_tipo == 0)
        {
            valor_24HORAS = "N";
        }
        else if (valor_actual_tipo == 1)
        {
            valor_24HORAS = "H";
        }
        else if (valor_actual_tipo == 2)
        {
            valor_24HORAS = "S";
        }



        Debug.Log(valor_24HORAS);
        WWWForm form = new WWWForm();
        form.AddField("nombre", Vinput_nom_empresa.text);
        form.AddField("tiempo_inicial", Vinput_hora_inicio.text);
        form.AddField("tiempo_final", Vinput_hora_final.text);
        form.AddField("tiempo_accion", Vinput_tiempo_repeticion.text);
        form.AddField("num_maquinas", Vinput_maquinas.text);
        form.AddField("result_time", Vinput_resulttime.text);
        form.AddField("id_empresa", txtid_empresa.text);
        form.AddField("scene", scena_valor);
        form.AddField("rifa", valor_rifa);
        form.AddField("hours_24", valor_24HORAS);
        form.AddField("acumulacion", valor_acumulacion);
        form.AddField("st_probabilidad", valor_prob);
        form.AddField("accion", "editar_datos_em");
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
    [System.Serializable]
    public class datosResponse
    {
        public int codigo;
        public string mensaje;
    }
}
