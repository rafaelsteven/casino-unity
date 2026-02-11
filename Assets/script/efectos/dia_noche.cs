using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dia_noche : MonoBehaviour
{
	public enum Modo
	{
		Show = 0,
		Hide = 1,
		Nothing = -1
	}

	[Range(0f, 1f)]
	public float Transparencia;

	[Range(0f, 1f)]
	public float TransitionSpeed = 1f;

	[Range(0f, 1f)]
	public float Transparenciacon;

	public SpriteRenderer[] spriteRenderer;

	public SpriteRenderer[] spriteRenderercon;

	public GameObject[] Obetos_noche;

	public float tiempo_dia;

	public float tiempo_noche;

	public bool tiempo_actual;

	public Modo modo;

	private void Start()
	{
		modo = Modo.Nothing;
	}

	private void Update()
	{
		if (!tiempo_actual)
		{
			tiempo_noche += Time.deltaTime;
			if (tiempo_noche < 10f)
			{
				if (Transparencia > -1f)
				{
					if (Transparencia <= 0f)
					{
						modo = Modo.Nothing;
					}
					spritsegundario(1);
					Transparencia -= Time.deltaTime;
					for (int i = 0; i < spriteRenderer.Length; i++)
					{
						spriteRenderer[i].color = new Color(spriteRenderer[i].color.r, spriteRenderer[i].color.g, spriteRenderer[i].color.b, Transparencia);
					}
				}
				if (tiempo_noche > 1f)
				{
					for (int j = 0; j < Obetos_noche.Length; j++)
					{
						Obetos_noche[j].SetActive(true);
					}
				}
			}
			else
			{
				tiempo_noche = 0f;
				tiempo_actual = true;
			}
		}
		if (!tiempo_actual)
		{
			return;
		}
		tiempo_dia += Time.deltaTime;
		if (tiempo_dia < 5f)
		{
			if (tiempo_dia > 1f)
			{
				for (int k = 0; k < Obetos_noche.Length; k++)
				{
					Obetos_noche[k].SetActive(false);
				}
			}
			if (Transparencia < 1f)
			{
				if (Transparencia >= 1f)
				{
					modo = Modo.Nothing;
				}
				spritsegundario(0);
				Transparencia += Time.deltaTime;
				for (int l = 0; l < spriteRenderer.Length; l++)
				{
					spriteRenderer[l].color = new Color(spriteRenderer[l].color.r, spriteRenderer[l].color.g, spriteRenderer[l].color.b, Transparencia);
				}
			}
		}
		else
		{
			tiempo_dia = 0f;
			tiempo_actual = false;
		}
	}

	public void spritsegundario(int valor)
	{
		switch (valor)
		{
			case 0:
				{
					Transparenciacon -= Time.deltaTime;
					for (int j = 0; j < spriteRenderercon.Length; j++)
					{
						spriteRenderercon[j].color = new Color(spriteRenderercon[j].color.r, spriteRenderercon[j].color.g, spriteRenderercon[j].color.b, Transparenciacon);
					}
					break;
				}
			case 1:
				{
					Transparenciacon += Time.deltaTime;
					for (int i = 0; i < spriteRenderercon.Length; i++)
					{
						spriteRenderercon[i].color = new Color(spriteRenderercon[i].color.r, spriteRenderercon[i].color.g, spriteRenderercon[i].color.b, Transparenciacon);
					}
					break;
				}
		}
	}
}
