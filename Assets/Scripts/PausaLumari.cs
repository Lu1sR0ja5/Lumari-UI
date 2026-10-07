using UnityEngine;
using UnityEngine.SceneManagement;

public class PausaLumari : MonoBehaviour
{
    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("UI-Lumari");
    }
}