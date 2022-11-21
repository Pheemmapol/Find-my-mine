using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackground : MonoBehaviour
{
    public Camera mainCamera;
    public Color newColor;
    public bool darkmode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BackgroundChange()
    {
        mainCamera.backgroundColor = newColor;
        if (darkmode)
        {
            Client.instance.darkmode = true;
        }
        else
        {
            Client.instance.darkmode = false;
        }
    }
}
