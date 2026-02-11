using EasyUI.Ventana;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu_managers : MonoBehaviour
{
    // Start is called before the first frame update
   

    [Header("GameObject")]
    public GameObject btnMenu;
    public GameObject menuUsuarios;
    public GameObject menucrearusuarios;
    public GameObject menuValores;
    public GameObject menudatosempresa;
    [Header("input")]
    public TextMeshProUGUI txtid_empresa;
    public TextMeshProUGUI txtnom_empresa;
    public TextMeshProUGUI input_nom_empresa;
    public TextMeshProUGUI input_hora_inicio;
    public TextMeshProUGUI input_hora_final;
    public TMP_InputField input_tiempo_repeticion;
    public TMP_InputField input_maquinas;
    public TMP_InputField input_result_time;
    public TMP_Dropdown dropscenes;
    public TextMeshProUGUI Trifa;
    public TextMeshProUGUI T24horas;
    public TextMeshProUGUI Tacumulacion;
    //public Toggle Trifa;
    //public Toggle Tacumulacion;
    [Header("Archivos")]
    public mgs_tabla_usuarios Tabla_Usuarios;
    public tabla_valores Tabla_Valores;

    void Start()
    {
        StartCoroutine(validar_datos_empresa(0));
    }
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
        StartCoroutine(validar_datos_empresa(1));
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
    IEnumerator validar_datos_empresa(int accion_vista)
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
                ventanaUI.Instance
               .SetTitle("ERROR")
               .SetMessage("Problem loading company data, please try again or contact support.")
               .SetImagen("error")
               .SetColor("#F50801")
               .Show(0);

            }
            else if (response.codigo == 200)
            {
                if (accion_vista == 0) 
                {
                    txtnom_empresa.text = response.datos.nom_empresa;
                } 
                else if (accion_vista == 1) 
                {
                    txtid_empresa.text = response.datos.id_empresa;
                    input_nom_empresa.text = response.datos.nom_empresa;
                    input_hora_inicio.text = response.datos.hora_inicial;
                    input_hora_final.text = response.datos.hora_final;
                    input_tiempo_repeticion.text = response.datos.tiempo_repeticion;
                    input_maquinas.text = response.datos.maquinas;
                    input_result_time.text = response.datos.result_time;
                    int tem_scene = 0;
                    if (response.datos.scene == "US")
                    {
                        tem_scene = 0;
                    }
                    else if (response.datos.scene == "OR")
                    {
                        tem_scene = 1;
                    }
                    else if (response.datos.scene == "JU")
                    {
                        tem_scene = 2;
                    }
                    if (response.datos.rifa == "S")
                    {
                        Trifa.text = "YES";
                    }
                    else
                    {
                        Trifa.text = "NO";
                    }

                    if (response.datos.hours_24 == "S")
                    {
                        T24horas.text = "YES";
                    }
                    else
                    {
                        T24horas.text = "NO";
                    }
                    //N = Normal - NORMAL;
                    //H = Nocturna  -- NIGHT;
                    //S = 24 horas -- 24 HOURS;
                    if (response.datos.hours_24 == "S")
                    {
                        T24horas.text = "24 HOURS";
                    }
                    else if (response.datos.hours_24 == "N")
                    {
                        T24horas.text = "NORMAL";
                    }
                    else if (response.datos.hours_24 == "H")
                    {
                        T24horas.text = "NIGHT";
                    }

                    if (response.datos.acumulacion == "S")
                    {
                        Tacumulacion.text = "YES";
                    }
                    else
                    {
                        Tacumulacion.text = "NO";
                    }
                    dropscenes.value = tem_scene;
                    btnMenu.SetActive(false);
                    menudatosempresa.SetActive(true);
                }
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

    public void editar_registros()
    {
        StartCoroutine(accion_editar());
    }
    IEnumerator accion_editar()
    {
        int valor_actual = dropscenes.value;

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
        string url = "http://localhost/unity_apis/empresa.php";
        WWWForm form = new WWWForm();
        //form.AddField("nombre", input_nom_empresa.text);
        //form.AddField("tiempo_inicial", input_hora_inicio.text);
        //form.AddField("tiempo_final", input_hora_final.text);
        form.AddField("tiempo_accion", input_tiempo_repeticion.text);
        form.AddField("num_maquinas", input_maquinas.text);
        form.AddField("result_time", input_result_time.text);
        form.AddField("id_empresa", txtid_empresa.text);
        form.AddField("scene", scena_valor);
        form.AddField("accion", "editar_datos_mng");
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
                txtnom_empresa.text = input_nom_empresa.text;
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
        }
    }
}
