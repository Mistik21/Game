using System;
using System.Collections;
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
            MusicManager.Instance.EnterCombat();
            SoundEffectsManager.Instance.PlayRoomEnter();

            foreach(var door in Doors)
            {
                StartCoroutine(EnableDoorAfterDelay(0.1f,door));
            }
            Room.GetComponent<RoomScript>().enabled=true;
            foreach (var triger in Trigers)
            {
                Destroy(triger);
            }

            StartCoroutine(Dest(0.1f));
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

    IEnumerator EnableDoorAfterDelay(float delay, GameObject door)
    {
        yield return new WaitForSeconds(delay); // Ждём
        if (door)
        {
            door.SetActive(true); // Включаем дверь
        }
    }

    IEnumerator Dest(float delay)
    {
        yield return new WaitForSeconds(delay); // Ждём
        Destroy(gameObject); // Включаем дверь
    }
}
