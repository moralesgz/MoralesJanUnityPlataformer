using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashLoader : MonoBehaviour
{
    public float Delay = 3f;

    void Start()
    {
        Invoke(nameof(LoadMenu), Delay);
    }

    void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
