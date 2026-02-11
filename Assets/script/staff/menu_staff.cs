using EasyUI.Ventana;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class menu_staff : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject btnMenu;
    public GameObject menuRifa;
    public GameObject menudatosempresa;
    [Header("input")]
    public TextMeshProUGUI txtnom_empresa;
    public TextMeshProUGUI input_nom_empresa;
    public TextMeshProUGUI input_maquinas;
    public TextMeshProUGUI input_hora_inicio;
    public TextMeshProUGUI input_hora_final;
    public TextMeshProUGUI input_tiempo_repeticion;
    public TextMeshProUGUI dropscenes;
    public TextMeshProUGUI Trifa;
    public TextMeshProUGUI Tacumulacion;
    public string scena_var;
    public string estado_rifa;
    //public Toggle Trifa;
    //public Toggle Tacumulacion;
    [Header("Archivos")]
    public tabla_valores Tabla_Valores;

    void Start()
    {
        StartCoroutine(validar_datos_empresa(0));
        
    }

    public void ir_crear_riffa()
    {
        if(estado_rifa == "S")
        {
            btnMenu.SetActive(false);
            menuRifa.SetActive(true);
            Tabla_Valores.datos_valores();
        }
        else
        {
            ventanaUI.Instance
              .SetTitle("ERROR")
              .SetMessage("You do not have permission to execute the raffle action, if you wish to activate the raffle option, please contact support.")
              .SetImagen("denegado")
              .SetColor("#F50801")
              .Show(0);
        }
        
    }
    public void back_crear_riffa()
    {
        btnMenu.SetActive(true);
        menuRifa.SetActive(false);
    }
    public void ir_registros()
    {
        SceneManager.LoadScene("registro");
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
    IEnumerator validar_datos_empresa(int accion_tem)
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
                if(accion_tem == 0)
                {
                    txtnom_empresa.text = response.datos.nom_empresa;
                    scena_var = response.datos.scene;
                    estado_rifa = response.datos.rifa;
                    if (rol.ROL.accion_menu == "RAFFLE")
                    {
                        ir_crear_riffa();
                        rol.ROL.accion_menu = "";
                    }
                }
                else if (accion_tem == 1)
                {
                    input_nom_empresa.text = response.datos.nom_empresa;
                    input_hora_inicio.text = response.datos.hora_inicial;
                    input_hora_final.text = response.datos.hora_final;
                    input_tiempo_repeticion.text = response.datos.tiempo_repeticion;
                    input_maquinas.text = response.datos.maquinas;
                    string tem_scene = "";
                    
                    if (response.datos.scene == "US")
                    {
                        tem_scene = "LAS VEGAS";
                    }
                    else if (response.datos.scene == "OR")
                    {
                        tem_scene = "ORIENTAL";
                    }
                    else if (response.datos.scene == "JU")
                    {
                        tem_scene = "JUNGLE";
                    }
                    if (response.datos.rifa == "S")
                    {
                        Trifa.text = "YES";
                    }
                    else
                    {
                        Trifa.text = "NO";
                    }
                    if (response.datos.acumulacion == "S")
                    {
                        Tacumulacion.text = "YES";
                    }
                    else
                    {
                        Tacumulacion.text = "NO";
                    }
                    dropscenes.text = tem_scene;
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

    public void ir_scene()
    {
        if (scena_var == "US")
        {
            SceneManager.LoadScene("newyok");
        }
        else if (scena_var == "OR")
        {
            SceneManager.LoadScene("oriente");
        }
        else if (scena_var == "JU")
        {
            SceneManager.LoadScene("selva");
        }
    }
    public void funcion_ir_login()
    {
        SceneManager.LoadScene("login");
    }

    public void funcion_ir_raffle()
    {
        SceneManager.LoadScene("raffle");
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
        }
    }
}
