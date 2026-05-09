using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Flashlight : MonoBehaviour
{
    public Light2D flashlight;
    public Light2D globalLight;
    private Camera cam;


    void Start()
    {
        cam = Camera.main;
        globalLight.intensity = 0.02f;
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            var mouse = cam.ScreenToWorldPoint(Input.mousePosition);
            var direction = (mouse - transform.position).normalized;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            flashlight.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}