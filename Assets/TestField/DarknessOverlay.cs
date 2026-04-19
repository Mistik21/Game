using UnityEngine;

public class DarknessOverlay : MonoBehaviour
{
    void Start()
    {
        transform.localScale = new Vector3(100, 100, 1);
    }
    
    void LateUpdate()
    {
        if (Camera.main != null)
            transform.position = Camera.main.transform.position;
    }
}