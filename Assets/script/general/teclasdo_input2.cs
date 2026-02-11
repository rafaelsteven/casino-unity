using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class teclasdo_input2 : MonoBehaviour
{
    public TMP_InputField inputField;

    private void OnEnable()
    {
        inputField = GetComponent<TMP_InputField>();
        // Suscribirse a los eventos OnSelect y OnDeselect del TMP_InputField
        inputField.onSelect.AddListener(OnInputFieldSelect);
        inputField.onDeselect.AddListener(OnInputFieldDeselect);
    }

    private void OnDisable()
    {
        // Asegurarse de desuscribirse de los eventos al deshabilitar el objeto
        inputField.onSelect.RemoveListener(OnInputFieldSelect);
        inputField.onDeselect.RemoveListener(OnInputFieldDeselect);
    }

    private void OnInputFieldSelect(string text)
    {
        // Acción cuando el TMP_InputField obtiene el enfoque (focus)
        mostrar_teclado.Instance.textBox = inputField;
        mostrar_teclado.Instance.abrirTeclado();
        Debug.Log("TMP_InputField ha recibido el enfoque.");
    }

    private void OnInputFieldDeselect(string text)
    {
        // Acción cuando el TMP_InputField pierde el enfoque (focus)
        //GameManager.Instance.textBox = null;
        //teclado.SetActive(false);
        Debug.Log("TMP_InputField ha perdido el enfoque.");
    }
}
