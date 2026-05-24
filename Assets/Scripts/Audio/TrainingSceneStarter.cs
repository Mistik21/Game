using UnityEngine;

public class TrainingSceneStarter : MonoBehaviour
{
    void Start()
    {
        // Запускаем музыку, когда сцена загрузилась
        MusicManager.Instance?.StartMusic();
    }
}