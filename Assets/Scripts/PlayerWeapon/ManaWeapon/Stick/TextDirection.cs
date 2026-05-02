using System;
using TMPro;
using UnityEngine;
using RiflePlayer;

namespace StickPlayer
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
                GetComponent<TextMeshPro>().text = "Посох";
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
                GetComponent<TextMeshPro>().text = "Посох";
                RectTransform rect = GetComponent<RectTransform>();
                Vector2 pos = rect.anchoredPosition;
                pos.y = 1f;
                rect.anchoredPosition = pos;
            }
        }
    }
}
