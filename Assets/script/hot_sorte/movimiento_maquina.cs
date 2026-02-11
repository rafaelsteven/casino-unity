using EasyUI.Ventana;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class movimiento_maquina : MonoBehaviour
{
	public class DBempresa
	{
		public int maquinas;
	}

	[SerializeField]
	private Vector2 velocidadMovimiento1;

	[SerializeField]
	private Vector2 velocidadMovimiento2;

	[SerializeField]
	private Vector2 velocidadMovimiento3;

	[SerializeField]
	private Vector2 velocidadMovimiento4;

	[Header("particulas")]
	public GameObject particulas_monedas;

	public GameObject particulas_billetes;

	public GameObject particulas_estrellas;

	public GameObject juegos_artificiales;

	[Header("variables")]
	public GameObject panel_botones;

	public bool activar_sonido = true;

	public AudioSource sonido_ruleta;

	public bool activar_sonido_vic1 = true;

	public AudioSource sonido_vic1;

	private Vector2 offset;

	public GameObject[] objeto_2d;

	public Material material;

	public Vector2 valorangularob;

	private float tiempo;

	private float tiempo_artificial;

	public bool activar_artifical;

	public int num_equipos;

	private string[] num_ganador = new string[3];

	private int i_contador;

	private int validacion_num1;

	private int validacion_num2;

	private int validacion_num3;

	public static string Vhora_inicial;
	public static string Vhora_final;
	public static string Vhours_24;
	public static string Vacumulacion;
	public string strStProbabilidad;

	private DBempresa datosempresa;
	public funcion_sorteo funcion_Sorteo;

	public void iniciar_juegoS()
	{
		StartCoroutine(datos_empresa());
	}

	private IEnumerator datos_empresa()
	{
		string[] datos = new string[1] { "maquinas_empresa" };
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
		string url = "http://localhost/unity_apis/empresa.php";

		WWWForm form = new WWWForm();
		form.AddField("accion", "maquinas_empresa");

		UnityWebRequest request = UnityWebRequest.Post(url, form);
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			string responseText = request.downloadHandler.text;
			Debug.Log(responseText);
			datosResponse response = JsonUtility.FromJson<datosResponse>(responseText);
			if (response.codigo == 200)
			{
				Vhora_inicial = response.datos[0].hora_inicial;
				Vhora_final = response.datos[0].hora_final;
				Vhours_24 = response.datos[0].hours_24;
				Vacumulacion = response.datos[0].acumulacion;
				num_equipos = int.Parse(response.datos[0].maquinas);
				num_equipos = Random.Range(1, num_equipos);
				strStProbabilidad = response.datos[0].st_probabilidad;
				string text = num_equipos.ToString();
				//string text = "679";
				for (int i = 0; i < text.Length; i++)
				{
					char c = text[i];
					Debug.Log(c);
					num_ganador[i_contador] = c.ToString();
					i_contador++;
				}
				Debug.Log("numero ganador" + num_equipos);
				Debug.Log("numero 000" + num_ganador[0]);
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


	private void Start()
	{
		iniciar_juegoS();
	}

	public void ruleta_numero(string num, int num_material)
	{
		switch (num)
		{
			case "0":
				material = objeto_2d[num_material].GetComponent<SpriteRenderer>().material;
				material.mainTextureOffset = new Vector2(0f, 1f);
				break;
			case "1":
				material = objeto_2d[num_material].GetComponent<SpriteRenderer>().material;
				material.mainTextureOffset = new Vector2(0f, 0.095f);
				break;
			case "2":
				material = objeto_2d[num_material].GetComponent<SpriteRenderer>().material;
				material.mainTextureOffset = new Vector2(0f, 0.195f);
				break;
			case "3":
				material = objeto_2d[num_material].GetComponent<SpriteRenderer>().material;
				material.mainTextureOffset = new Vector2(0f, 0.3f);
				break;
			case "4":
				material = objeto_2d[num_material].GetComponent<SpriteRenderer>().material;
				material.mainTextureOffset = new Vector2(0f, 0.4f);
				break;
			case "5":
				material = objeto_2d[num_material].GetComponent<SpriteRenderer>().material;
				material.mainTextureOffset = new Vector2(0f, 0.505f);
				break;
			case "6":
				material = objeto_2d[num_material].GetComponent<SpriteRenderer>().material;
				material.mainTextureOffset = new Vector2(0f, 0.603f);
				break;
			case "7":
				material = objeto_2d[num_material].GetComponent<SpriteRenderer>().material;
				material.mainTextureOffset = new Vector2(0f, 0.703f);
				break;
			case "8":
				material = objeto_2d[num_material].GetComponent<SpriteRenderer>().material;
				material.mainTextureOffset = new Vector2(0f, 0.805f);
				break;
			case "9":
				material = objeto_2d[num_material].GetComponent<SpriteRenderer>().material;
				material.mainTextureOffset = new Vector2(0f, 0.91f);
				break;
		}
	}

	public int activarAcumulacion()
    {
		string fecha_mofidocacion = funcion_Sorteo.strFecha_modificacion;
		int valor_accion = verificar_acumulado(fecha_mofidocacion);
		Debug.Log(valor_accion);
		return valor_accion;
	}
	public static int verificar_acumulado(string data1)
    {
		string text = data1;
		string text2 = System.DateTime.Now.ToString("yyy-MM-dd");
		string[] array = text.Split(' ');
		Debug.Log(array[0] + "----" + text2);
		int reultado_tiempo = comprobar_tiempo_funcion();
		if ((Vacumulacion == "S" && reultado_tiempo != 0) || (Vacumulacion == "S" && Vhours_24 == "S"))
		{
			return 1;
		}
		else if ((text2 == array[0] && reultado_tiempo == 1 && Vacumulacion == "N") || (text2 != array[0] && reultado_tiempo == 4 && Vacumulacion == "N" && Vhours_24 == "H"))
		{
			return 1;
		}
		else
		{
			return 0;
		}
		return 0;
	}
	public static int comprobar_tiempo_funcion()
	{
		string text = System.DateTime.Now.ToString("HH:mm:ss");
		if (text.Length != 8)
		{
			text = "0" + text;
		}
		Debug.Log(text);
		//Debug.Log(tiempo_final);
		if (Vhours_24 == "S")
		{
			return 1;
		}
		else if (Vhours_24 == "N")
		{
			int num = comparar_fechas(Vhora_inicial, text);
			int num2 = comparar_fechas(Vhora_final, text);

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
		else if (Vhours_24 == "H")
		{
			int num = comparar_fechas(Vhora_inicial, text);
			int num2 = comparar_fechas(text, Vhora_final);
			//Debug.Log("RESULTADO ---" + num + "-----" +num2);

			if (num == 2 && num2 == 1)
			{
				return 4;
			}
			else if ((num == 1 && num2 == 2) || num == 0 || num2 == 0)
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
	public static int comparar_fechas(string fecha, string fecha1)
	{
		//0 = HORA IGUALES
		//1 = EL TIEMPO ACTUAL ES MAYOR AL TIEMPO INGRESADO
		//2 = EL TIEMPO ACTUAL ES MENOR AL TIEMPO INGRESADO
		int num = System.Convert.ToInt32(fecha.Substring(0, 2));
		int num2 = System.Convert.ToInt32(fecha.Substring(3, 2));
		int num3 = System.Convert.ToInt32(fecha.Substring(6, 2));
		int num4 = System.Convert.ToInt32(fecha1.Substring(0, 2));
		int num5 = System.Convert.ToInt32(fecha1.Substring(3, 2));
		int num6 = System.Convert.ToInt32(fecha1.Substring(6, 2));
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
	private void Update()
	{
		if (tiempo < 24f)
		{
			tiempo += Time.deltaTime;
			if (activar_sonido)
			{
				sonido_ruleta.Play();
				activar_sonido = false;
			}
			if (tiempo < 5f)
			{
				material = objeto_2d[0].GetComponent<SpriteRenderer>().material;
				offset = velocidadMovimiento1 * Time.deltaTime;
				material.mainTextureOffset += offset;
				particulas_estrellas.SetActive(value: true);
			}
			else if (tiempo < 7f && tiempo > 4f)
			{
				material = objeto_2d[0].GetComponent<SpriteRenderer>().material;
				offset = velocidadMovimiento2 * Time.deltaTime;
				material.mainTextureOffset += offset;
			}
			else if (tiempo < 8f && tiempo > 6f)
			{
				if (validacion_num1 == 0 && i_contador < 3)
				{
					ruleta_numero("0", 0);
					validacion_num1 = 1;
				}
				else if (validacion_num1 == 0 && i_contador == 3)
				{
					string num = num_ganador[0];
					ruleta_numero(num, 0);
					validacion_num1 = 1;
				}
			}
			if (tiempo < 12f)
			{
				material = objeto_2d[1].GetComponent<SpriteRenderer>().material;
				offset = velocidadMovimiento1 * Time.deltaTime;
				material.mainTextureOffset += offset;
				particulas_estrellas.SetActive(value: true);
			}
			else if (tiempo < 14f && tiempo > 12f)
			{
				material = objeto_2d[1].GetComponent<SpriteRenderer>().material;
				offset = velocidadMovimiento2 * Time.deltaTime;
				material.mainTextureOffset += offset;
			}
			else if (tiempo < 15f && tiempo > 13f)
			{
				if (validacion_num2 == 0 && i_contador < 2)
				{
					ruleta_numero("0", 1);
					validacion_num2 = 1;
				}
				else if (validacion_num2 == 0 && i_contador > 1)
				{
					int num2 = 2;
					if (i_contador == 3)
					{
						num2 = 1;
					}
					else if (i_contador == 2)
					{
						num2 = 0;
					}
					string num3 = num_ganador[num2];
					ruleta_numero(num3, 1);
					validacion_num2 = 1;
				}
			}
			if (tiempo < 18f)
			{
				material = objeto_2d[2].GetComponent<SpriteRenderer>().material;
				offset = velocidadMovimiento1 * Time.deltaTime;
				material.mainTextureOffset += offset;
				particulas_estrellas.SetActive(value: true);
			}
			else if (tiempo < 20f && tiempo > 18f)
			{
				material = objeto_2d[2].GetComponent<SpriteRenderer>().material;
				offset = velocidadMovimiento2 * Time.deltaTime;
				material.mainTextureOffset += offset;
			}
			else if (tiempo < 21f && tiempo > 19f && validacion_num3 == 0)
			{
				int num4 = 0;
				if (i_contador == 3)
				{
					num4 = 2;
				}
				else if (i_contador == 2)
				{
					num4 = 1;
				}
				string num5 = num_ganador[num4];
				ruleta_numero(num5, 2);
				validacion_num3 = 1;
				sonido_vic1.Play();
				particulas_estrellas.SetActive(false);
				particulas_monedas.SetActive(true);
				particulas_billetes.SetActive(true);
				
				juegos_artificiales.SetActive(true);
				activar_artifical = true;
			}
		}
		if (activar_artifical)
		{
			tiempo_artificial += Time.deltaTime;
			if (7f < tiempo_artificial)
			{
				juegos_artificiales.SetActive(false);
				panel_botones.SetActive(true);
				activar_artifical = false;

			}
		}
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
			public string maquinas;
			public string hora_inicial;
			public string hora_final;
			public string hours_24;
			public string acumulacion;
			public string st_probabilidad;
		}
	}
}
