using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Flashlight : MonoBehaviour
{
    public Light2D flashlight;
    public Light2D globalLight;
    private Camera cam;
    
    private bool isFlashlightMode = false;
    
    void Start()
    {
        cam = Camera.main;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFlashlightMode = !isFlashlightMode;
            
            if (isFlashlightMode)
            {
                flashlight.enabled = true;
                globalLight.intensity = 0.1f;
            }
            else
            {
                flashlight.enabled = false;
                globalLight.intensity = 1f;
            }
        }
        
        // Поворот фонарика только в режиме фонарика
        if (isFlashlightMode && flashlight.enabled)
        {
            var mouse = cam.ScreenToWorldPoint(Input.mousePosition);
            var direction = (mouse - transform.position).normalized;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            flashlight.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}