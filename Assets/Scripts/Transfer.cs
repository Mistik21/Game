using UnityEngine;

public class Transfer : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Переносится ВЕСЬ объект
    }
}
