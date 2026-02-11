using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using EasyUI.Ventana;
using UnityEngine.SceneManagement;

public class botonMenu : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject btnMenu;
    public GameObject menuUsuarios;
    public GameObject menucrearusuarios;
    public GameObject menuValores;
    public GameObject menudatosempresa;
    public GameObject menucreardatosempresa;
    [Header("input")]
    public TextMeshProUGUI txtid_empresa;
    public TMP_InputField input_nom_empresa;
    public TMP_InputField input_hora_inicio;
    public TMP_InputField input_hora_final;
    public TMP_InputField input_tiempo_repeticion;
    public TMP_InputField input_maquinas;
    public TMP_InputField input_result_time;
    public TMP_Dropdown dropscenes;
    public Toggle Trifa;
    public Toggle Probabilidad;
    public TMP_Dropdown Thours_24;
    public Toggle Tacumulacion;
    [Header("Archivos")]
    public tabla_usuarios Tabla_Usuarios;
    public tabla_valores Tabla_Valores;
    public void ir_crear_usuarios()
    {
        btnMenu.SetActive(false);
        menuUsuarios.SetActive(true);
        Tabla_Usuarios.datos_usuarios_cargar();
    }
    public void back_crear_usuarios()
    {
        menuUsuarios.SetActive(false);
        btnMenu.SetActive(true);
    }
    public void ir_crear_usuario_btn()
    {
        menucrearusuarios.SetActive(true);
        menuUsuarios.SetActive(false);
    }
    public void back_crear_usuario_btn()
    {
        menuUsuarios.SetActive(true);
        menucrearusuarios.SetActive(false);
    }
    public void ir_crear_valores()
    {
        btnMenu.SetActive(false);
        menuValores.SetActive(true);
        Tabla_Valores.datos_valores();
    }
    public void back_crear_valores()
    {
        btnMenu.SetActive(true);
        menuValores.SetActive(false);
    }

    public void ir_datos_empresa()
    {
        StartCoroutine(validar_datos_empresa());
    }
    public void back_credatos_empresa()
    {
        btnMenu.SetActive(true);
        menucreardatosempresa.SetActive(false);
    }
    public void back_vistadatos_empresa()
    {
        btnMenu.SetActive(true);
        menudatosempresa.SetActive(false);
    }
    public void ir_registros()
    {
        SceneManager.LoadScene("registro");
    }
    IEnumerator validar_datos_empresa()
    {

        string url = "http://localhost/unity_apis/empresa.php";

        WWWForm form = new WWWForm();
        form.AddField("accion", "datos_empresa");
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log(responseText);
            datosResponse response = JsonUtility.FromJson<datosResponse>(responseText);
            if (response.codigo == 400)
            {
                btnMenu.SetActive(false);
                menucreardatosempresa.SetActive(true);
                
            }
            else if (response.codigo == 200)
            {
                txtid_empresa.text = response.datos.id_empresa;
                input_nom_empresa.text = response.datos.nom_empresa;
                input_hora_inicio.text = response.datos.hora_inicial;
                input_hora_final.text = response.datos.hora_final;
                input_tiempo_repeticion.text = response.datos.tiempo_repeticion;
                input_maquinas.text = response.datos.maquinas;
                input_result_time.text = response.datos.result_time;
                int tem_scene = 0;
                if(response.datos.scene == "US")
                {
                    tem_scene = 0;
                }else if (response.datos.scene == "OR")
                {
                    tem_scene = 1;
                }
                else if (response.datos.scene == "JU")
                {
                    tem_scene = 2;
                }
                int tem_tipo = 0;
                //N = Normal;
                //H = Nocturna;
                //S = 24 horas;
                if (response.datos.hours_24 == "N")
                {
                    tem_tipo = 0;
                }
                else if (response.datos.hours_24 == "H")
                {
                    tem_tipo = 1;
                }
                else if (response.datos.hours_24 == "S")
                {
                    tem_tipo = 2;
                }
                Thours_24.value = tem_tipo;
                if(response.datos.rifa == "S")
                {
                    Trifa.isOn = true;
                }

                if (response.datos.st_probabilidad == "S")
                {
                    Probabilidad.isOn = true;
                }

                if (response.datos.acumulacion == "S")
                {
                    Tacumulacion.isOn = true;
                }
                dropscenes.value = tem_scene;
                btnMenu.SetActive(false);
                menudatosempresa.SetActive(true);
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
            //ventanaemergente.activar_ventana("ERROR", "Error with database communication. Please check if the servers are working or contact support.", 0);
        }

    }
    public void funcion_ir_login()
    {
        SceneManager.LoadScene("login");
    }

    [System.Serializable]
    public class datosResponse
    {
        public int codigo;
        public string mensaje;
        public Datos datos;

        [System.Serializable]
        public class Datos
        {
            public string id_empresa;
            public string nom_empresa;
            public string hora_inicial;
            public string hora_final;
            public string tiempo_repeticion;
            public string rifa;
            public string acumulacion;
            public string maquinas;
            public string scene;
            public string hours_24;
            public string result_time;
            public string st_probabilidad;
        }
    }
}
