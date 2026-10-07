using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CargaLumari : MonoBehaviour
{
    public CanvasGroup panelCarga;

    [Min(0f)] public float tiempoMinimo = 2f;
    [Min(0.01f)] public float tiempoTransicion = 0.5f;

    private bool cargando;
    private CanvasGroup pantallaNegra;

    private void Awake()
    {
        if (panelCarga != null)
            panelCarga.gameObject.SetActive(false);
    }

    public void NuevaPartida()
    {
        CargarEscena("Lumaria_Ecena1");
    }

    public void VolverAlMenu()
    {
        CargarEscena("UI-Lumari");
    }

    public void CargarEscena(string nombreEscena)
    {
        if (cargando)
            return;

        if (panelCarga == null)
        {
            Debug.LogError("Asigna Panel_Carga en Control_Carga.");
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(nombreEscena))
        {
            Debug.LogError("Agrega la escena a Build Profiles: " + nombreEscena);
            return;
        }

        cargando = true;
        StartCoroutine(Cargar(nombreEscena));
    }

    private IEnumerator Cargar(string nombreEscena)
    {
        Time.timeScale = 1f;

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.sendNavigationEvents = false;
        }

        CrearCanvasDeCarga();

        panelCarga.alpha = 0f;
        panelCarga.interactable = false;
        panelCarga.blocksRaycasts = true;
        panelCarga.gameObject.SetActive(true);

        foreach (Animator animador in
                 panelCarga.GetComponentsInChildren<Animator>(true))
        {
            animador.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        yield return Fundir(panelCarga, 1f);

        float inicio = Time.realtimeSinceStartup;
        AsyncOperation operacion = SceneManager.LoadSceneAsync(nombreEscena);
        operacion.allowSceneActivation = false;

        while (operacion.progress < 0.9f ||
               Time.realtimeSinceStartup - inicio < tiempoMinimo)
        {
            yield return null;
        }

        yield return Fundir(pantallaNegra, 1f);

        operacion.allowSceneActivation = true;

        while (!operacion.isDone)
            yield return null;

        panelCarga.gameObject.SetActive(false);
        yield return null;

        yield return Fundir(pantallaNegra, 0f);

        Destroy(gameObject);
    }

    private void CrearCanvasDeCarga()
    {
        CanvasScaler escalaOriginal =
            panelCarga.GetComponentInParent<CanvasScaler>();

        // Conserva la carga mientras cambia la escena.
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        GameObject objetoCanvas = new GameObject(
            "Canvas_Carga",
            typeof(RectTransform),
            typeof(Canvas),
            typeof(CanvasScaler),
            typeof(GraphicRaycaster)
        );

        objetoCanvas.transform.SetParent(transform, false);

        Canvas canvas = objetoCanvas.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 32767;

        CanvasScaler escala = objetoCanvas.GetComponent<CanvasScaler>();
        escala.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        escala.referenceResolution = new Vector2(1920, 1080);
        escala.matchWidthOrHeight = 0.5f;

        if (escalaOriginal != null)
        {
            escala.uiScaleMode = escalaOriginal.uiScaleMode;
            escala.referenceResolution = escalaOriginal.referenceResolution;
            escala.screenMatchMode = escalaOriginal.screenMatchMode;
            escala.matchWidthOrHeight = escalaOriginal.matchWidthOrHeight;
            escala.scaleFactor = escalaOriginal.scaleFactor;
            escala.referencePixelsPerUnit =
                escalaOriginal.referencePixelsPerUnit;
        }

        panelCarga.transform.SetParent(objetoCanvas.transform, false);
        Estirar(panelCarga.GetComponent<RectTransform>());

        GameObject negro = new GameObject(
            "Pantalla_Negra",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Image),
            typeof(CanvasGroup)
        );

        negro.transform.SetParent(objetoCanvas.transform, false);
        Estirar(negro.GetComponent<RectTransform>());

        Image imagen = negro.GetComponent<Image>();
        imagen.color = Color.black;
        imagen.raycastTarget = true;

        pantallaNegra = negro.GetComponent<CanvasGroup>();
        pantallaNegra.alpha = 0f;
        pantallaNegra.interactable = false;
        pantallaNegra.blocksRaycasts = true;
    }

    private void Estirar(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector3.one;
        rect.localRotation = Quaternion.identity;
    }

    private IEnumerator Fundir(CanvasGroup grupo, float destino)
    {
        float inicio = grupo.alpha;
        float tiempo = 0f;
        float duracion = Mathf.Max(0.01f, tiempoTransicion);

        while (tiempo < duracion)
        {
            tiempo += Time.unscaledDeltaTime;
            grupo.alpha = Mathf.Lerp(inicio, destino, tiempo / duracion);
            yield return null;
        }

        grupo.alpha = destino;
    }
}