using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class TrigerDoorScript : MonoBehaviour
{
    public List<GameObject> Doors;
    public List<GameObject> Trigers;
    public GameObject Room;
    private void OnTriggerStay2D(Collider2D other)
    {
        // Проверяем, что объект находится сверху
        if (other.CompareTag("Player"))
        {
            foreach(var door in Doors)
            {
                door.SetActive(true);
            }
            Room.GetComponent<RoomScript>().enabled=true;
            foreach (var triger in Trigers)
            {
                Destroy(triger);
            }
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Doors.Clear();
        
        // Находим всех детей текущего объекта (включая вложенных)
        foreach (Transform child in Room.GetComponentsInChildren<Transform>(true))
        {
            // Пропускаем сам родительский объект
            if (child == transform) continue;
            
            // Если у дочернего объекта есть тег "Door", добавляем его в список
            if (child.CompareTag("Door"))
            {
                Doors.Add(child.gameObject);
            }
        }
        foreach (Transform child in Room.GetComponentsInChildren<Transform>(true))
        {
            // Пропускаем сам родительский объект
            if (child == transform) continue;
            
            // Если у дочернего объекта есть тег "Door", добавляем его в список
            if (child.CompareTag("TrigerDoor"))
            {
                Trigers.Add(child.gameObject);
            }
        }
    }
}
