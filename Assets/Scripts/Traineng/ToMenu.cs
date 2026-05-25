using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public void ToMenuEnd()
    {
        Time.timeScale = 1f;
        MusicManager.Instance?.TurnOffMusic();
        SceneManager.LoadScene("MainMenu");
    }
}
