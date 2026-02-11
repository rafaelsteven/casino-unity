using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimiento_letras : MonoBehaviour
{
	[SerializeField]
	private float velocidadMovimiento;

	[SerializeField]
	private Transform[] puntosMovimiento;

	[SerializeField]
	private float distanciaMinima;

	private SpriteRenderer spriteRenderer;

	public AudioSource gritos;

	public AudioSource artificiales;


	[Header("variables")]
	private float tiempo_fun;

	private int numeroAleatorio;

	public GameObject goartificiales;


	[SerializeField]
	public bool activar_movimiento = false;

	[SerializeField]
	private bool sonido_grito = false;

	

	private void Start()
	{
		numeroAleatorio = Random.Range(0, puntosMovimiento.Length);
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (activar_movimiento)
		{
		
			base.transform.position = Vector2.MoveTowards(base.transform.position, puntosMovimiento[numeroAleatorio].position, velocidadMovimiento * Time.deltaTime);
			if (Vector2.Distance(base.transform.position, puntosMovimiento[numeroAleatorio].position) < distanciaMinima)
			{
				//sonido_grito = true;
				numeroAleatorio = Random.Range(0, puntosMovimiento.Length);
				if (sonido_grito)
				{
					gritos.Play();
					sonido_grito = false;
					artificiales.Stop();
					goartificiales.SetActive(false);
				}
				activar_movimiento = false;

			}
				tiempo_fun += Time.deltaTime;
			
		}
	}
}
