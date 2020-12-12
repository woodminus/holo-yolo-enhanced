# Unity + Microsoft Cognitive Services Emotion API Demo
This is a demo project showcasing the [Microsoft Cognitive Services](https://www.microsoft.com/cognitive-services/en-us/emotion-api) Emotion Recognition API used in Unity, specifically for the Microsoft HoloLens running on the Universal Windows Platform. 

It includes the HoloToolkit components, as well as instructions for implementing the Cognitive Services emotion functionality from a photo located locally on the device. This demo uses the streaming assets folder, but it can be applied to other images located in different directories.

## Setup
If you need instructions for setting up your machine for HoloLens development, check out the instructions on the [HoloLens Developer Site](https://www.microsoft.com/microsoft-hololens/en-us/developers).

1. Sign up for a [Cognitive Services](https://www.microsoft.com/cognitive-services/en-us/emotion-api) Emotion API key. You will need this to access the API endpoints.

2. Clone the project to your local machine and open the project in Unity. 

3. Go to Scripts > ImageToEmotionAPI and replace the YOURKEYHERE string with your Emotion API key.

4. Run the application in the editor. Press 'P' to show the image on screen and the space bar to call the API request. The emotion with the highest match is printed in the debug console.

## Components
This demo contains the following components:

* FaceObject.cs: A class that wraps around the Face & Emotion response returned by the API request
* ImageToEmotionAPI.cs: Script that handles the API call and returns the results as a JSON string
* ParseEmotionResponse.cs: Uses the JSON utility (included) to convert the JSON array to a list of FaceObjects
* ShowImageOnPanel.cs: Displays the photo onto a panel

## Scene 