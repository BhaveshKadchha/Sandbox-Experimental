using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MandelbrotExplorer : MonoBehaviour
{
    public Material mat; 
    Vector2 pos, smoothPos;
    float scale = 4;
    float angle, smoothScale, smoothAngle;

    void FixedUpdate()
    {
        HandleInputs();
        UpdateShader();
    }

    void HandleInputs()
    {
        if(Input.GetKey(KeyCode.KeypadPlus))
            scale *= 0.99f;
        if(Input.GetKey(KeyCode.KeypadMinus))
            scale *= 1.01f;

        if(Input.GetKey(KeyCode.Q))
            angle += .01f;
        if(Input.GetKey(KeyCode.E))
            angle -= .01f;

        Vector2 dir = new Vector2(0.01f * scale,0);
        float s = Mathf.Sin(angle);
        float c = Mathf.Cos(angle);

        // Use Rotation Formula. But dir.y is 0 so removed from the equation.
        dir = new Vector2(dir.x*c ,dir.x*s);

        if(Input.GetKey(KeyCode.A))
            pos -= dir;
        if(Input.GetKey(KeyCode.D))
            pos += dir;

        dir = new Vector2(-dir.y,dir.x);

        if(Input.GetKey(KeyCode.S))
            pos -= dir;
        if(Input.GetKey(KeyCode.W))
            pos += dir;
    }

    void UpdateShader()
    {

        smoothPos = Vector2.Lerp(smoothPos, pos, 0.03f);
        smoothScale = Mathf.Lerp(smoothScale, scale, 0.03f);
        smoothAngle = Mathf.Lerp(smoothAngle, angle, 0.03f);

        float aspectRatio = (float)Screen.width / (float)Screen.height;

        float scaleX = smoothScale;
        float scaleY = smoothScale;

        if(aspectRatio > 1)
            scaleY /= aspectRatio;

        if(aspectRatio < 1)
            scaleX *= aspectRatio;

        mat.SetVector("_Area",new Vector4(smoothPos.x,smoothPos.y,scaleX,scaleY));
        mat.SetFloat("_Angle",smoothAngle);
    }
}