using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rol : MonoBehaviour
{
    public static rol ROL;

    [SerializeField] public string tipoRol;

    [SerializeField] public string accion_menu;
    private void Awake()
    {
        if(rol.ROL == null)
        {
            rol.ROL = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void asignarRol(string textorol)
    {
        tipoRol = textorol;
    }
    public void implementar_accion(string accion)
    {
        accion_menu = accion;
    }
}
