
﻿using UnityEngine;
using System.Collections;
using UnityEngine.Windows;

public class ShowImageOnPanel : MonoBehaviour {

    public GameObject ImageFrameObject; // The object to place the image on

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        // Replace this block of code with how you plan to call your image display
	    if(Input.GetKeyDown(KeyCode.P))
        {
            DisplayImage();
        }
	} 

    void DisplayImage()
    {
//        Texture2D imageTxtr = new Texture2D(2, 2);
//        string fileName = gameObject.GetComponent<ImageToEmotionAPI>().fileName;
//#if UNITY_WINRT
//        byte[] fileData = UnityEngine.Windows.File.ReadAllBytes(fileName);
//#else
//        byte[] fileData = System.IO.File.ReadAllBytes(fileName);
//#endif
//        imageTxtr.LoadImage(fileData);
//        ImageFrameObject.GetComponent<Renderer>().material.mainTexture = imageTxtr;
    }
}