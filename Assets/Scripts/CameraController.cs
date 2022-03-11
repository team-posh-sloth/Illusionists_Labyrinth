using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] bool disableOtherCams;
    List<Camera> camList;

    // Start is called before the first frame update
    void Start()
    {
        if (disableOtherCams) { DisableOtherCams(); }
    }

    /// <summary>
    /// Initializes camList and assigns all cameras in the scene
    /// </summary>
    void SetCamList() { camList = new List<Camera>(); camList.AddRange(FindObjectsOfType<Camera>()); }

    /// <summary>
    /// Disables all other camera components in the scene
    /// </summary>
    void DisableOtherCams()
    {
        SetCamList(); foreach (Camera listCam in camList) { if (disableOtherCams && listCam != cam) { listCam.enabled = false; } }
    }
}