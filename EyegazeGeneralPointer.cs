using System;
using System.Collections;
using System.Collections.Generic;
using Tobii.Gaming;
using UnityEngine;


//Code based on Tobii's "GazePlotter.cs". All theirs
public class EyegazeGeneralPointer : MonoBehaviour {

    [SerializeField]
    public float visualizationDistance = 10f;
    [SerializeField]
    public Vector3 pointerScale = new Vector3 (1, 1, 1);
    [SerializeField]
    [Range(0.1f, 1.0f)] // Recommended value is 0.90 or more, or else it will jitter a lot
    public float filterSmoothingFactor = 0.99f;
    [SerializeField]
    public float timeToClick = 1.0f;
    [SerializeField]
    public float clickFeedbackTime = 0.5f;
    [SerializeField]
    public bool clickActive = false;

    public GameObject mPointer;
    public Camera mCamera;

    private bool tryingToClick = false;
    protected bool clickSent = false;
    private bool hasHistoricPoint;
    private Vector3 historicPoint;
    private Vector3 clickPosition;
    private Vector3 newClickPosition;
    private float countdownToClick;
    private float maxClickDistance = 0.2f;

    public GazePoint gazePoint;

    // Use this for initialization
    void Awake () {
        InitializeGazePointer();
        countdownToClick = timeToClick;
        clickPosition = newClickPosition = new Vector3Int(0, 0, 0);
    }

    
    // Update is called once per frame
    protected void Update () {
        gazePoint = TobiiAPI.GetGazePoint();
        if (gazePoint.IsRecent())
        {
            UpdateGazePointerPosition(gazePoint);
            newClickPosition = ProjectToPlaneInWorld(gazePoint);
            newClickPosition = Smoothify(newClickPosition);
            if (clickActive)
            {
                CheckClicker(newClickPosition, clickPosition, gazePoint);
            }

            clickPosition = ProjectToPlaneInWorld(gazePoint);
            clickPosition = Smoothify(clickPosition);
        }
    }

    /// <summary>
    /// Initializes the pointer
    /// </summary>
    public void InitializeGazePointer()
    {
        mPointer = Instantiate(mPointer, new Vector3Int(0, 0, 0), Quaternion.identity);
        //mPointer.transform.localScale = pointerScale;
        mPointer.SetActive(true);
    }

    //Upates the pointer position in world space, smoothes movement
    /// <summary>
    /// Updates the pointer position in the world space, projects from screen plane to plane in world
    /// Smoothes pointer position on movement (Remove jagged movement)
    /// </summary>
    /// <param name="gazePoint"></param>
    public virtual void UpdateGazePointerPosition(GazePoint gazePoint)
    {
        Vector3 gazePointInWorld = ProjectToPlaneInWorld(gazePoint);
        mPointer.transform.position = Smoothify(gazePointInWorld);
    }

    //Use main camera and gaze location to transform into game view
    /// <summary>
    /// Uses camera and Gaze input to determine gaze location in world space
    /// </summary>
    /// <param name="gazePoint"></param>
    /// <returns>The gaze lovation in world space</returns>
    public Vector3 ProjectToPlaneInWorld(GazePoint gazePoint)
    {
        Vector3 gazeOnScreen = gazePoint.Screen;
        gazeOnScreen += (transform.forward * visualizationDistance);
        return Camera.main.ScreenToWorldPoint(gazeOnScreen);
    }

    //Smoothes movement, uses averages and histroy
    /// <summary>
    /// Smoothes the movement of the pointer.
    /// Uses the previous position of the pointer with a smoothing factor 
    /// to determine how to move it according to gaze location.
    /// </summary>
    /// <param name="point"></param>
    /// <returns>Smoothed location of the gaze pointer</returns>
    public Vector3 Smoothify(Vector3 point)
    {
        if (!hasHistoricPoint)
        {
            historicPoint = point;
            hasHistoricPoint = true;
        }
        var smoothedPoint = new Vector3(
            point.x * (1.0f - filterSmoothingFactor) + historicPoint.x * filterSmoothingFactor,
            point.y * (1.0f - filterSmoothingFactor) + historicPoint.y * filterSmoothingFactor,
            point.z * (1.0f - filterSmoothingFactor) + historicPoint.z * filterSmoothingFactor);

        historicPoint = smoothedPoint;
        return smoothedPoint;
    }

    //Check if gaze user is clicking in browser
    /// <summary>
    /// Uses a timer to determine a "Left mouse"-action.
    /// Uses a user-set timer to determine action by checking movement compared to old position.
    /// Then transforms gaze-position to Browser-view position, send mouse action at said position.
    /// </summary>
    /// <param name="newPos"></param>
    /// <param name="oldPos"></param>
    /// <param name="gazePoint"></param>
    public void CheckClicker(Vector3 newPos, Vector3 oldPos, GazePoint gazePoint)
    {
        if (newPos.x >= oldPos.x + maxClickDistance || newPos.y >= oldPos.y + maxClickDistance)
        {
            tryingToClick = false;
            countdownToClick = timeToClick;
        }
        else
        {
            tryingToClick = true;
        }

        if (tryingToClick)
        {

            if (countdownToClick < clickFeedbackTime)
            {
                IncreasePointerAlpha();
            }

            countdownToClick -= Time.deltaTime;
            if (countdownToClick <= 0)
            {
                SetPointerAlpha();
                Debug.Log("Click sent!");
                SendClickMessage();
                countdownToClick = timeToClick;
                Debug.Log("Countdown reset to: " + countdownToClick);
            }
        }

    }

    public virtual void SendClickMessage()
    {
        //Do nothing, override locally
    }

    public void SetPointerAlpha()
    {
        //throw new NotImplementedException();
    }

    public void IncreasePointerAlpha()
    {
        //throw new NotImplementedException();
    }
}
