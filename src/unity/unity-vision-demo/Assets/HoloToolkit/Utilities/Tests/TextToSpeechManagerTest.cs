using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Input;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity;
using System;

public class TextToSpeechManagerTest : MonoBehaviour
{
    private GestureRecognizer gestureRecognizer;
    public TextToSpeechManager textToSpeechManager;

    // Use this for initialization
    void Start ()
    {
        // Set up a GestureRecognizer to detect Select gestures.
        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.TappedEvent += GestureRecognizer_TappedEvent;
        gestureRecognizer.StartCapturingGestures();
    }

    private void GestureRecognizer_TappedEvent(Interac