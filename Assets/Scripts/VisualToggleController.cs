using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Takes in toggles to hide or show given visuals
public class VisualToggleController : MonoBehaviour
{
    public GameObject[] GLCLLines;
    public GameObject[] vector;
    public GameObject[] coordPlanes;
    public GameObject[] attribContrib;

    private bool GLCLToggle = true;
    private bool vectorToggle = true;
    private bool coordToggle = true;
    private bool attribContribToggle = true;

    //attribContribPointsBeneathEachPlane
    public void setAttribContrib()
    {
        attribContribToggle = !attribContribToggle;
        for(int i = 0; i < attribContrib.Length; i++)
        {
            attribContrib[i].SetActive(attribContribToggle);
        }
    }

    //connecting line between subcoordinate points
    public void setVector()
    {
        vectorToggle = !vectorToggle;
        for (int i = 0; i < vector.Length; i++)
        {
            vector[i].SetActive(vectorToggle);
        }
    }
    //rbg coordinates for point
    public void setCoord()
    {
        coordToggle = !coordToggle;
        for(int i = 0; i < coordPlanes.Length; i++)
        {
            coordPlanes[i].SetActive(coordToggle);
        }
    }

    //GLCL points
    public void setGLCL()
    {
        GLCLToggle = !GLCLToggle;
        for (int i = 0; i < GLCLLines.Length; i++)
        {
            GLCLLines[i].SetActive(GLCLToggle);
        }
    }
}
