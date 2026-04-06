using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public Vector3 OriginPos;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
        OriginPos = transform.position;
    }

    public void Shake(float power, float time)
    {
        StartCoroutine(ShakeCamera(power, time));
    }

    public IEnumerator ShakeCamera(float power, float time)
    {
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            float x = Random.Range(-power, power);
            float z = Random.Range(-power, power);

            transform.position = OriginPos + new Vector3(x, 0, z);

            yield return null;
        }
        transform.position = OriginPos;
    }
}
