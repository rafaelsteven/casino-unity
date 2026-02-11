using EasyUI.Ventana;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class funcion_sorteo : MonoBehaviour
{
	public class DBempresa
	{
		public string valor;
	}

	public class DB_registro_juego
	{
		public string datos;
	}

	[Header("paneles")]
	public GameObject panel_tabla_valores;

	public GameObject panel_btn_resultado;

	public GameObject panel_resultado_si;

	public GameObject panel_resultado_no;

	public GameObject panel_valor_ganador_1;

	public GameObject panel_valor_ganador_2;

	public GameObject panel_num_ganador;

	public GameObject panel_pregunta;

	public GameObject fondo_wind;

	public GameObject fondo_sorteo;

	public GameObject panel_fondo_resultado_1;

	public GameObject panel_fondo_resultado_2;

	[Header("particulas")]
	public GameObject particulas_confeti;

	public GameObject particulas_monedas;

	public GameObject particulas_billetes;

	public GameObject particulas_monedas_valor;

	public GameObject artificiales_resultados;

	public GameObject particulas_estrellas;

	[Header("texto_GANADO")]
	public TextMeshProUGUI tex_valor_ganado;

	[Header("texto")]
	public TextMeshProUGUI tex_num_ganado_1;

	public TextMeshProUGUI tex_valor_ganado_2;

	public TextMeshProUGUI tex_acumulado_3;

	public TextMeshProUGUI tex_valor_total_4;

	public TextMeshProUGUI txt_titulo_ultimo;

	public TextMeshProUGUI txt_titulo_resultado1;

	[Header("sonido")]
	public AudioSource subida_dinero;

	public AudioSource subida_juego_artificiales;

	private bool subida_dinero_activador = true;

	public AudioSource monedas;

	public AudioSource tambores;

	[Header("variables")]
	public movimiento_maquina Funcion_Ganar;

	public probabilidad Probabilidad;

	public img_resultados img_Resultados;

	public int num_ganador_sorteo;

	private float tiempo;

	private float tiempo2;

	public float velocidad_tiempo = 5f;

	private int accion_registro;

	private bool iniciar_finalizacion_vista = false;

	public int valor_total_ganado;

	public string strFecha_modificacion;

	private bool aumento_numero;

	private float tiempo_valor_garnar;

	public int stAcumulacion;

	private string valor_ganado;

	private string valor_acumulado;

	public valores_a_ganar Valoresganar;

	
	public void datos_emoresa()
	{
		StartCoroutine(valore_premios_datos());
	}

	private IEnumerator valore_premios_datos()
	{
		string url = "http://localhost/unity_apis/empresa.php";

		WWWForm form = new WWWForm();
		form.AddField("tipo", "S");
		form.AddField("accion", "valores_premios");

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
				string estadoProbabilidad = Funcion_Ganar.strStProbabilidad;
				Debug.Log("estadoProbabilidad=" + estadoProbabilidad);
				if (estadoProbabilidad == "S")
                {

					int[] valoresPremioInt = new int[response.datos.Length];

					for (int i = 0; i < response.datos.Length; i++)
					{
						if (int.TryParse(response.datos[i].valor_premio, out int valorPremio))
						{
							valoresPremioInt[i] = valorPremio;
						}
						else
						{
							// Maneja el caso de conversión fallida si es necesario
						}
					}
					Debug.Log(valoresPremioInt);
					int value = Probabilidad.WeightedRandomChoice(valoresPremioInt);
					valor_ganado = response.datos[value].valor_premio;
					Debug.Log(value);
				}
                else if (estadoProbabilidad == "N")
				{
					int count = response.datos.Length;
					int value = UnityEngine.Random.Range(0, count);
					Debug.Log(response.datos);
					valor_ganado = response.datos[value].valor_premio;
					Debug.Log("vlorrrr="+valor_ganado);
				}
				
				stAcumulacion = Funcion_Ganar.activarAcumulacion();
				particulas_monedas_valor.SetActive(true);
				//particulas_monedasZ.SetActive(true);
				particulas_estrellas.SetActive(true);
				aumento_numero = true;
				//Destroy(datosUsuario);
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

	private IEnumerator ejecucion_datos()
	{
		string url = "http://localhost/unity_apis/empresa.php";

		WWWForm form = new WWWForm();
		form.AddField("accion", "valor_acumulado");

		UnityWebRequest request = UnityWebRequest.Post(url, form);
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			string responseText = request.downloadHandler.text;
			Debug.Log(responseText);
			datosvalor response = JsonUtility.FromJson<datosvalor>(responseText);
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
				strFecha_modificacion = response.datos[0].fecha_modificacion;
				valor_acumulado = response.datos[0].valor;
				Debug.Log(valor_acumulado);
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

	private void Start()
	{
		StartCoroutine(ejecucion_datos());
	}

	private void Update()
	{
		if (iniciar_finalizacion_vista)
		{
			panel_valor_ganador_1.SetActive(false);
			panel_valor_ganador_2.SetActive(false);
			fondo_sorteo.SetActive(false);
			img_Resultados.activar_img = true;
			panel_fondo_resultado_1.SetActive(true);
			panel_fondo_resultado_2.SetActive(true);
			completar_datos_text();
			artificiales_resultados.SetActive(true);
			iniciar_finalizacion_vista = false;
			subida_juego_artificiales.Play();
			
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SceneManager.LoadScene("Menu_cliente");
		}
	}

	public void completar_datos_text()
	{
		if(stAcumulacion == 0)
        {
			valor_acumulado = "0";

		}

		num_ganador_sorteo = Funcion_Ganar.num_equipos;
		valor_total_ganado = int.Parse(valor_ganado) + int.Parse(valor_acumulado);
		crear_registro_juego();
		if (accion_registro == 1)
		{
			txt_titulo_ultimo.text = "TOTAL PRIZE";
			txt_titulo_resultado1.text = "WINNER";

		}
        else
        {
			txt_titulo_ultimo.text = "JACKPOT PRIZE";
			txt_titulo_resultado1.text = "ROLLOVER";
		}
			tex_num_ganado_1.text = "Nº"+num_ganador_sorteo.ToString();
			tex_valor_ganado_2.text = "$" + valor_ganado;
			tex_acumulado_3.text = "$" + valor_acumulado;
			tex_valor_total_4.text = "$" + valor_total_ganado;

	}

	public void accion_win_si()
	{
		panel_pregunta.SetActive(false);
		panel_num_ganador.SetActive(false);
		panel_tabla_valores.SetActive(true);
		Valoresganar.datos_valores("S");
		panel_btn_resultado.SetActive(true);
		accion_registro = 1;
	}

	public void accion_win_no()
	{
		panel_pregunta.SetActive(false);
		panel_num_ganador.SetActive(false);
		panel_valor_ganador_1.SetActive(true);
		panel_valor_ganador_2.SetActive(true);
		datos_emoresa();
		accion_registro = 0;
	}

	public void accion_win_iniciar()
	{
		panel_tabla_valores.SetActive(false);
		panel_btn_resultado.SetActive(false);
		panel_num_ganador.SetActive(false);
		panel_valor_ganador_1.SetActive(true);
		panel_valor_ganador_2.SetActive(true);
		datos_emoresa();
	}

	public void crear_registro_juego()
	{
		StartCoroutine(datos_registro_juego());
	}

	private IEnumerator datos_registro_juego()
	{
		string text = "";
		string text2 = DateTime.Now.ToLongTimeString();
		string text3 = DateTime.Now.ToString("yyy-M-d") + " " + text2;
		string text4 = "";
		if (accion_registro == 1)
		{
			text = valor_ganado.ToString();
			text4 = "G";
		}
		else if (accion_registro == 0)
		{
			text = "0";
			text4 = "P";
		}

		string url = "http://localhost/unity_apis/empresa.php";

		WWWForm form = new WWWForm();
		form.AddField("valor_ganado", text);
		form.AddField("valor_acumulado_actual", valor_total_ganado.ToString());
		form.AddField("valor_acumulado_ant", valor_acumulado.ToString());
		form.AddField("num_ganador", num_ganador_sorteo.ToString());
		form.AddField("fecha_creacion", text3.ToString());
		form.AddField("tipo_registro", "S");
		form.AddField("datoextra1", text4);
		form.AddField("datoextra2", "");
		form.AddField("accion", "crear_registros");

		UnityWebRequest request = UnityWebRequest.Post(url, form);
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			string responseText = request.downloadHandler.text;
			Debug.Log(responseText);
			datoscreacion response = JsonUtility.FromJson<datoscreacion>(responseText);
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
				StartCoroutine(editar_acumulado());
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


	private IEnumerator editar_acumulado()
	{
		string text = DateTime.Now.ToLongTimeString();
		string text2 = DateTime.Now.ToString("yyy-M-d") + " " + text;
		string url = "http://localhost/unity_apis/empresa.php";

		WWWForm form = new WWWForm();
		if (accion_registro == 1)
		{
			form.AddField("valor", "0");
			form.AddField("fecha_modificacion", text2);
			Debug.Log("datos 1");
		}
		else if (accion_registro == 0)
		{
			form.AddField("valor", valor_total_ganado.ToString());
			form.AddField("fecha_modificacion", text2);
			Debug.Log("datos 0");
		}
		form.AddField("accion", "editar_acumulado");

		UnityWebRequest request = UnityWebRequest.Post(url, form);
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			string responseText = request.downloadHandler.text;
			Debug.Log(responseText);
			datoscreacion response = JsonUtility.FromJson<datoscreacion>(responseText);
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
				Debug.Log("finalizar");
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
	public void ir_registros()
	{
		SceneManager.LoadScene("registro");
	}

	private void FixedUpdate()
	{
		if (aumento_numero)
		{

			int num = int.Parse(valor_ganado) + 1;

			if (num >= 311)
			{
				velocidad_tiempo = 80f;
			}
			else if (num >= 250 && num <= 310)
			{
				velocidad_tiempo = 50f;
			}
			else if (num >= 100 && num <= 220)
			{
				velocidad_tiempo = 25f;
			}
			else if (num >= 50 && num <= 100)
			{
				velocidad_tiempo = 15f;
			}
			else if (num >= 1 && num <= 50)
			{
				velocidad_tiempo = 5f;
			}

			tiempo_valor_garnar += velocidad_tiempo * Time.deltaTime;

			if (subida_dinero_activador)
			{
				monedas.Play();
				subida_dinero.Play();
				subida_dinero_activador = false;
			}

			if (tiempo_valor_garnar < (float)num)
			{
				
				tex_valor_ganado.text = "$" + (int)tiempo_valor_garnar;
            }
            else
            {
				subida_dinero.Stop();
				tiempo += Time.deltaTime;
				if (tiempo > 5f)
				{
					monedas.Stop();
					particulas_monedas_valor.SetActive(false);
					//particulas_monedasZ.SetActive(false);
					particulas_estrellas.SetActive(false);
					aumento_numero = false;
					iniciar_finalizacion_vista = true;
				}
			}
		}
		
	}

	public void iniciar_scenas()
	{
		StartCoroutine(scenas_datos());
	}

	private IEnumerator scenas_datos()
	{

		string url = "http://localhost/unity_apis/empresa.php";

		WWWForm form = new WWWForm();
		form.AddField("accion", "scene_empresa");

		UnityWebRequest request = UnityWebRequest.Post(url, form);
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			string responseText = request.downloadHandler.text;
			Debug.Log(responseText);
			datosscena response = JsonUtility.FromJson<datosscena>(responseText);
			if (response.codigo == 200)
			{
				Debug.Log(response.datos.scene);
				switch (response.datos.scene)
				{
					case "US":
						SceneManager.LoadScene("newyok");
						break;
					case "JU":
						SceneManager.LoadScene("selva");
						break;
					case "OR":
						SceneManager.LoadScene("oriente");
						break;
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

	private void accion_scenas()
	{
		
	}

	[System.Serializable]
	public class datosResponse
	{
		public int codigo;
		public string mensaje;
		public Datos[] datos;

		[System.Serializable]
		public class Datos
		{
			public string valor_premio;

		}
	}

	[System.Serializable]
	public class datosvalor
	{
		public int codigo;
		public string mensaje;
		public Datos[] datos;

		[System.Serializable]
		public class Datos
		{
			public string valor;
			public string fecha_modificacion;

		}
	}

	[System.Serializable]
	public class datoscreacion
	{
		public int codigo;
		public string mensaje;
	}

	[System.Serializable]
	public class datosscena
	{
		public int codigo;
		public string mensaje;
		public Datos datos;

		[System.Serializable]
		public class Datos
		{
			public string scene;

		}
	}
}
