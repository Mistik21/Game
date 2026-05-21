using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class BulletSale : MonoBehaviour
{
    public string targetTag = "Player"; // Тег искомых объектов
    public float detectionRange = 1.3f;
    public List<GameObject> nearbyEnemies = new List<GameObject>();
    public GameObject InformationBlock;
    public int price = 20;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
            if (Keyboard.current.eKey.wasPressedThisFrame && nearbyEnemies[0].GetComponent<PlayerScript>().Money >= price)
            {
                SoundEffectsManager.Instance?.PlayItemPurchase();
                nearbyEnemies[0].GetComponent<PlayerScript>().Money -= price;
                nearbyEnemies[0].GetComponent<PlayerScript>().Ammo += 50;
                Destroy(gameObject);
            }
        }
        else
        {
            InformationBlock.SetActive(false);
        }
    }
}
