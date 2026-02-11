using EasyUI.Ventana;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class activar_desactivaruser : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject activar;
    public GameObject desactivar;
    public TextMeshProUGUI txtid_user;
    public void activar_usuario()
    {
        StartCoroutine(accion_activar());
    }
    IEnumerator accion_activar()
    {
        string url = "http://localhost/unity_apis/empresa.php";
        WWWForm form = new WWWForm();
        form.AddField("cod_usuario", txtid_user.text);
        form.AddField("st_usuario", "A");
        form.AddField("accion", "ac_ds");
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
                activar.SetActive(false);
                desactivar.SetActive(true);
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
    public void desactivar_usuario()
    {
        StartCoroutine(accion_desactivar());
    }
    IEnumerator accion_desactivar()
    {
        string url = "http://localhost/unity_apis/empresa.php";
        WWWForm form = new WWWForm();
        form.AddField("cod_usuario", txtid_user.text);
        form.AddField("st_usuario", "D");
        form.AddField("accion", "ac_ds");
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
                desactivar.SetActive(false);
                activar.SetActive(true);
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
    [System.Serializable]
    public class datosResponse
    {
        public int codigo;
        public string mensaje;
    }
}
