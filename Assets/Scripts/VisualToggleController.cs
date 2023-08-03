using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Takes in toggles to hide or show given visuals
public class VisualToggleController : MonoBehaviour
{
    public Material GLCLMat;
    public Material vectorMat;
    public Material coordMat;
    public Material attribContribMat;

    private bool GLCLToggle = true;
    private bool vectorToggle = true;
    private bool coordToggle = true;
    private bool attribContribToggle = true;

    //attribContribPointsBeneathEachPlane
    public void setAttribContrib()
    {
        attribContribToggle = !attribContribToggle;
        int disabled = attribContribToggle ? 0 : 1;
        attribContribMat.SetInt("disabled", disabled);
    }

    //connecting line between subcoordinate points
    public void setVector()
    {
        vectorToggle = !vectorToggle;
        int disabled = vectorToggle ? 0 : 1;
        vectorMat.SetInt("disabled", disabled);
    }
    //rbg coordinates for point
    public void setCoord()
    {
        coordToggle = !coordToggle;
        int disabled = coordToggle ? 0 : 1;
        coordMat.SetInt("disabled", disabled);
    }

    //GLCL points
    public void setGLCL()
    {
        GLCLToggle = !GLCLToggle;
        int disabled = GLCLToggle ? 0 : 1;
        GLCLMat.SetInt("disabled", disabled);
    }
}
