using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Material flashlightMaterial;
    public Transform player;
    public float radius = 5f;
    public float angle = 10f;
    
    public bool isOn = true;
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
    }
    
    void Update()
    {

        if (flashlightMaterial == null || player == null) 
        return;

        if (Input.GetKeyDown(KeyCode.F))
            isOn = !isOn;

        if (!isOn)
        {
            flashlightMaterial.SetFloat("_Radius", 0);
            return;
        }

        flashlightMaterial.SetFloat("_Radius", radius);

        flashlightMaterial.SetVector("_LightPos", player.position);
        
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
        Vector2 direction = (mousePos - player.position).normalized;
        flashlightMaterial.SetVector("_LightDir", new Vector4(direction.x, direction.y, 0, 0));
        
        flashlightMaterial.SetFloat("_Radius", radius);
        flashlightMaterial.SetFloat("_Angle", angle);
    }
}