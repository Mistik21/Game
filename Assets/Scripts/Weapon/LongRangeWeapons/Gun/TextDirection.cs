using System;
using UnityEngine;


namespace Gun
{
    public class TextDirection : MonoBehaviour
    {

        // Update is called once per frame
        void Update()
        {
            Transform firstChild = transform.GetChild(0);
            var scale = firstChild.transform.localScale;
            scale.x = transform.parent.localScale.x / Math.Abs(transform.parent.localScale.x);
            firstChild.transform.localScale = scale;
        }
    }
}
