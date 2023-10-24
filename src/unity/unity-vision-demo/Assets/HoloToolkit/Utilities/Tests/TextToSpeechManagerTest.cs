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

    private void GestureRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        GazeManager gm = GazeManager.Instance;
        if (gm.Hit)
        {
            // Get the target object
            GameObject obj = gm.HitInfo.collider.gameObject;

            // Try and get a TTS Manager
            TextToSpeechManager tts = null;
            if (obj != null)
            {
                tts = obj.GetComponent<TextToSpeechManager>();
            }

            // If we have a text to speech manager on the target object, say something.
            // This voice will appear to emanate from the object.
            if (tts != null)
            {
                // Get the name
                var voiceName = Enum.GetName(typeof(TextToSpeechVoice), tts.Voice);

                // Create message
                var msg = string.Format("This is t