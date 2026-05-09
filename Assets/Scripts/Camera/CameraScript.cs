using System;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject Player;
    void Update()
    {
        try
        {
            transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y,-10);
        }
        catch (Exception e)
        {
            var a=1;
        }
    }
}