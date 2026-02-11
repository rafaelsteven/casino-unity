using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
namespace EasyUI.Ventana
{
    public class Ventana
    {
        public string Title;
        public string Message;
        public string Color;
        public string Imagen;
        public UnityAction OnClass;
        public UnityAction SiOnClass;
        public UnityAction NoOnClass;
    }
    public class ventanaUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI titulotxt;
        [SerializeField] TextMeshProUGUI mensajetxt;
        [SerializeField] Button btnsi;
        [SerializeField] Button btnno;
        [SerializeField] Button btnok;
        [SerializeField] Image imagenventana;
        [SerializeField] GameObject panelventana;
        Ventana ventana = new Ventana();

        public static ventanaUI Instance;

        void Awake()
        {
            Instance = this;
        }
        public ventanaUI SetTitle (string title)
        {
            ventana.Title = title;
            return Instance;
        }
        public ventanaUI SetMessage(string message)
        {
            ventana.Message = message;
            return Instance;
        }
        public ventanaUI SetImagen(string imagen)
        {
            ventana.Imagen = imagen;
            return Instance;
        }
        public ventanaUI SetColor(string color)
        {
            ventana.Color = color;
            return Instance;
        }

        public ventanaUI Onclass( UnityAction action)
        {
            ventana.OnClass = action;
            return Instance;
        }
        public ventanaUI NoOnclass(UnityAction action)
        {
            ventana.NoOnClass = action;
            return Instance;
        }
        public ventanaUI SiOnclass(UnityAction action)
        {
            ventana.SiOnClass = action;
            return Instance;
        }

        public void Show(int accion)
        {
            switch (accion)
            {
                case 0:
                    panelventana.SetActive(true);
                    Color nuevoColor0 = HexToRGB(ventana.Color);
                    titulotxt.color = nuevoColor0;
                    titulotxt.text = ventana.Title;
                    mensajetxt.text = ventana.Message;
                    imagenventana.sprite = Resources.Load<Sprite>("img/"+ ventana.Imagen);
                    btnno.gameObject.SetActive(false);
                    btnsi.gameObject.SetActive(false);
                    btnok.gameObject.SetActive(true);
                    btnok.onClick.AddListener(Hide);
                    break;
                case 1:
                    panelventana.SetActive(true);
                    Color nuevoColor2 = HexToRGB(ventana.Color);
                    titulotxt.color = nuevoColor2;
                    titulotxt.text = ventana.Title;
                    mensajetxt.text = ventana.Message;
                    imagenventana.sprite = Resources.Load<Sprite>("img/" + ventana.Imagen);
                    btnno.gameObject.SetActive(true);
                    btnsi.gameObject.SetActive(true);
                    btnok.gameObject.SetActive(false);
                    btnsi.onClick.AddListener(funcion_btnsi);
                    btnno.onClick.AddListener(funcion_btno);
                    break;
            }
        }

        public void funcion_btnsi()
        {
            panelventana.SetActive(false);

            if (ventana.SiOnClass != null)
                ventana.SiOnClass.Invoke();

            ventana = new Ventana();
        }
        public void funcion_btno()
        {
            panelventana.SetActive(false);

            if (ventana.NoOnClass != null)
                ventana.NoOnClass.Invoke();

            ventana = new Ventana();
        }
        public void Hide()
        {
            panelventana.SetActive(false);

            if (ventana.OnClass != null)
                ventana.OnClass.Invoke();

            ventana = new Ventana();
        }
        Color HexToRGB(string hex)
        {
            hex = hex.TrimStart('#');
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, 255);
        }
       
    }
}
