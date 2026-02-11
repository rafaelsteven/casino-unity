using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine.Networking;
using System.Net;
using System.Linq;
using UnityEngine.SceneManagement;
using EasyUI.Ventana;

public class login : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    private string rolsql;
    private string ipsql;
    //public ventanaEmergente ventanaemergente;

    //public LoginResponse Loginresponse;
    private string ip_pc;
    private void Start()
    {
        string hostname = Dns.GetHostName();
        ip_pc = GetSerialNumber();

    }
    public void QuitGame()
    {
       Application.Quit();
    }

    public void Login()
    {
        StartCoroutine(Login_sql());
    }
    IEnumerator Login_sql()
    {

        string url = "http://localhost/unity_apis/usuarios.php";

        WWWForm form = new WWWForm();
        form.AddField("usuario", usernameInput.text);
        form.AddField("password", passwordInput.text);
        form.AddField("accion", "login");
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log(responseText);
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(responseText);
            if (response.codigo == 400)
            {
                ventanaUI.Instance
                .SetTitle("ERROR")
                .SetMessage(response.mensaje)
                .SetImagen("error")
                .SetColor("#F50801")
                .Show(0);
            }
            else if(response.codigo == 200) 
            {
                Debug.Log(ip_pc+ "--"+ response.datos.ip);
                if (response.datos.ip == ip_pc || response.datos.rol == "ADMIN") {
                    rol.ROL.asignarRol(response.datos.rol);
                    switch (response.datos.rol)
                    {
                        case "ADMIN":
                            SceneManager.LoadScene("admin");
                            break;
                        case "STAFF":
                            SceneManager.LoadScene("staff");
                            break;
                        case "MGR":
                            SceneManager.LoadScene("mgr");
                            break;
                    }
                } else {
                    ventanaUI.Instance
                       .SetTitle("ERROR")
                       .SetMessage("Security access denied.")
                       .SetImagen("error")
                       .SetColor("#F50801")
                       .Show(0);
                }

            }
            else
            {
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
        }

    }
    string GetSerialNumber()
    {
        string serial = "";
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

        startInfo.FileName = "wmic";
        startInfo.Arguments = "bios get serialnumber";
        startInfo.RedirectStandardOutput = true;
        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = true;

        process.StartInfo = startInfo;
        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        string[] lines = output.Split('\n');
        if (lines.Length >= 2)
        {
            serial = lines[1].Trim();
        }

        return serial;
    }
    [System.Serializable]
    public class LoginResponse
    {
        public int codigo;
        public string mensaje;
        public Datos datos;

        [System.Serializable]
        public class Datos
        {
            public string ip;
            public string rol;
        }
    }
}
