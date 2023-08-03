using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

public class Placebuttons : MonoBehaviour
{
    public InputField prefabAButton;
    public Button pregabXDrag;
    public MaterialController controller;

    float[] aVals;
    float c = 0.5f;

    void fillA(string aInp, int placement)
    {
        if (float.TryParse(aInp, out _))
            aVals[placement] = float.Parse(aInp);
    }

    public void fillC(string cInp)
    {
        if (float.TryParse(cInp, out _))
            c = float.Parse(cInp);
    }

    public void updateVals()
    {
        controller.readC(c);
        controller.readA(aVals);
        controller.updateCubes();
    }

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    public void init()
    {
        aVals = new float[ReadFileData.attribCount];
        for (int i = 0; i < aVals.Length; i++)
        {
            aVals[i] = 1;
        }
        Debug.Log("attrib: " + ReadFileData.attribCount);
        for (int i = 0; i < ReadFileData.attribCount; i++)
        {
            int currentPlacement = i;

            Button curButton = Instantiate(pregabXDrag, this.gameObject.transform);
            curButton.transform.position += new Vector3(50 * i, 0, 0);
            curButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "X" + (i + 1);
            curButton.gameObject.GetComponent<DragController>().index = i;
            InputField curInputField = Instantiate(prefabAButton, curButton.transform);
            curInputField.transform.position += new Vector3(0, 0, 0);
            curInputField.onEndEdit.AddListener(delegate { fillA(curInputField.text, currentPlacement); });

        }
        controller.updateCubes();
    }
}
