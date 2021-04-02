using HoloToolkit.Unity;
using UnityEngine;

/// <summary>
/// SelectedObjectMessageSender class sends a Unity message to currently selected object.
/// Object selection is controlled via OnSelectObject and OnClearSelection events.
/// Object selection and messages are triggered using voice commands, so keyword responses
/// need to be registered in KeywordManager.
/// </summary>
public class SelectedObjectMessageSender : MonoBehaviour
{
    /// <summary>
    /// Currently selected object.
    /// </summary>
    private GameObject selectedObject;

    /// <summary>
    /// Sets selection to currently focused object.
    /// </summary>
    public void OnSelectObject()
    {
        OnClearSelection();
        selectedObject = GazeManager.Instance.FocusedObject;
        SendMessageToSelectedObject("OnSelectObject");
    }

    /// <summary>
    /// Clears currently selected object.
    /// </summary>
    public void OnClearSelection()
    {
        SendMessageToSelectedObject("OnClearSelection");
        selectedObject = null;
    }

    /// <