using EasyUI.Ventana;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class funciones_valores : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject panel_principal;
    [Header("input crear")]
    public TMP_InputField valor_input;
    public TMP_Dropdown droptipo_valor;
    public TextMeshProUGUI txtid_valor;
    public void funcion_editar_valor()
    {
        StartCoroutine(accion_crear_valores());
    }
    IEnumerator accion_crear_valores()
    {
        int tipo_valor = droptipo_valor.value;

        string tipo_valor_sql = "";
        if (tipo_valor == 0)
        {
            tipo_valor_sql = "S";
        }
        else if (tipo_valor == 1)
        {
            tipo_valor_sql = "R";
        }

        string url = "http://localhost/unity_apis/empresa.php";
        WWWForm form = new WWWForm();
        form.AddField("valor", valor_input.text);
        form.AddField("cod_valor", txtid_valor.text);
        form.AddField("tipo", tipo_valor_sql);
        form.AddField("accion", "editar_valores_premios");
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log(responseText);
            datosResponse response = JsonUtility.FromJson<datosResponse>(responseText);

            if (response.codigo == 200)
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
                //ventanaemergente.activar_ventana("ERROR", response.mensaje, 0);
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
            //ventanaemergente.activar_ventana("ERROR", request.error, 0);
            ventanaUI.Instance
            .SetTitle("ERROR")
            .SetMessage("Error with database communication. Please check if the servers are working or contact support.")
            .SetImagen("error")
            .SetColor("#F50801")
            .Show(0);
        }

    }
    public void funcion_btn_eliminar()
    {
        ventanaUI.Instance
        .SetTitle("INFORMATION")
        .SetMessage("Are you sure you want to delete the selected value? Once deleted, you will not be able to recover it.")
        .SetImagen("ayuda")
        .SetColor("#007bff")
        .SiOnclass(funcion_eliminar_valores)
        .Show(1);
    }
    public void funcion_eliminar_valores()
    {
        StartCoroutine(Eliminar_valores());
    }
    IEnumerator Eliminar_valores()
    {


        string url = "http://localhost/unity_apis/empresa.php";
        WWWForm form = new WWWForm();
        form.AddField("id_valores", txtid_valor.text);
        form.AddField("accion", "eliminar_valor_premio");

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log(responseText);
            datosResponse response = JsonUtility.FromJson<datosResponse>(responseText);

            if (response.codigo == 200)
            {
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
}
