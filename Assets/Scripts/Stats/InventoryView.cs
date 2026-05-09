using System.Collections;
using RiflePlayer;
using TMPro;
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
    public GameObject ViewType1;
    public GameObject ViewType2;
    public GameObject ViewCount1;
    public GameObject ViewCount2;
    public GameObject ViewCountReal1;
    public GameObject ViewCountReal2;
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
            ViewCount1.SetActive(true);
            view1Render.sprite=inventory[0].GetComponent<SpriteRenderer>().sprite;
            view1Render.transform.localScale=inventory[0].transform.localScale;;
            var weapon = inventory[0].GetComponent<BaseWeapon>();
            if (weapon && weapon.type == "P")
            {
                ViewCountReal1.SetActive(true);
                Transform childTransform1 = ViewCountReal1.transform.Find("Text (TMP)");
                childTransform1.gameObject.GetComponent<TextMeshProUGUI>().text=weapon.currentAmmo.ToString();
                Transform childTransform = ViewCount1.transform.Find("Text (TMP)");
                childTransform.gameObject.GetComponent<TextMeshProUGUI>().text=weapon.ammoPerReload.ToString();
                ViewType1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Pixel Guns 2D/Guns/Bullets/Button 12");
                ViewType1.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
                ViewType1.GetComponent<SpriteRenderer>().color=Color.white;
            }
            else if (weapon && weapon.type == "M")
            {
                ViewCountReal1.SetActive(false);
                Transform childTransform = ViewCount1.transform.Find("Text (TMP)");
                childTransform.gameObject.GetComponent<TextMeshProUGUI>().text=weapon.ammoPerReload.ToString();
                ViewType1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Pixel Guns 2D/Guns/Bullets/Button 12");
                ViewType1.GetComponent<SpriteRenderer>().color = Color.blue;
                ViewType1.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            }
            else
            {
                ViewCountReal1.SetActive(false);
                Transform childTransform = ViewCount1.transform.Find("Text (TMP)");
                childTransform.gameObject.GetComponent<TextMeshProUGUI>().text = "0";
                ViewType1.GetComponent<SpriteRenderer>().sprite = null;
                ViewType1.GetComponent<SpriteRenderer>().color=Color.white;
            }
        }
        else if (!inventory[0])
        {
            ViewCountReal1.SetActive(false);
            ViewCount1.SetActive(false);
            View1Image.GetComponent<Image>().fillAmount = 0f;
            ViewType1.GetComponent<SpriteRenderer>().sprite = null;
            ViewType1.GetComponent<SpriteRenderer>().color=Color.white;
            indexReloading = -1;
            view1Render.sprite=null;
            view1Render.transform.localScale=new Vector3(1,1,1);
        }
        if (inventory[1])
        {
            ViewCount2.SetActive(true);
            view2Render.sprite=inventory[1].GetComponent<SpriteRenderer>().sprite;
            view2Render.transform.localScale=inventory[1].transform.localScale;;
            var weapon = inventory[1].GetComponent<BaseWeapon>();
            if (weapon && weapon.type == "P")
            {
                ViewCountReal2.SetActive(true);
                Transform childTransform1 = ViewCountReal2.transform.Find("Text (TMP)");
                childTransform1.gameObject.GetComponent<TextMeshProUGUI>().text=weapon.currentAmmo.ToString();
                Transform childTransform = ViewCount2.transform.Find("Text (TMP)");
                childTransform.gameObject.GetComponent<TextMeshProUGUI>().text=weapon.ammoPerReload.ToString();
                ViewType2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Pixel Guns 2D/Guns/Bullets/Button 12");
                ViewType2.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
                ViewType2.GetComponent<SpriteRenderer>().color=Color.white;
            }
            else if (weapon && weapon.type == "M")
            {
                ViewCountReal2.SetActive(false);
                Transform childTransform = ViewCount2.transform.Find("Text (TMP)");
                childTransform.gameObject.GetComponent<TextMeshProUGUI>().text=weapon.ammoPerReload.ToString();
                ViewType2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Pixel Guns 2D/Guns/Bullets/Button 12");
                ViewType2.GetComponent<SpriteRenderer>().color = Color.blue;
                ViewType2.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            }
            else
            {
                ViewCountReal2.SetActive(false);
                Transform childTransform = ViewCount2.transform.Find("Text (TMP)");
                childTransform.gameObject.GetComponent<TextMeshProUGUI>().text="0";
                ViewType2.GetComponent<SpriteRenderer>().sprite = null;
                ViewType2.GetComponent<SpriteRenderer>().color=Color.white;
            }
        }
        else if (!inventory[1])
        {
            ViewCountReal2.SetActive(false);
            ViewCount2.SetActive(false);
            View1Image.GetComponent<Image>().fillAmount = 0f;
            indexReloading = -1;
            ViewType2.GetComponent<SpriteRenderer>().sprite = null;
            ViewType2.GetComponent<SpriteRenderer>().color=Color.white;
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
