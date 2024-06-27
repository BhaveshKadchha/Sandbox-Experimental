using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderEffectManager : MonoBehaviour
{
    public GameObject mandelbrot;
    public GameObject julia;
    public Material juliaMat;
    public Canvas ui;
    public Slider realValue;
    public Slider complexValue;

    bool isJuliaOn;

    void Start()
    {
        mandelbrot.SetActive(true);
        julia.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            SwitchShader();

        if (isJuliaOn)
        {
            UpdateShader();

            if (Input.GetKeyDown(KeyCode.C))
                ChangeUI();
        }
    }

    void ChangeUI()
    {
        ui.enabled = !ui.enabled;
    }

    void SwitchShader()
    {
        mandelbrot.SetActive(isJuliaOn);
        julia.SetActive(!isJuliaOn);
        ui.enabled = !isJuliaOn;

        isJuliaOn = !isJuliaOn;
    }

    void UpdateShader()
    {
        juliaMat.SetFloat("_RealVal", realValue.value);
        juliaMat.SetFloat("_ComplexVal", complexValue.value);
    }
}
