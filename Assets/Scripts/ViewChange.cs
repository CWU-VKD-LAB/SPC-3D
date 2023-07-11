using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ViewChange : MonoBehaviour
{
    public bool interactable = true;
    public GameObject[] fx;
    public GameObject[] x;
    public GameObject[] y;
    public Button[] views;
    //public GameObject[] zero;
    //public GameObject camera can be accessed anytime without explicit reference in variables
    public void topView()
    {
        disableObjects(fx);
        enableObjects(x);
        enableObjects(y);
        Vector3 newPos = new Vector3(-4.96999979f, 5.17000008f, -0.400000006f);
        Quaternion newRot = Quaternion.Euler(new Vector3(90, 0, 0));
        IEnumerator cor = lerpCamera(newPos, newRot);
        StartCoroutine(cor);

    }
    public void centerView()
    {
        enableObjects(fx);
        enableObjects(x);
        enableObjects(y);
        Vector3 newPos = new Vector3(-4.96999979f, 1.75f, -6.73000002f);
        Quaternion newRot = Quaternion.Euler(new Vector3(16, 0, 0));
        IEnumerator cor = lerpCamera(newPos, newRot);
        StartCoroutine(cor);

    }
    public void leftView()
    {
        enableObjects(fx);
        enableObjects(x);
        disableObjects(y);
        Vector3 newPos = new Vector3(-10.1800003f, 0.219999999f, -7.05999994f);
        Quaternion newRot = Quaternion.Euler(new Vector3(0, 45, 0));
        IEnumerator cor = lerpCamera(newPos, newRot);
        StartCoroutine(cor);
    }
    public void rightView()
    {
        enableObjects(fx);
        disableObjects(x);
        enableObjects(y);
        Vector3 newPos = new Vector3(1.63f, 0.219999999f, -6.82000017f);
        Quaternion newRot = Quaternion.Euler(new Vector3(0, -45, 0));
        IEnumerator cor = lerpCamera(newPos, newRot);
        StartCoroutine(cor);
    }

    void disableObjects(GameObject[] objects)
    {
        foreach(GameObject i in objects)
        {
            if(i.activeSelf == true)
                i.SetActive(false);
        }
    }

    void enableObjects(GameObject[] objects)
    {
        foreach (GameObject i in objects)
        {
            if (i.activeSelf == false)
                i.SetActive(true);
        }
    }

    IEnumerator lerpCamera(Vector3 newPos, Quaternion newRot)
    {
        foreach(Button i in views)
        {
            i.interactable = false;
            interactable = false;
        }
        float timeElapsed = 0;
        float lerpDuration = 0.2f;
        while (Camera.main.transform.position != newPos && Camera.main.transform.rotation != newRot)
        {


            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newPos, timeElapsed/lerpDuration);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, newRot, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        foreach (Button i in views)
        {
            i.interactable = true;
            interactable = true;
        }
    }
}
