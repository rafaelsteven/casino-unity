using EasyUI.Ventana;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class sumar_temp_tiempo : MonoBehaviour
{

	public static sumar_temp_tiempo Instance;

	
	[SerializeField]
	private string tiempo_inicial;

	[SerializeField]
	private string tiempo_final;

	[SerializeField]
	private int tiempo_sorteo;

	public bool st_tiempo;

	public float tiempo;

	private string[] datos_time_sorteo;

	private string horas24_estado;

	public float validacionTiempoInicio;

	public bool boolEjecucionHot = false;

	public void Awake()
	{
		ejecutar_crear_usuario();
		if (Instance == null)
		{
			Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	public void remplazar_tiempo(float tiempo_var)
	{
		tiempo = tiempo_var;
		Debug.Log(tiempo);
	}

	public void ejecutar_crear_usuario()
	{

		StartCoroutine(datos_empresa());
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
			Debug.Log(responseText);
			datosResponse response = JsonUtility.FromJson<datosResponse>(responseText);
			if (response.codigo == 200)
			{
				tiempo_inicial = response.datos.hora_inicial.ToString();
				tiempo_final = response.datos.hora_final.ToString();
				string tiempo_repeticion = response.datos.tiempo_repeticion;
				datos_time_sorteo = tiempo_repeticion.Split(":");
				int num = int.Parse(datos_time_sorteo[0]);
				int num2 = int.Parse(datos_time_sorteo[1]);
				tiempo_sorteo = num * 60 * 60 + num2 * 60 + int.Parse(datos_time_sorteo[2]);
				horas24_estado = response.datos.hours_24;
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
		StartCoroutine(editar_contador_scena(num_accion));
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
			Debug.Log(responseText);
			datosAccion response = JsonUtility.FromJson<datosAccion>(responseText);
			if (response.codigo == 200)
			{
				if(num_accion == 0)
                {
					SceneManager.LoadScene("hot_sorteo");
				}
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

	[System.Serializable]
	public class datosAccion
	{
		public int codigo;
		public string mensaje;
	}

	public static int comparar_fechas(string fecha, string fecha1)
	{
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
				else if (num3 < num6)
				{

					return 1;
				}
			}
			if (num2 > num5)
			{
				return 2;

			}
			else if (num2 < num5)
			{

				return 1;
			}
		}
		if (num > num4)
		{
			return 2;
		}

		return 1;
	}

	public float valor_actual_tiempo()
	{
		return tiempo;
	}

	public int comprobar_tiempo_funcion()
	{
		string text = System.DateTime.Now.ToString("HH:mm:ss");
		if (text.Length != 8)
		{
			text = "0" + text;
		}
		if (horas24_estado == "S")
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
			//Debug.Log("RESULTADO ---" + num + "-----" + num2);
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

	private void Update()
	{
		if (st_tiempo)
		{
			if(tiempo == 0)
            {
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
			} 
			else
            {
				boolEjecucionHot = true;
			}
			

			if (boolEjecucionHot)
			{
				tiempo += Time.deltaTime;
				if (tiempo > (float)tiempo_sorteo)
				{
					int reultado_tiempo = comprobar_tiempo_funcion();
					//Debug.Log(reultado_tiempo);
					if (reultado_tiempo == 1)
					{
						//Debug.Log("Suma un dia");
						funcion_ejecucion_suma_1(1);
					}
					tiempo = 0f;
				}
			}
		}
	}
}
