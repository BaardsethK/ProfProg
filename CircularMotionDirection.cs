using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GazeAware))]
public class CircularMotionDirection : MonoBehaviour, IPointerClickHandler {

    public enum MoveDir
    {
        UP,
        DOWN,
        LEFT, 
        RIGHT,
        ZOOM_IN,
        ZOOM_OUT,
        RESET_CAMERA
    };

    private CircularMotionSettings settings;

    //Variables set by CircularMotionSettings.cs
    private Camera mainCamera;
    private Canvas motionCircleCanvas;
    private Canvas useAreaCanvas;

    private float cameraMoveSpeed;
    //End of

    [SerializeField]
    public GazeAware gaze;
    [SerializeField]
    MoveDir moveDir;

    private Vector3 useAreaPosition;
    private Vector2 useAreaSize;
    private Vector3 useAreaScale;
    private Vector3 stdCameraPos;

    /// <summary>
    /// Initialize the class, loading from CircleMotionSettings.
    /// </summary>
    void Awake()
    {
        settings = GameObject.FindGameObjectWithTag("MotionButtonSettings").GetComponent<CircularMotionSettings>();
        gaze = GetComponent<GazeAware>();

        this.mainCamera = settings.mainCamera;
        this.motionCircleCanvas = settings.motionCircleCanvas;
        this.useAreaCanvas = settings.useAreaCanvas;
        this.cameraMoveSpeed = settings.cameraMoveSpeed;
        this.stdCameraPos = settings.stdCameraPos;

        useAreaSize = useAreaCanvas.GetComponent<RectTransform>().sizeDelta;
        useAreaScale = useAreaCanvas.GetComponent<Transform>().localScale;
        useAreaPosition = useAreaCanvas.GetComponent<Transform>().localPosition;
    }

    // Update is called once per frame
    void Update () {
		if (gaze.HasGazeFocus)
        {
            Clicked();
        }
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        Clicked();
    }

    /// <summary>
    /// Depending on set enum, moves the camera around inside the view of the chosen usage canvas.
    /// Camera can move to edges of set usage canvas, and zoom as close as possible in z-axis position.
    /// Movement-size is defined by CircularMotionSettings.
    /// </summary>
    public void Clicked()
    {
        switch (moveDir)
        {
            case MoveDir.UP:
                if (motionCircleCanvas.transform.position.y < useAreaSize.y / 2 * useAreaScale.x)
                {
                    mainCamera.transform.Translate(new Vector3(0, cameraMoveSpeed, 0));
                    motionCircleCanvas.transform.Translate(new Vector3(0, cameraMoveSpeed, 0));
                }
                break;
            case MoveDir.DOWN:
                if (motionCircleCanvas.transform.position.y > -(useAreaSize.y / 2) * useAreaScale.x)
                {
                    mainCamera.transform.Translate(new Vector3(0, -cameraMoveSpeed, 0));
                    motionCircleCanvas.transform.Translate(new Vector3(0, -cameraMoveSpeed, 0));
                }
                break;
            case MoveDir.LEFT:
                if (motionCircleCanvas.transform.position.x > - (useAreaSize.x / 2) * useAreaScale.y)
                {
                    mainCamera.transform.Translate(new Vector3(-cameraMoveSpeed, 0, 0));
                    motionCircleCanvas.transform.Translate(new Vector3( cameraMoveSpeed, 0, 0));
                }
                break;
            case MoveDir.RIGHT:
                if (motionCircleCanvas.transform.position.x < (useAreaSize.x / 2) * useAreaScale.y)
                {
                    mainCamera.transform.Translate(new Vector3(cameraMoveSpeed, 0, 0));
                    motionCircleCanvas.transform.Translate(new Vector3(cameraMoveSpeed, 0, 0));
                }
                break;
            case MoveDir.ZOOM_IN:
                if (motionCircleCanvas.transform.position.z < useAreaPosition.z - cameraMoveSpeed)
                {
                    mainCamera.transform.Translate(new Vector3(0, 0, cameraMoveSpeed));
                    motionCircleCanvas.transform.Translate(new Vector3(0, 0, cameraMoveSpeed));
                }
                break;
            case MoveDir.ZOOM_OUT:
                mainCamera.transform.Translate(new Vector3(0, 0, -cameraMoveSpeed));
                motionCircleCanvas.transform.Translate(new Vector3(0, 0, -cameraMoveSpeed));
                break;
            case MoveDir.RESET_CAMERA:
                mainCamera.transform.position = stdCameraPos;
                break;
            default:
                break;
        }
    }
}
