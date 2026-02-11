using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transparente : MonoBehaviour
{
    public GameObject[] objetosDia;
    public GameObject[] objetosNoche;
    public float vardia = 1.0f;
    public float varnoche = 1.0f;

    public float tiempodia = 0;
    public float tiemponoche = 0.5f;
    bool booldia = true;
    bool boolfalse = false;
    void Start()
    {
        
    }
    void Update()
    {
        if(booldia == true) { 
            tiempodia += Time.deltaTime;
            if (tiempodia > 5)
            {
                noche();
                booldia = false;
                boolfalse = true;
                tiempodia = 0;
            }
        }
        if (boolfalse == true)
        {
            tiemponoche += Time.deltaTime;
            if (tiemponoche > 5)
            {
                dia();
                boolfalse = false;
                booldia = true;
                tiemponoche = 0;
            }
        }

    }
    void noche()
    {
        foreach (GameObject obj in  objetosDia)
        {
            StartCoroutine(desvanecimiento(obj));
        }
        foreach (GameObject obj in objetosNoche)
        {
            StartCoroutine(aparecer(obj));
        }
    }
    void dia()
    {
        foreach (GameObject obj in objetosNoche)
        {
            StartCoroutine(desvanecimiento(obj));
        }
        foreach (GameObject obj in  objetosDia)
        {
            StartCoroutine(aparecer(obj));
        }

    }
    IEnumerator desvanecimiento(GameObject obj)
    {
        float fadeOutStartTime = Time.time;
        Renderer objRenderer = obj.GetComponent<Renderer>();

        while (objRenderer.material.color.a > 0)
        {
            float timeSinceFadeOutStarted = Time.time - fadeOutStartTime;
            float fadeOutProgress = timeSinceFadeOutStarted / varnoche;
            Color objectColor = objRenderer.material.color;
            objectColor.a = Mathf.Lerp(1.0f, 0.0f, fadeOutProgress);
            objRenderer.material.color = objectColor;
            yield return null;
        }
        obj.SetActive(false);
    }
    IEnumerator aparecer(GameObject obj)
    {
        float fadeInStartTime = Time.time;
        Renderer objRenderer = obj.GetComponent<Renderer>();
        obj.SetActive(true);

        while (objRenderer.material.color.a < 1)
        {
            float timeSinceFadeInStarted = Time.time - fadeInStartTime;
            float fadeInProgress = timeSinceFadeInStarted / vardia;
            Color objectColor = objRenderer.material.color;
            objectColor.a = Mathf.Lerp(0.0f, 1.0f, fadeInProgress);
            objRenderer.material.color = objectColor;
            yield return null;
        }
    }
}
