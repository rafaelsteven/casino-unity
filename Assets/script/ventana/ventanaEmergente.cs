using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;
using UnityEngine.UI;
using System;

    public class ventanaEmergente : MonoBehaviour
    {

        public TextMeshProUGUI titulo;
        public TextMeshProUGUI mensaje;
        public Button btnsi;
        public Button btnno;
        public Button btnok;
        public Image imagenventana;
        public GameObject panelventana;

        public void activar_ventana(string txttitulo, string txtmensaje, int tipo)
        {
            switch (tipo)
            {
                case 0:
                    panelventana.SetActive(true);
                    Color nuevoColor0 = HexToRGB("#F50801");
                    titulo.color = nuevoColor0;
                    titulo.text = txttitulo;
                    mensaje.text = txtmensaje;
                    imagenventana.sprite = Resources.Load<Sprite>("img/error");
                    btnno.gameObject.SetActive(false);
                    btnsi.gameObject.SetActive(false);
                    btnok.gameObject.SetActive(true);
                    btnok.onClick.AddListener(ok_cerrar_ventana);
                    break;
                case 1:
                    panelventana.SetActive(true);
                    Color nuevoColor1 = HexToRGB("#33d038");
                    titulo.color = nuevoColor1;
                    titulo.text = txttitulo;
                    mensaje.text = txtmensaje;
                    imagenventana.sprite = Resources.Load<Sprite>("img/listo");
                    btnno.gameObject.SetActive(false);
                    btnsi.gameObject.SetActive(false);
                    btnok.gameObject.SetActive(true);
                    btnok.onClick.AddListener(ok_cerrar_ventana);
                    break;
                case 2:
                    panelventana.SetActive(true);
                    Color nuevoColor2 = HexToRGB("#33d038");
                    titulo.color = nuevoColor2;
                    titulo.text = txttitulo;
                    mensaje.text = txtmensaje;
                    imagenventana.sprite = Resources.Load<Sprite>("img/listo");
                    btnno.gameObject.SetActive(true);
                    btnsi.gameObject.SetActive(true);
                    btnok.gameObject.SetActive(false);
                    btnok.onClick.AddListener(ok_cerrar_ventana);
                    btnok.onClick.AddListener(ok_cerrar_ventana);
                    break;
        }
        }

        private void ok_cerrar_ventana()
        {
            panelventana.SetActive(false);
        }

        Color HexToRGB(string hex)
        {
            hex = hex.TrimStart('#');
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, 255);
        }
        internal void activar_ventana()
        {
            throw new NotImplementedException();
        }
    }
