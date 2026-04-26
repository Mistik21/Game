using Unity.AI.Navigation;
using UnityEngine;

public class TrigerDoorScript : MonoBehaviour
{
    public GameObject Door;
    public GameObject Room;
    private void OnTriggerStay2D(Collider2D other)
    {
        // Проверяем, что объект находится сверху
        if (other.CompareTag("Player"))
        {
            Door.SetActive(true);
            Room.GetComponent<RoomScript>().enabled=true;
            Destroy(gameObject);
        }
    }
}
