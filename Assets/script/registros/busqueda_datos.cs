using EasyUI.Ventana;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class busqueda_datos : MonoBehaviour
{

	private string txt_buscar;

	private string tipo_busqueda;

	private string paginacion;

	public TMP_Dropdown tipo_selec;

	public TMP_InputField text_buscar;

	public GameObject datosRegistro_total;

    private void Start()
    {
		buscar_datos();

	}
    public void buscar_datos()
    {
		StartCoroutine(accion_buscar_datos());

	}
	IEnumerator accion_buscar_datos()
	{
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
		string url = "http://localhost/unity_apis/empresa.php";

		WWWForm form = new WWWForm();
		form.AddField("txt_busqueda", text_buscar.text);
		form.AddField("tipo_busqueda", tipo_selec.value.ToString());
		form.AddField("pagina", 0);
		form.AddField("accion", "registro_juego");

		UnityWebRequest request = UnityWebRequest.Post(url, form);
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			string responseText = request.downloadHandler.text;
			Debug.Log(responseText);
			datosRegistros response = JsonUtility.FromJson<datosRegistros>(responseText);

			if (response.codigo == 100)
			{
				GameObject obj = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/error_sin_registro") as GameObject);
				obj.transform.SetParent(base.transform);
				obj.transform.localScale = Vector3.one;
				obj.transform.localPosition = Vector3.zero;

			}else if (response.codigo == 400)
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
				int temo_con = 0;
				foreach (var dato_arry in response.datos)
				{
					temo_con++;
					GameObject g = Instantiate(datosRegistro_total, transform);
					//usuario
					string text_id = dato_arry.id_registro;
					string text_num_ganador = dato_arry.num_ganador;
					string text_valor_ganado = dato_arry.valor_ganado;
					string text_valor_acumulado_ant = dato_arry.valor_acumulado_ant;
					string text_valor_acumulado_actual = dato_arry.valor_acumulado_actual;
					string text_tipo_registro = dato_arry.tipo_registro;
					string text_evento = dato_arry.datoextra1;
					string[] array = (dato_arry.fecha_creacion).Split(" ");
					string[] array2 = array[0].Split("-");
					string text_fecha = array2[1] + "-" + array2[2] + "-" + array2[0] + " " + array[1];
					text_tipo_registro = ((!(text_tipo_registro == "R")) ? "Lottery" : "Raffle");
					text_evento = ((!(text_evento == "G")) ? "Lost" : "Won");
					g.transform.Find("number").GetComponent<TextMeshProUGUI>().text = "Nº" + text_num_ganador;
					g.transform.Find("prize").GetComponent<TextMeshProUGUI>().text = "$" + text_valor_ganado;
					g.transform.Find("prog_prize").GetComponent<TextMeshProUGUI>().text = "$" + text_valor_acumulado_ant;
					g.transform.Find("jackpot").GetComponent<TextMeshProUGUI>().text = "$" + text_valor_acumulado_actual;
					g.transform.Find("type").GetComponent<TextMeshProUGUI>().text = text_tipo_registro;
					g.transform.Find("event").GetComponent<TextMeshProUGUI>().text =  text_evento;
					g.transform.Find("date").GetComponent<TextMeshProUGUI>().text = text_fecha;

				}
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
}
