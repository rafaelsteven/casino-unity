using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class funcion_registros_datos : MonoBehaviour
{
	public void validar_back()
    {
		switch (rol.ROL.tipoRol)
		{
			case "MGR":
				SceneManager.LoadScene("mgr");
				break;
			case "STAFF":
				SceneManager.LoadScene("staff");
				break;
			case "ADMIN":
				SceneManager.LoadScene("admin");
				break;
		}
	}


}
