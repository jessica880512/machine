using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //足球是否進球門
    public static bool complete;


    //觸發開始事件:碰到勾選is trigger物件會執行一次
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "進球感應區")
        {
            complete = true;
        }
    }

}