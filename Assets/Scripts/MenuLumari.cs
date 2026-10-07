using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLumari : MonoBehaviour
{
    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void NuevaPartida()
{
    SceneManager.LoadScene("Lumaria_Ecena1");
}
}