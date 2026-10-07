using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDLumari : MonoBehaviour
{
    public Image barraVida;
    public Image barraEnergia;

    public TMP_Text textoVida;
    public TMP_Text textoEnergia;
    public TMP_Text cantidadHongo;
    public TMP_Text cantidadCristal;
    public TMP_Text cantidadLinterna;
    public TMP_Text textoObjetivo;

    public void ActualizarVida(float actual, float maxima)
    {
        barraVida.fillAmount = maxima > 0 ? actual / maxima : 0;
        textoVida.text = $"{actual:0}/{maxima:0}";
    }

    public void ActualizarEnergia(float actual, float maxima)
    {
        barraEnergia.fillAmount = maxima > 0 ? actual / maxima : 0;
        textoEnergia.text = $"{actual:0}/{maxima:0}";
    }

    public void ActualizarInventario(int hongos, int cristales, int linternas)
    {
        cantidadHongo.text = hongos.ToString();
        cantidadCristal.text = cristales.ToString();
        cantidadLinterna.text = linternas.ToString();
    }

    public void CambiarObjetivo(string objetivo)
    {
        textoObjetivo.text = "OBJETIVO\n" + objetivo;
    }

    [ContextMenu("Probar HUD")]
public void ProbarHUD()
{
    ActualizarVida(75, 100);
    ActualizarEnergia(40, 100);
    ActualizarInventario(3, 5, 1);
    CambiarObjetivo("Encuentra el santuario");
}
}