using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using EasyUI.Ventana;
using UnityEngine.Networking;

public class crear_valores : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject menutablavalores;
    public GameObject crearvalores;
    [Header("input crear")]
    public TMP_InputField valor_input;
    public TMP_Dropdown droptipo_valor;
    [Header("Archivos")]
    public tabla_valores Tabla_Valores;

    public void abrir_crear_valores()
    {
        menutablavalores.SetActive(false);
        crearvalores.SetActive(true);
    }
    public void cerrar_crear_valores()
    {
        crearvalores.SetActive(false);
        menutablavalores.SetActive(true);
        Tabla_Valores.datos_valores();
    }

    public void funcion_crear_valores()
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
        form.AddField("valor_premio", valor_input.text);
        form.AddField("tipo", tipo_valor_sql);
        form.AddField("accion", "crear_valor_premio");
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log(responseText);
            datosResponse response = JsonUtility.FromJson<datosResponse>(responseText);

            if (response.codigo == 200)
            {
                valor_input.text = "";
                ventanaUI.Instance
                .SetTitle("SUCCESS")
                .SetMessage("the entered value has been entered correctly. do you want to continue entering values?.")
                .SetImagen("listo")
                .SetColor("#33d038")
                .NoOnclass(cerrar_crear_valores)
                .Show(1);

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
    [System.Serializable]
    public class datosResponse
    {
        public int codigo;
        public string mensaje;
    }
}
