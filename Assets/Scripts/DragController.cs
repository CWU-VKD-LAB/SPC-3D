using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

//modified version of https://github.com/dipen-apptrait/Vertical-drag-drop-listview-unity/blob/master/DragController.cs
public class DragController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform currentTransform;
    private GameObject mainContent;
    private Vector3 currentPossition;
    public MaterialController controller;

    public int index;

    private int totalChild;
    int[] order;
    private void Start()
    {
        init();
    }

    public void init()
    {
        order = new int[ReadFileData.attribCount];
        controller = this.gameObject.transform.parent.gameObject.GetComponent<Placebuttons>().controller;
        for (int i = 0; i < ReadFileData.attribCount; i++)
        {
            order[i] = i;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        currentPossition = currentTransform.position;
        mainContent = currentTransform.parent.gameObject;
        totalChild = mainContent.transform.childCount;
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentTransform.position =
            new Vector3(eventData.position.x, currentTransform.position.y, currentTransform.position.z);

        for (int i = 0; i < totalChild; i++)
        {
            if (i != currentTransform.GetSiblingIndex())
            {
                GameObject otherChild = mainContent.transform.GetChild(i).gameObject;
                Transform otherTransform = mainContent.transform.GetChild(i);
                int distance = (int)Vector3.Distance(currentTransform.position,
                    otherTransform.position);
                if (distance <= 10)
                {
                    //int tempIndex = this.index;
                    //this.index = otherChild.GetComponent<DragController>().index;
                    //otherChild.GetComponent<DragController>().index = tempIndex;
                    Vector3 otherTransformOldPosition = otherTransform.position;
                    otherTransform.position = new Vector3(currentPossition.x, otherTransform.position.y,
                        otherTransform.position.z);
                    currentTransform.position = new Vector3(otherTransformOldPosition.x, currentTransform.position.y,
                        currentTransform.position.z);
                    currentTransform.SetSiblingIndex(otherTransform.GetSiblingIndex());
                    currentPossition = currentTransform.position;
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameObject[] children = new GameObject[ReadFileData.attribCount];
        //int i = 1 in order to account for the text child
        for (int i = 1; i < totalChild; i++)
        {
            children[i - 1] = mainContent.transform.GetChild(i).gameObject;
        }
        System.Array.Sort(children, CompareByXPosition);
        for (int i = 1; i < totalChild; i++)
        {
            order[i - 1] = children[i - 1].GetComponent<DragController>().index;
        }

        setOrder();

        currentTransform.position = currentPossition;
    }

    int CompareByXPosition(GameObject obj1, GameObject obj2)
    {
        return obj1.transform.position.x.CompareTo(obj2.transform.position.x);
    }

    public void setOrder()
    {
        Debug.Log($"Order: [{string.Join(", ", order)}]");

        controller.order = order;
    }
}