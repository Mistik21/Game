using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public void ToMenuEnd()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
