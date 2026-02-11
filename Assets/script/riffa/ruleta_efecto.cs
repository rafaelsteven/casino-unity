using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ruleta_efecto : MonoBehaviour
{
	public int temvar;

	public float angulo = -300f;

	public float tiempo;

	public ParticleSystem particulas;

	public ParticleSystem particulas_punta;

	public GameObject gameparticulas;

	public GameObject gameparticulas_punta;

	public bool activacion_giro;

	public acciones_rifa Funciones_Rifas;

	[Header("sonidos")]
	public AudioSource ruleta;

	private bool activar_sonidos = true;

	[Obsolete]
	private void Update()
	{
		if (activacion_giro)
		{
			tiempo += Time.deltaTime;
			if (activar_sonidos)
			{
				ruleta.Play();
				activar_sonidos = false;
			}
			if (tiempo > 0f && tiempo < 4f)
			{
				gameparticulas.SetActive(value: true);
				gameparticulas_punta.SetActive(value: true);
				particulas.loop = true;
				particulas_punta.loop = true;
				angulo = -300f;
			}
			else if (tiempo > 5f && tiempo < 7f)
			{
				angulo = -150f;
			}
			else if (tiempo > 9f && tiempo < 10f)
			{
				angulo = -100f;
				particulas.loop = false;
				particulas_punta.loop = false;
				gameparticulas.SetActive(value: false);
				activacion_giro = false;
				activar_sonidos = true;
				Funciones_Rifas.fun_numero_ruleta();
			}
			else if (tiempo > 15f && tiempo < 18f)
			{
				angulo = -200f;
			}
			else if (tiempo > 18f && tiempo < 20f)
			{
				angulo = -120f;
			}
			else if (tiempo > 20f && tiempo < 23f)
			{
				angulo = -70f;
			}
			else if (tiempo > 23f && tiempo < 26f)
			{
				angulo = -30f;
			}
			else if (tiempo > 27f && tiempo < 29f)
			{
				particulas.loop = false;
				particulas_punta.loop = false;
				angulo = -10f;
			}
			else if (tiempo > 30f)
			{
				angulo = -50f;
				gameparticulas.SetActive(value: false);
				activacion_giro = false;
				activar_sonidos = true;
				Funciones_Rifas.fun_numero_ruleta();
			}
			base.transform.Rotate(new Vector3(0f, 0f, angulo) * Time.deltaTime);
		}
		else
		{
			tiempo = 0f;
		}
	}
}
