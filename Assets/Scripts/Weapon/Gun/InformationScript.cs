using System;
using System.Collections.Generic;
using UnityEngine;

public class InformationScript : MonoBehaviour
{
    public string targetTag = "Player";   // Тег искомых объектов
    public float detectionRange = 1.3f;
    public List<GameObject> nearbyEnemies = new List<GameObject>();
    public GameObject InformationBlock;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(!GetComponent<WeaponScript>().enabled)
        {
            GameObject[] allObjects = GameObject.FindGameObjectsWithTag(targetTag);

            nearbyEnemies.Clear();

            foreach (GameObject obj in allObjects)
            {
                float distance = Vector2.Distance(transform.position, obj.transform.position);

                if (distance <= detectionRange)
                {
                    nearbyEnemies.Add(obj);
                }
            }

            if (nearbyEnemies.Count > 0)
            {
                InformationBlock.SetActive(true);
                InformationBlock.GetComponent<TextDirection>().enabled = true;
            }
            else
            {
                InformationBlock.GetComponent<TextDirection>().enabled = false;
                InformationBlock.SetActive(false);
            }
        }
        else
        {
            InformationBlock.GetComponent<TextDirection>().enabled = false;
            InformationBlock.SetActive(false);
        }
    }
}
