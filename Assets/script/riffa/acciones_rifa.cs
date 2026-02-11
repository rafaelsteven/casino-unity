using EasyUI.Ventana;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class acciones_rifa : MonoBehaviour
{

	[Header("sonidos")]
	public AudioSource victoria;

	public AudioSource monedas;

	public AudioSource numero_sub;

	public AudioSource sonido_eliminado;

	public AudioSource sonido_victoria;

	[Header("objetos_input")]

	public TMP_InputField input_num_eliminar;

	public TMP_InputField input_num_rango;

	[Header("objetos_text")]

	public TextMeshProUGUI txt_num_ruleta;

	public TextMeshProUGUI txt_valor_ganado;

	public TextMeshProUGUI txt_result_num_ganador;

	public TextMeshProUGUI txt_result_valor_ganador;

	[Header("NUEVO_GameObject")]
	public GameObject panel_genela_datos_rifa;

	public GameObject fondo_generar_datos_rifa;

	public GameObject panel_datos_rifa;

	public GameObject panel_valores_rifa;

	public GameObject panel_general_rifa_juego;

	public GameObject tabla_valores_a_ganar;

	public GameObject panel_ruleta;

	public GameObject efecto_piroteinico;

	public GameObject panel_btn_letrero;

	public GameObject panel_resultado_valor_1;

	public GameObject panel_resultado_valor_2;

	public GameObject panel_resultado_final_rifa_1;
	public GameObject panel_resultado_final_rifa_2;

	bool activar_juego_rifa_ganador = false;
	float tiempo_ganador_rifa = 0;
	[Header("objetos_GameOb")]
	public GameObject panel_valores;

	

	public GameObject panel_totanl_inicial;

	public GameObject panel_rifa;

	

	


	public GameObject panel_registrar_valores;

	[Header("objetos_estado_numero_ganador")]
	public GameObject banner_ganador;

	public GameObject banner_perdedor;

	public GameObject banner_neutro;

	[Header("objetos_botones")]
	public GameObject btn_iniciar_rifa;

	public GameObject btn_valor_ganado;

	[Header("objetos_PARTICULAS")]
	public GameObject ps_confeti;

	public GameObject ps_billetes;

	public GameObject ps_monedas;

	public GameObject ps_monedas_valor;

	public GameObject particulas_estrellas_valor;
	[Header("variables")]

	public ruleta_efecto ruleta_Efecto;
	public valores_a_ganar valores_A_Ganar;

	private bool activar_numero = true;

	private int numeros_eliminados;

	private int numeros_rango;

	private int conta_vueltas;

	private int numero_ganador_rifa;

	private int cantidad_ganadora;

	private List<int> numeros_tocados = new List<int>();


	private bool aumento_numero;

	private float tiempo_valor_garnar;

	public float velocidad_tiempo = 50f;

	private float tiempo;


	public void muestreo_panel_inicio_rifa()
	{
		if(input_num_rango.text != null && input_num_rango.text != "" && input_num_rango.text != " ")
        {
			if (int.TryParse(input_num_eliminar.text, out numeros_eliminados))
			{
				numeros_eliminados = int.Parse(input_num_eliminar.text);
			}
			else
			{
				numeros_eliminados = 0;
			}
			numeros_rango = int.Parse(input_num_rango.text);
			panel_genela_datos_rifa.SetActive(false);
			fondo_generar_datos_rifa.SetActive(false);

			panel_general_rifa_juego.SetActive(true);
        }
        else
        {
			ventanaUI.Instance
			   .SetTitle("INFORMATION")
			   .SetMessage("You must enter the number of numbers to be played in the draw.")
			   .SetImagen("error")
			   .SetColor("#1b40ac")
			   .Show(0);
		}
		
	}

	public void volver_menu()
	{
		SceneManager.LoadScene("STAFF");
	}


	public void activar_juego_rifa()
	{
		ruleta_Efecto.activacion_giro = true;
		txt_num_ruleta.text = "N°000";
		banner_perdedor.SetActive(false);
		banner_neutro.SetActive(true);
		numero_ganador_rifa = UnityEngine.Random.Range(1, numeros_rango);
		Debug.Log(numeros_rango);
		bool flag = numeros_tocados.Any((int x) => x == numero_ganador_rifa);
		btn_iniciar_rifa.SetActive(false);
		if (numeros_eliminados != conta_vueltas)
		{
			if (flag)
			{
				Debug.Log(flag);
				Debug.Log(numero_ganador_rifa);
				activar_juego_rifa();
			}
			else
			{
				numeros_tocados.Add(numero_ganador_rifa);
				Debug.Log("numero eliminado " + numero_ganador_rifa);
			}
		}
		else if (flag)
		{
			Debug.Log(flag);
			Debug.Log(numero_ganador_rifa);
			activar_juego_rifa();
		}
		else
		{
			numeros_tocados.Add(numero_ganador_rifa);
			Debug.Log("numero ganador" + numero_ganador_rifa);
		}
	}

	public void abrir_crear_valores()
	{
		panel_valores.SetActive(false);
		panel_registrar_valores.SetActive(true);
	}

	public void cerrar_crear_valores()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void iniciar_rifa()
	{
		
		panel_totanl_inicial.SetActive(false);
		panel_rifa.SetActive(true);
	}

	public void datos_valores_total()
	{
			StartCoroutine(validar_datos_empresa());
	}
	IEnumerator validar_datos_empresa()
	{

		string url = "http://localhost/unity_apis/empresa.php";

		WWWForm form = new WWWForm();
		form.AddField("tipo", "R");
		form.AddField("accion", "valores_premios");
		UnityWebRequest request = UnityWebRequest.Post(url, form);
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			string responseText = request.downloadHandler.text;
			Debug.Log(responseText);
			datosValores response = JsonUtility.FromJson<datosValores>(responseText);
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
					int count = response.datos.Length;
					int aIndex = UnityEngine.Random.Range(0, count);
					datosValores.Datos primerDato = response.datos[aIndex];
					cantidad_ganadora = int.Parse(primerDato.valor_premio);
					Debug.Log(cantidad_ganadora);	
					panel_resultado_valor_ganado(1);
					aumento_numero = true;

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

	private void panel_resultado_valor_ganado(int accion)
	{
		if(accion == 1)
        {
			tabla_valores_a_ganar.SetActive(false);
			panel_btn_letrero.SetActive(false);
			panel_resultado_valor_1.SetActive(true);
			panel_resultado_valor_2.SetActive(true);
			ps_monedas_valor.SetActive(true);
			particulas_estrellas_valor.SetActive(true);
			ps_billetes.SetActive(false);
			ps_monedas.SetActive(false);
			sonido_victoria.Stop();
		}
		
	}

	public void fun_numero_ruleta()
	{
		if (numeros_eliminados != conta_vueltas)
		{
			banner_neutro.SetActive(false);
			banner_perdedor.SetActive(true);
			sonido_eliminado.Play();
			txt_num_ruleta.text = "N°" + numero_ganador_rifa;
			conta_vueltas++;
			btn_iniciar_rifa.SetActive(true);
		}
		else
		{
			banner_neutro.SetActive(false);
			banner_perdedor.SetActive(false);
			banner_ganador.SetActive(true);
			sonido_victoria.Play();
			txt_num_ruleta.text = "N°" + numero_ganador_rifa;

			ps_confeti.SetActive(true);
			ps_monedas.SetActive(true);
			ps_billetes.SetActive(true);
			efecto_piroteinico.SetActive(true);
			conta_vueltas++;
			activar_juego_rifa_ganador = true;
		}
	}


	private void FixedUpdate()
	{
        if (activar_juego_rifa_ganador)
        {
			tiempo_ganador_rifa += Time.deltaTime;
			if(tiempo_ganador_rifa > 5)
            {
				panel_ruleta.SetActive(false);
				tabla_valores_a_ganar.SetActive(true);
				btn_iniciar_rifa.SetActive(false);
				btn_valor_ganado.SetActive(true);
				
				valores_A_Ganar.datos_valores("R");
				activar_juego_rifa_ganador = false;
			}
        }
        else
        {
			efecto_piroteinico.SetActive(false);
		}

		if (aumento_numero)
		{
			int num = cantidad_ganadora + 1;
			if (num > 310)
			{
				Debug.Log(num);
				velocidad_tiempo = 80f;

			} 
			else if (num > 250 && num < 310)
			{
				velocidad_tiempo = 40f;
			}
			else if (num > 120 && num < 220)
			{
				velocidad_tiempo = 25f;
			}
			else if (num > 50 && num < 100)
			{
				velocidad_tiempo = 15f;
			}
			else if (num > 1 && num < 50)
			{
				velocidad_tiempo = 5f;
			}
			tiempo_valor_garnar += velocidad_tiempo * Time.deltaTime;
			if (activar_numero)
			{
				numero_sub.Play();
				monedas.Play();
				activar_numero = false;
			}
			if (tiempo_valor_garnar < (float)num)
			{
				txt_valor_ganado.text = "$" + (int)tiempo_valor_garnar;
			}
			else
			{
				numero_sub.Stop();
				tiempo += Time.deltaTime;
				if (tiempo > 5f)
				{
					monedas.Stop();
					aumento_numero = false;
					crear_registro_juego();
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SceneManager.LoadScene("Menu_cliente");
		}
	}

	private void resultado_rifa()
	{
		txt_result_valor_ganador.text = "$" + cantidad_ganadora;
		txt_result_num_ganador.text = "N°" + numero_ganador_rifa;
		panel_general_rifa_juego.SetActive(false);
		panel_resultado_final_rifa_1.SetActive(true);
		panel_resultado_final_rifa_2.SetActive(true);
	}

	public void crear_registro_juego()
	{
		StartCoroutine(datos_registro_juego());
	}

	IEnumerator datos_registro_juego()
	{

		string text = DateTime.Now.ToLongTimeString();
		string text2 = DateTime.Now.ToString("yyy-M-d") + " " + text;

		string url = "http://localhost/unity_apis/empresa.php";

		WWWForm form = new WWWForm();
		form.AddField("valor_ganado", cantidad_ganadora.ToString());
		form.AddField("valor_acumulado_actual", "00");
		form.AddField("valor_acumulado_ant", "00");
		form.AddField("num_ganador", numero_ganador_rifa.ToString());
		form.AddField("fecha_creacion", text2.ToString());
		form.AddField("tipo_registro", "R");
		form.AddField("datoextra1", "G");
		form.AddField("datoextra2", "");
		form.AddField("accion", "crear_registros");
		UnityWebRequest request = UnityWebRequest.Post(url, form);
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			string responseText = request.downloadHandler.text;
			Debug.Log(responseText);
			datosresultado response = JsonUtility.FromJson<datosresultado>(responseText);
			if (response.codigo == 400)
			{
				ventanaUI.Instance
			   .SetTitle("ERROR")
			   .SetMessage("Problem registering the winner's data, try again or contact the helpdesk.")
			   .SetImagen("error")
			   .SetColor("#F50801")
			   .Show(0);

			}
			else if (response.codigo == 200)
			{
				ps_monedas_valor.SetActive(false);
				particulas_estrellas_valor.SetActive(false);
				resultado_rifa();

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
	public void accion_aleatoria()
	{

			resultado_rifa();
		
	}
	public void back_menu_valores()
    {
		rol.ROL.implementar_accion("RAFFLE");
		SceneManager.LoadScene("staff");
	}
	public void reiniciar_scene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	[System.Serializable]
	public class datosValores
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
	public class datosresultado
	{
		public int codigo;
		public string mensaje;
	}
}
