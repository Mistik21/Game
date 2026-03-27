using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject Player;
    void Update()
    {
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y,-10);
    }
}