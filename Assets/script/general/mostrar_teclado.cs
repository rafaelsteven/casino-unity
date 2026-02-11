using TMPro;
using UnityEngine;

public class mostrar_teclado : MonoBehaviour
{
    public static mostrar_teclado Instance;
    // Start is called before the first frame update
    [SerializeField] public TMP_InputField textBox;
    public GameObject teclado;
    private void Start()
    {
        Instance = this;
        textBox.text = "";
    }
    public void abrirTeclado()
    {
        GameManager.Instance.textBox = textBox;
        teclado.SetActive(true);
    } 
}
