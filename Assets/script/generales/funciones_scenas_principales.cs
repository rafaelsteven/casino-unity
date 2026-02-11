using EasyUI.Ventana;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class funciones_scenas_principales : MonoBehaviour
{
    [Header("Variables")]
    public float tiempo_tabla_ganadores = 30;

	private string horas24_estado;

	[SerializeField]
	private Action<string> _createItemsCallback;

	[SerializeField]
	private Action<string> callback2;

	[SerializeField]
	private string tiempo_inicial;

	[SerializeField]
	private string tiempo_final;

	[SerializeField]
	private int tiempo_sorteo;

	public bool st_tiempo;

	private bool datos_ingresados;

	public float tiempo;

	public float validacionTiempoInicio;

	public bool boolEjecucionHot = false;

	private string[] datos_time_sorteo;

	private string[] datos_time_sorteo_2;

	public float velocidad_tiempo = 1f;

	[Header("texto")]
	public TextMeshProUGUI valor_acumulado_txt;

	[Header("variables")]
	private Action<string> _createItemsCallbackeliminar;

	public int valor_acumulado_ant;

	public int valor_acumulado_nuevo;

	public string estado_acumulador;

	public float valor_subida_scene;

	private string st_ultimo_registro = "";

	private bool aumento_numero;

	[Header("efectos")]
	private bool inicio_efectos = true;

	private bool finalizar_efectos;

	public AudioSource sonido_numero;

	public GameObject GameObject_efectos;
	public void remplazar_tiempo(float tiempo_var)
	{
		tiempo = tiempo_var;
	}

	public void cambiar_estado_tiempo(int num)
	{
		switch (num)
		{
			case 1:
				st_tiempo = true;
				break;
			case 0:
				st_tiempo = false;
				break;
		}
	}

	IEnumerator datos_empresa()
	{

		string url = "http://localhost/unity_apis/empresa.php";

		WWWForm form = new WWWForm();
		form.AddField("accion", "datos_empresa");
		UnityWebRequest request = UnityWebRequest.Post(url, form);
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			string responseText = request.downloadHandler.text;
			//Debug.Log(responseText);
			datosResponse response = JsonUtility.FromJson<datosResponse>(responseText);
			if (response.codigo == 200)
			{

					tiempo_inicial = response.datos.hora_inicial.ToString();
					//Debug.Log(tiempo_inicial);
					tiempo_final = response.datos.hora_final.ToString();
					string tiempo_repeticion = response.datos.tiempo_repeticion;
					datos_time_sorteo = tiempo_repeticion.Split(":");
					int num = int.Parse(datos_time_sorteo[0]);
					int num2 = int.Parse(datos_time_sorteo[1]);
					tiempo_sorteo = num * 60 * 60 + num2 * 60 + int.Parse(datos_time_sorteo[2]);
					estado_acumulador = response.datos.acumulacion;

					string tiempo_repeticion_2 = response.datos.result_time;
					datos_time_sorteo_2 = tiempo_repeticion_2.Split(":");
					int num_2 = int.Parse(datos_time_sorteo_2[0]);
					int num2_2 = int.Parse(datos_time_sorteo_2[1]);
					tiempo_tabla_ganadores = num_2 * 60 * 60 + num2_2 * 60 + int.Parse(datos_time_sorteo_2[2]);
					horas24_estado = response.datos.hours_24;
					datos_ingresados = true;
					activar_verificacion();
					ejecutar_llamar_ultimos_registros();
					ejecutar_llamado_valor_vueltas();
					sumar_temp_tiempo.Instance.cambiar_estado_tiempo(0);
					tiempo = sumar_temp_tiempo.Instance.valor_actual_tiempo();
					sumar_temp_tiempo.Instance.tiempo = 0;
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
			public string result_time;
			public string hours_24;
		}
	}

	public void funcion_ejecucion_suma_1(int num_accion)
	{
		StartCoroutine(editar_contador_scena( num_accion));
	}

	private IEnumerator editar_contador_scena(int num_accion)
	{

		string url = "http://localhost/unity_apis/empresa.php";

		WWWForm form = new WWWForm();

		form.AddField("accion", "editar_valor_vueltas");
		form.AddField("ejecucion", num_accion);
		UnityWebRequest request = UnityWebRequest.Post(url, form);
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			string responseText = request.downloadHandler.text;
			//Debug.Log(responseText);
			datosAccion response = JsonUtility.FromJson<datosAccion>(responseText);
			if (response.codigo == 200)
			{
				Debug.Log(response.mensaje);
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



	


	public void ejecutar_llamar_ultimos_registros()
	{

		StartCoroutine(llamar_ultimos_registros());
	}

	private IEnumerator llamar_ultimos_registros()
	{
		string url = "http://localhost/unity_apis/empresa.php";

		WWWForm form = new WWWForm();
		form.AddField("tipo", "S");
		form.AddField("accion", "ultima_actividad");
		UnityWebRequest request = UnityWebRequest.Post(url, form);
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			string responseText = request.downloadHandler.text;
			//Debug.Log(responseText);
			datosRegistros response = JsonUtility.FromJson<datosRegistros>(responseText);
			if (response.codigo == 200)
			{
				string text = response.datos[0].fecha_creacion;
				string text2 = DateTime.Now.ToString("yyy-MM-dd");
				valor_acumulado_nuevo = int.Parse(response.datos[0].valor_acumulado_actual);
				valor_acumulado_ant = int.Parse(response.datos[0].valor_acumulado_ant);
				st_ultimo_registro = response.datos[0].datoextra1;
				string[] array = text.Split(' ');
				//Debug.Log(array[0] + "----" + text2);
				int reultado_tiempo = comprobar_tiempo_funcion();
				if ((estado_acumulador == "S" && reultado_tiempo == 1) || (estado_acumulador == "S" && horas24_estado == "S"))
				{
					valor_subida_scene = valor_acumulado_ant;
					aumento_numero = true;
				}
				else if (text2 == array[0] && reultado_tiempo == 1)
				{
					//Debug.Log("son el mismo dia");
					valor_subida_scene = valor_acumulado_ant;
					aumento_numero = true;
				}
				else
				{
					//Debug.Log("No es el mismo dia");
					valor_acumulado_txt.text = "$0000";
				}
			} else if (response.codigo == 400)
            {
				valor_acumulado_txt.text = "$0000";
			}
			else
			{
				//ventanaemergente.activar_ventana("ERROR", response.mensaje, 0);
				ventanaUI.Instance
				.SetTitle("ERROR")
				.SetMessage("Error with database communication. Please check if the servers are working or contact support.")
				.SetImagen("error")
				.SetColor("#F50801")
				.Show(0);
			}
		}
	}

	public void ejecutar_llamado_valor_vueltas()
	{

		StartCoroutine(valor_vuelta_actual());
	}

	private IEnumerator valor_vuelta_actual()
	{
		string url = "http://localhost/unity_apis/empresa.php";

		WWWForm form = new WWWForm();
		form.AddField("accion", "mostrar_valor_vueltas");
		UnityWebRequest request = UnityWebRequest.Post(url, form);
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			string responseText = request.downloadHandler.text;
			//Debug.Log(responseText);time
			datos_contador_scene response = JsonUtility.FromJson<datos_contador_scene>(responseText);
			if (response.codigo == 200)
			{
				int num = int.Parse(response.datos[0].contador_scena);
				if (num > 0)
				{
					Debug.Log("valor_vueltas --" + num);
                    sumar_temp_tiempo.Instance.funcion_ejecucion_suma_1(0);
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
	}

	private void FixedUpdate()
	{
		if (aumento_numero && datos_ingresados)
		{
			int num = valor_acumulado_nuevo + 1;
			int num2 = 0;
			Debug.Log(valor_acumulado_ant);

			if(st_ultimo_registro == "G")
            {
				num2 = valor_acumulado_ant;

			}
            else 
			{
				num2 = valor_acumulado_nuevo - valor_acumulado_ant;
			}
			//Debug.Log(num);
			if(num2 >= 1000)
            {
				velocidad_tiempo = 140f;
			}
			else if (num2 >= 400 && num2 <= 1000)
			{
				velocidad_tiempo = 90f;
			}
			else if (num2 >= 220 && num2 <= 400)
			{
				velocidad_tiempo = 55f;
			}
			else if (num2 >= 100 && num2 <= 220)
			{
				velocidad_tiempo = 25f;
			}
			else if (num2 >= 50 && num2 <= 100)
			{
				velocidad_tiempo = 15f;
			}
			else if (num2 >= 1 && num2 <= 50)
			{
				velocidad_tiempo = 8f;
			}
			if (st_ultimo_registro == "P")
			{
				//Debug.Log(valor_acumulado_nuevo + "_>_" + valor_acumulado_ant);
				if (inicio_efectos)
				{
					sonido_numero.Play();
					GameObject_efectos.SetActive(true);
					inicio_efectos = false;
				}
				valor_subida_scene += velocidad_tiempo * Time.deltaTime;
				if (valor_subida_scene < (float)num)
				{
					valor_acumulado_txt.text = "$" + (int)valor_subida_scene;
					return;
				}
				else
				{
					GameObject_efectos.SetActive(false);
					sonido_numero.Stop();
					aumento_numero = false;
				}
			}
			else if (st_ultimo_registro == "G")
			{
				//Debug.Log(valor_acumulado_nuevo + "_<_" + valor_acumulado_ant);
				if (inicio_efectos)
				{
					sonido_numero.Play();
					GameObject_efectos.SetActive(true);
					inicio_efectos = false;
				}
				valor_subida_scene -= velocidad_tiempo * Time.deltaTime;
				if (valor_subida_scene >= 0f)
				{
					valor_acumulado_txt.text = "$" + (int)valor_subida_scene;
					return;
                }
                else
                {
					GameObject_efectos.SetActive(false);
					sonido_numero.Stop();
					aumento_numero = false;
				}
				
			}
		}
	}
	public void activar_verificacion()
    {
		StartCoroutine(Verificar_fecha_contador());
	}
	private IEnumerator Verificar_fecha_contador()
	{
		string url = "http://localhost/unity_apis/empresa.php";
		WWWForm form = new WWWForm();
		
		form.AddField("accion", "datos_configuracion");
		UnityWebRequest request = UnityWebRequest.Post(url, form);
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			string responseText = request.downloadHandler.text;
			//Debug.Log(responseText);
			datosconfiguracion response = JsonUtility.FromJson<datosconfiguracion>(responseText);
			if (response.codigo == 200)
			{
				DateTime currentDateTime = DateTime.Now;
				string formattedDate_hora = currentDateTime.ToString("yy-MM-dd HH:mm:ss");
				string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd");
				string fecha_total = response.datos.fecha_modificacion;
				string[] subcadenas = fecha_total.Split(" ");

				if (formattedDateTime != subcadenas[0] && horas24_estado != "S")
                {
					activar_borrar_contador(formattedDate_hora);

				}
				//.Log(subcadenas[0]+"***"+ formattedDateTime);
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
	}

	public void activar_borrar_contador(string fecha_actual)
	{
		StartCoroutine(borrar_contador(fecha_actual));
	}
	private IEnumerator borrar_contador(string fecha_actual)
	{
		string url = "http://localhost/unity_apis/empresa.php";

		WWWForm form = new WWWForm();
		form.AddField("fecha_actual", fecha_actual);
		form.AddField("accion", "borrar_contador");
		UnityWebRequest request = UnityWebRequest.Post(url, form);
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			string responseText = request.downloadHandler.text;
			//Debug.Log(responseText);
			datosAccion response = JsonUtility.FromJson<datosAccion>(responseText);
			if (response.codigo == 200)
			{
				Debug.Log("Borrar contador");
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
	}

	public void iniciar_juegoS()
	{
		StartCoroutine(datos_empresa());
	}

	public int comprobar_tiempo_funcion()
	{
		string text = System.DateTime.Now.ToString("HH:mm:ss");
		if (text.Length != 8)
		{
			text = "0" + text;
		}
		Debug.Log(text);
		//Debug.Log(tiempo_final);
		if(horas24_estado == "S")
        {
			return 1;
        }
        else if (horas24_estado == "N")
		{
			int num = comparar_fechas(tiempo_inicial, text);
			int num2 = comparar_fechas(tiempo_final, text);

			if (num != 2)
			{
				if (num2 != 1)
				{
					return 1;
				}
				return 0;
			}
			return 0;
		} 
			else if (horas24_estado == "H")
		{
			int num = comparar_fechas(tiempo_inicial, text);
			int num2 = comparar_fechas(text, tiempo_final);
			//Debug.Log("RESULTADO ---" + num + "-----" +num2);
			if ((num == 2 && num2 == 1) || (num == 1 && num2 == 2) || num == 0 || num2 == 0)
			{
					return 1;
            }
            else
            {
				return 0;
            }
		}
		return 0;

	}

	private void Start()
	{

		iniciar_juegoS();
	}

	private void Update()
	{
		if (datos_ingresados)
		{
			//validacion de inicio de juego
			if (!boolEjecucionHot)
            {
				validacionTiempoInicio += Time.deltaTime;
				if (validacionTiempoInicio > 60)
				{
					int reultado_tiempo = comprobar_tiempo_funcion();
					//Debug.Log(reultado_tiempo);
					if (reultado_tiempo == 1)
					{
						boolEjecucionHot = true;
					}
					validacionTiempoInicio = 0f;


				}
			}
			
			if(boolEjecucionHot) 
			{
				//ejecucion de tiempo de hot sorteo
				tiempo += Time.deltaTime;
				if (tiempo > (float)tiempo_sorteo)
				{
					int reultado_tiempo = comprobar_tiempo_funcion();
					//Debug.Log(reultado_tiempo);
					if (reultado_tiempo == 1)
					{
						SceneManager.LoadScene("hot_sorteo");
					}
					tiempo = 0f;
				}
			}
			
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				sumar_temp_tiempo.Instance.remplazar_tiempo(tiempo);
				sumar_temp_tiempo.Instance.cambiar_estado_tiempo(1);
				SceneManager.LoadScene("staff");
			}
		}
	}
	public static int comparar_fechas(string fecha, string fecha1)
	{
		//0 = HORA IGUALES
		//1 = EL TIEMPO ACTUAL ES MAYOR AL TIEMPO INGRESADO
		//2 = EL TIEMPO ACTUAL ES MENOR AL TIEMPO INGRESADO
		int num = Convert.ToInt32(fecha.Substring(0, 2));
		int num2 = Convert.ToInt32(fecha.Substring(3, 2));
		int num3 = Convert.ToInt32(fecha.Substring(6, 2));
		int num4 = Convert.ToInt32(fecha1.Substring(0, 2));
		int num5 = Convert.ToInt32(fecha1.Substring(3, 2));
		int num6 = Convert.ToInt32(fecha1.Substring(6, 2));
		if (num == num4)
		{
			if (num2 == num5)
			{
				if (num3 == num6)
				{
					return 0;
				}
				if (num3 > num6)
				{
					return 2;
				}
				return 1;
			}
			if (num2 > num5)
			{
				return 2;
			}
			return 1;
		}
		if (num > num4)
		{
			return 2;
		}
		return 1;
	}

	
	[System.Serializable]
	public class datosconfiguracion
	{
		public int codigo;
		public string mensaje;
		public Datos datos;

		[System.Serializable]
		public class Datos
        {
			public string valor;
			public string fecha_modificacion;
			public string contador_scena;
		}
	}
	[System.Serializable]
	public class datosAccion
	{
		public int codigo;
		public string mensaje;
	}

	[System.Serializable]
	public class datosRegistros
	{
		public int codigo;
		public string mensaje;
		public Datos[] datos;

		[System.Serializable]
		public class Datos
		{
			public string id_registro;
			public string valor_ganado;
			public string valor_acumulado_ant;
			public string num_ganador;
			public string valor_acumulado_actual;
			public string tipo_registro;
			public string datoextra1;
			public string datoextra2;
			public string fecha_creacion;
		}
	}
	

	[System.Serializable]
	public class datos_contador_scene
	{
		public int codigo;
		public string mensaje;
		public Datos[] datos;

		[System.Serializable]
		public class Datos
		{
			public string contador_scena;
		}
	}


}
