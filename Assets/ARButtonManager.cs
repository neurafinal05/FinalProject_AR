using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARButtonManager : MonoBehaviour
{
    public GameObject model1;
    public GameObject model2;
    public ARRaycastManager arRaycastManager;
    public GameObject arSessionOrigin;

    private GameObject selectedModel;

    void Start()
    {
        Button btn1 = GameObject.Find("ButtonModel1").GetComponent<Button>();
        Button btn2 = GameObject.Find("ButtonModel2").GetComponent<Button>();

        btn1.onClick.AddListener(() => SelectModel(model1));
        btn2.onClick.AddListener(() => SelectModel(model2));
    }

    void SelectModel(GameObject model)
    {
        selectedModel = model;
        arSessionOrigin.SetActive(true);
    }

    void Update()
    {
        if (selectedModel != null && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    Instantiate(selectedModel, hitPose.position, hitPose.rotation);
                    selectedModel = null; // Reset selected model after placing
                }
            }
        }
    }
}
