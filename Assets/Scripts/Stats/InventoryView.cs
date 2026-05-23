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
    public Sprite SwordPrefab;
    private int indexReloading = -1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        var inventory=Player.GetComponent<inventory>().Inventory;
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            View2Image.GetComponent<Image>().fillAmount = 0f;
            indexReloading = -1;
            View1Image.GetComponent<Image>().fillAmount = 0f;
            indexReloading = -1;
        }
        if (Player.GetComponent<inventory>().IndexInventory()==0)
        {
            if (indexReloading == 1)
            {
                View2Image.GetComponent<Image>().fillAmount = 0f;
                indexReloading = -1;
            }
            // Убираем ".png" на конце и используем generic-версию <Texture>
            GetComponent<RawImage>().texture = Resources.Load<Texture>("Инвентарь/Инвентарь_левый_слот");
        }
        else if (Player.GetComponent<inventory>().IndexInventory()==1)
        {
            if (indexReloading == 0)
            {
                View1Image.GetComponent<Image>().fillAmount = 0f;
                indexReloading = -1;
            }
            GetComponent<RawImage>().texture = Resources.Load<Texture>("Инвентарь/Инвентарь_правый_слот");
        }
        
        var view1Render = View1.GetComponent<Image>();
        var view2Render = View2.GetComponent<Image>();
        if (inventory[0])
        {
            ViewCount1.SetActive(true);
            view1Render.sprite=inventory[0].GetComponent<SpriteRenderer>().sprite;
            var color = view1Render.color;
            color.a = 255;
            view1Render.color = color;

            var weapon = inventory[0].GetComponent<BaseWeapon>();
            if (weapon && weapon.type == "P")
            {
                view1Render.GetComponent<RectTransform>().localScale = new Vector3(weapon.scl[0], weapon.scl[1], weapon.scl[2]);
                ViewCountReal1.SetActive(true);
                Transform childTransform1 = ViewCountReal1.transform.Find("Text (TMP)");
                childTransform1.gameObject.GetComponent<TextMeshProUGUI>().text=weapon.currentAmmo.ToString();
                Transform childTransform = ViewCount1.transform.Find("Text (TMP)");
                childTransform.gameObject.GetComponent<TextMeshProUGUI>().text=weapon.ammoPerReload.ToString();
                ViewType1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Pixel Guns 2D/Guns/Bullets/Button 12");
                ViewType1.transform.localScale = new Vector3(0.75f, 0.75f, 1f)*60;
                ViewType1.GetComponent<SpriteRenderer>().color=Color.white;
            }
            else if (weapon && weapon.type == "M")
            {
                view1Render.GetComponent<RectTransform>().localScale = new Vector3(weapon.scl[0], weapon.scl[1], weapon.scl[2]);
                ViewCountReal1.SetActive(false);
                Transform childTransform = ViewCount1.transform.Find("Text (TMP)");
                childTransform.gameObject.GetComponent<TextMeshProUGUI>().text=weapon.ammoPerReload.ToString();
                ViewType1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Pixel Guns 2D/Guns/Bullets/Button 12");
                ViewType1.GetComponent<SpriteRenderer>().color = Color.blue;
                ViewType1.transform.localScale = new Vector3(0.75f, 0.75f, 1f)*60;
            }
            else
            {
                view1Render.sprite=SwordPrefab;
                view1Render.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.8f, 1);
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
            if (indexReloading == 0)
            {
                indexReloading = -1;
            }
            view1Render.sprite=null;
            var color = view1Render.color;
            color.a = 0;
            view1Render.color = color;
            view1Render.transform.localScale=new Vector3(1,1,1);
        }
        if (inventory[1])
        {
            ViewCount2.SetActive(true);
            view2Render.sprite=inventory[1].GetComponent<SpriteRenderer>().sprite;
            var color = view2Render.color;
            color.a = 255;
            view2Render.color = color;
            var weapon = inventory[1].GetComponent<BaseWeapon>();
            if (weapon && weapon.type == "P")
            {
                view2Render.GetComponent<RectTransform>().localScale = new Vector3(weapon.scl[0], weapon.scl[1], weapon.scl[2]);
                ViewCountReal2.SetActive(true);
                Transform childTransform1 = ViewCountReal2.transform.Find("Text (TMP)");
                childTransform1.gameObject.GetComponent<TextMeshProUGUI>().text=weapon.currentAmmo.ToString();
                Transform childTransform = ViewCount2.transform.Find("Text (TMP)");
                childTransform.gameObject.GetComponent<TextMeshProUGUI>().text=weapon.ammoPerReload.ToString();
                ViewType2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Pixel Guns 2D/Guns/Bullets/Button 12");
                ViewType2.transform.localScale = new Vector3(0.75f, 0.75f, 1f)*60;
                ViewType2.GetComponent<SpriteRenderer>().color=Color.white;
            }
            else if (weapon && weapon.type == "M")
            {
                view2Render.GetComponent<RectTransform>().localScale = new Vector3(weapon.scl[0], weapon.scl[1], weapon.scl[2]);
                ViewCountReal2.SetActive(false);
                Transform childTransform = ViewCount2.transform.Find("Text (TMP)");
                childTransform.gameObject.GetComponent<TextMeshProUGUI>().text=weapon.ammoPerReload.ToString();
                ViewType2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Pixel Guns 2D/Guns/Bullets/Button 12");
                ViewType2.GetComponent<SpriteRenderer>().color = Color.blue;
                ViewType2.transform.localScale = new Vector3(0.75f, 0.75f, 1f)*60;
            }
            else
            {
                view2Render.sprite=SwordPrefab;
                view2Render.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.8f, 1);
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
            View2Image.GetComponent<Image>().fillAmount = 0f;
            if (indexReloading == 1)
            {
                indexReloading = -1;
            }
            ViewType2.GetComponent<SpriteRenderer>().sprite = null;
            ViewType2.GetComponent<SpriteRenderer>().color=Color.white;
            view2Render.sprite=null;
            var color = view2Render.color;
            color.a = 0;
            view2Render.color = color;
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
