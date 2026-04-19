using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    public GameObject Player;
    public GameObject View1;
    public GameObject View2;
    public GameObject View1Image;
    public GameObject View2Image;
    private int indexReloading = -1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var inventory=Player.GetComponent<inventory>().Inventory;
        if (Player.GetComponent<inventory>().IndexInventory()==0)
        {
            if (indexReloading == 1)
            {
                View2Image.GetComponent<Image>().fillAmount = 0f;
                indexReloading = -1;
            }
            GetComponent<SpriteRenderer>().sprite=Resources.Load<Sprite>("Инвентарь/Инвентарь_левый_слот");;
        }
        else if (Player.GetComponent<inventory>().IndexInventory()==1)
        {
            if (indexReloading == 0)
            {
                View1Image.GetComponent<Image>().fillAmount = 0f;
                indexReloading = -1;
            }
            GetComponent<SpriteRenderer>().sprite=Resources.Load<Sprite>("Инвентарь/Инвентарь_правый_слот");;
        }
        
        var view1Render = View1.GetComponent<SpriteRenderer>();
        var view2Render = View2.GetComponent<SpriteRenderer>();
        if (inventory[0])
        {
            view1Render.sprite=inventory[0].GetComponent<SpriteRenderer>().sprite;
            view1Render.transform.localScale=inventory[0].transform.localScale;;
        }
        else if (!inventory[0])
        {
            view1Render.sprite=null;
            view1Render.transform.localScale=new Vector3(1,1,1);
        }
        if (inventory[1])
        {
            view2Render.sprite=inventory[1].GetComponent<SpriteRenderer>().sprite;
            view2Render.transform.localScale=inventory[1].transform.localScale;;
        }
        else if (!inventory[1])
        {
            view2Render.sprite=null;
            view2Render.transform.localScale=new Vector3(1,1,1);
        }
    }

    public void ViewReload(int time)
    {
        
        StartCoroutine(ReloadRoutine(time));
    }

    public IEnumerator ReloadRoutine(float totalTime)
    {
        int currentSlot = Player.GetComponent<inventory>().IndexInventory();
        Image targetImage = null;
        indexReloading=currentSlot;
        if (currentSlot == 0)
        {
            targetImage = View1Image.GetComponent<Image>();
        }
        else if (currentSlot == 1)
        {
            targetImage = View2Image.GetComponent<Image>();
        }
        
        if (targetImage == null)
        {
            Debug.LogError("Target Image не найден!");
            yield break;
        }
        
        // Начинаем с fillAmount = 1 (полностью заполнено)
        targetImage.fillAmount = 1f;
        
        float elapsedTime = 0f;
        
        // Пока не прошло totalTime секунд
        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            
            // fillAmount уменьшается от 1 до 0
            // Чем больше прошло времени, тем меньше fillAmount
            float fill = 1f - (elapsedTime / totalTime);
            targetImage.fillAmount = Mathf.Clamp01(fill);
            
            yield return null; // Ждём следующий кадр
        }
        
        // Завершаем: fillAmount = 0
        targetImage.fillAmount = 0f;
        indexReloading = -1;
    }
}
