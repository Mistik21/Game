using System;
using System.Collections.Generic;
using UnityEngine;

public class InformationScript : MonoBehaviour
{
    public string targetTag = "Player";   // Тег искомых объектов
    public float detectionRange = 0.6f;
    public List<GameObject> nearbyEnemies = new List<GameObject>();
    public GameObject InformationText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var scale = transform.localScale;
        scale.x = Math.Abs(scale.x);
        transform.localScale = scale;
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
                InformationText.SetActive(true);
            }
            else
            {
                InformationText.SetActive(false);
            }
        }
        else
        {
            InformationText.SetActive(false);
        }
    }
}
