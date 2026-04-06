using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : MonoBehaviour
{
    public Transform Bg1;
    public Transform Bg2;

    public float ScroolSpeed = 30f;

    public float beHeight = 150f;

    private void Start()
    {
        Bg1.position = new Vector3(0f, transform.position.y, 0f);
        Bg2.position = new Vector3(0f, transform.position.y, beHeight);
    }

    private void Update()
    {
        Bg1.Translate(Vector3.back * ScroolSpeed * Time.deltaTime);
        Bg2.Translate(Vector3.back * ScroolSpeed * Time.deltaTime);

        if(Bg1.position.z <= -beHeight)
        {
            Bg1.position = new Vector3(0f, transform.position.y, Bg2.position.z + beHeight);
        }

        if (Bg2.position.z <= -beHeight)
        {
            Bg2.position = new Vector3(0f, transform.position.y, Bg1.position.z + beHeight);
        }
    }
}
