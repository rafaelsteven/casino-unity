using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mover_luces : MonoBehaviour
{
	public Transform objetoimg;

	public float valorangularob;

	public int temvar;

	public float angulo = 0.04f;

	public float angulomenor = 0.04f;
	private void Awake()
	{
	}

	private void sumarangulo()
	{
		base.transform.Rotate(new Vector3(0f, 0f, 6f) * Time.deltaTime);
	}

	private void restarangulo()
	{
		base.transform.Rotate(new Vector3(0f, 0f, -6f) * Time.deltaTime);
	}

	private void Update()
	{
		valorangularob = objetoimg.transform.rotation.z;
		if (valorangularob > angulo && temvar == 0)
		{
			temvar = 1;
		}
		else if (valorangularob < angulomenor && temvar == 1)
		{
			temvar = 0;
		}
		if (temvar == 0)
		{
			sumarangulo();
		}
		else if (temvar == 1)
		{
			restarangulo();
		}
	}
}
