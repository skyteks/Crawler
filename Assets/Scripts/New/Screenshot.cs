using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public GameObject[] models;

    public bool takePhoto;
    private Coroutine routine;

    private void Start()
    {
        
    }

    void Update()
    {
        if (takePhoto && routine == null)
        {
            routine = StartCoroutine(Coroutine());
        }
    }

    public IEnumerator Coroutine()
    {
        for (int i = 0; i < models.Length; i++)
        {
            models[i].SetActive(false);
        }
        yield return null;
        for (int i = 0; i < models.Length; i++)
        {
            yield return null;

            models[i].SetActive(true);

            yield return null;

            ScreenCapture.CaptureScreenshot(string.Concat(Application.persistentDataPath,"/",models[i].name,".png"));

            yield return null;

            models[i].SetActive(false);

            yield return null;
        }
        routine = null;
        takePhoto = false;
        Debug.Log(Application.persistentDataPath);
    }
}
