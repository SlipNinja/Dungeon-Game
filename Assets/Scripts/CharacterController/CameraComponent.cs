using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraComponent : MonoBehaviour
{
    public static CameraComponent instance;
    public float cameraShakeDuration = 0.1f;
    public float cameraShakeMagnitude = 0.1f;
    Coroutine cameraShake;
    float originalSize;

    Vector3 originalPos;

    private void Start()
    {

        originalPos = this.transform.localPosition;
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.R))
            CameraShakeCall();
    }
    public void CameraShakeCall()
    {if (cameraShake != null)
        {
            StopCoroutine(cameraShake);
            Camera.main.orthographicSize = originalSize;
            transform.localPosition = originalPos;
        }
        cameraShake =  StartCoroutine(CameraShake());
    }

    /*
     * camera shake coroutine
     */
    IEnumerator CameraShake()
    {
        float timer = 0;

        while (timer < cameraShakeDuration)
        {
            timer += Time.deltaTime;
            float x = Random.Range(-1f, 1f) * cameraShakeMagnitude;
            float y = Random.Range(-1f, 1f) * cameraShakeMagnitude;



            transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition + new Vector3(x, y, originalPos.z), timer / cameraShakeDuration);

            yield return null;

        }

        Camera.main.orthographicSize = originalSize;
        transform.localPosition = originalPos;
    }
}
