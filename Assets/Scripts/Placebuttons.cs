using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

public class Placebuttons : MonoBehaviour
{
    public InputField prefabAButton;
    public MaterialController controller;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(ReadFileData.attribCount);
        //for (int i = 0; i < ReadFileData.attribCount; i++)
        //{
        //    InputField curButton = Instantiate(prefabAButton, this.gameObject.transform);
        //    curButton.transform.position += new Vector3(50 * i, 0, 0);
        //    curButton.onEndEdit.AddListener(delegate { controller.readA(curButton.text, i); });
        //}
        InputField curButton = Instantiate(prefabAButton, this.gameObject.transform);
        curButton.transform.position += new Vector3(50, 0, 0);
        curButton.onEndEdit.AddListener(delegate { controller.readA(curButton.text, 0); });

        InputField curButton1 = Instantiate(prefabAButton, this.gameObject.transform);
        curButton1.transform.position += new Vector3(100, 0, 0);
        curButton1.onEndEdit.AddListener(delegate { controller.readA(curButton1.text, 1); });

        InputField curButton2 = Instantiate(prefabAButton, this.gameObject.transform);
        curButton2.transform.position += new Vector3(150, 0, 0);
        curButton2.onEndEdit.AddListener(delegate { controller.readA(curButton2.text, 2); });

        InputField curButton3 = Instantiate(prefabAButton, this.gameObject.transform);
        curButton3.transform.position += new Vector3(200, 0, 0);
        curButton3.onEndEdit.AddListener(delegate { controller.readA(curButton3.text, 3); });

        InputField curButton4 = Instantiate(prefabAButton, this.gameObject.transform);
        curButton4.transform.position += new Vector3(250, 0, 0);
        curButton4.onEndEdit.AddListener(delegate { controller.readA(curButton4.text, 4); });

        InputField curButton5 = Instantiate(prefabAButton, this.gameObject.transform);
        curButton5.transform.position += new Vector3(300, 0, 0);
        curButton5.onEndEdit.AddListener(delegate { controller.readA(curButton5.text, 5); });

        InputField curButton6 = Instantiate(prefabAButton, this.gameObject.transform);
        curButton6.transform.position += new Vector3(350, 0, 0);
        curButton6.onEndEdit.AddListener(delegate { controller.readA(curButton6.text, 6); });

        InputField curButton7 = Instantiate(prefabAButton, this.gameObject.transform);
        curButton7.transform.position += new Vector3(400, 0, 0);
        curButton7.onEndEdit.AddListener(delegate { controller.readA(curButton7.text, 7); });

        //InputField curButton8 = Instantiate(prefabAButton, this.gameObject.transform);
        //curButton8.transform.position += new Vector3(450, 0, 0);
        //curButton8.onEndEdit.AddListener(delegate { controller.readA(curButton8.text, 8); });

        //InputField curButton9 = Instantiate(prefabAButton, this.gameObject.transform);
        //curButton9.transform.position += new Vector3(500, 0, 0);
        //curButton9.onEndEdit.AddListener(delegate { controller.readA(curButton9.text, 9); });
    }
}
