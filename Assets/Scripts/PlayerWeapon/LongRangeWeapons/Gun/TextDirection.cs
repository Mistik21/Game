using System;
using RiflePlayer;
using TMPro;
using UnityEngine;


namespace GunPlayer
{
    public class TextDirection : MonoBehaviour
    {

        void Start()
        {
            if(GetComponentInParent<BaseWeapon>().sale)
            {
                GetComponent<TextMeshPro>().text += GetComponentInParent<BaseWeapon>().price.ToString() + " монет";
            }
            else
            {
                GetComponent<TextMeshPro>().text = "Пистолет";
            }
        }
        // Update is called once per frame
        void Update()
        {
            Transform firstChild = transform.GetChild(0);
            var scale = firstChild.transform.localScale;
            scale.x = transform.parent.localScale.x / Math.Abs(transform.parent.localScale.x);
            firstChild.transform.localScale = scale;
            if (!GetComponentInParent<BaseWeapon>().sale)
            {
                GetComponent<TextMeshPro>().text = "Пистолет";
                RectTransform rect = GetComponent<RectTransform>();
                Vector2 pos = rect.anchoredPosition;
                pos.y = 1f;
                rect.anchoredPosition = pos;
            }
        }
    }
}
