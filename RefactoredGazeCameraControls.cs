using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GazeAware))]
public class GazeCameraControls : MonoBehaviour, IPointerClickHandler {

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

    private GazeCameraControlSettings settings;

    //Variables set by CircularMotionSettings.cs
    private Transform mainCameraTransform;
	private Transform gazeCameraControlCanvasTransform;
    private Canvas mainUseCanvas;

    private float cameraMoveSpeed;
    //End of

    [SerializeField]
    public GazeAware gaze;
    [SerializeField]
    MoveDir moveDir;

    private Vector3 mainUseCanvasPosition;
    private Vector2 mainUseCanvasSize;
    private Vector3 mainUseCanvasScale;
    private Vector3 startCameraPos;

    /// <summary>
    /// Initialize the class, loading from CircleMotionSettings.
    /// </summary>
    void Awake()
    {
        settings = GameObject.FindGameObjectWithTag("MotionButtonSettings").GetComponent<CircularMotionSettings>();
        gaze = GetComponent<GazeAware>();

        this.mainCameraTransform = settings.mainCamera.transform;
        this.gazeCameraControlCanvasTransform = settings.gazeCamaraControlCanvas.transform;
        this.mainUseCanvas = settings.mainUseCanvas;
        this.cameraMoveSpeed = settings.cameraMoveSpeed;
        this.startCameraPos = settings.startCameraPos;
        this.mainUseCanvasPosition = mainUseCanvas.GetComponent<Transform>().localPosition;
        this.mainUseCanvasSize = mainUseCanvas.GetComponent<RectTransform>().sizeDelta;
        this.mainUseCanvasScale = mainUseCanvas.GetComponent<Transform>().localScale;

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
                if (gazeCameraControlCanvasTransform.position.y < mainUseCanvasSize.y / 2 * mainUseCanvasScale.x)
                {
					UpdateObjectAxisPosition(new Vector3(0, cameraMoveSpeed, 0));
                }
                break;
            case MoveDir.DOWN:
                if (gazeCameraControlCanvasTransform.position.y > -(mainUseCanvasSize.y / 2) * mainUseCanvasScale.x)
                {
					UpdateObjectAxisPosition(new Vector3(0, -cameraMoveSpeed, 0));
                }
                break;
            case MoveDir.LEFT:
                if (gazeCameraControlCanvasTransform.position.x > - (mainUseCanvasSize.x / 2) * mainUseCanvasScale.y)
                {
					UpdateObjectAxisPosition(new Vector3(-cameraMoveSpeed, 0, 0));
                }
                break;
            case MoveDir.RIGHT:
                if (gazeCameraControlCanvasTransform.position.x < (mainUseCanvasSize.x / 2) * mainUseCanvasScale.y)
                {
					UpdateObjectAxisPosition(new Vector3(cameraMoveSpeed, 0, 0));
                }
                break;
            case MoveDir.ZOOM_IN:
                if (gazeCameraControlCanvasTransform.position.z < useAreaPosition.z - cameraMoveSpeed)
                {
					UpdateObjectAxisPosition(new Vector3(0, 0, cameraMoveSpeed));
                }
                break;
            case MoveDir.ZOOM_OUT:
				UpdateObjectAxisPosition(new Vector3(0, 0, -cameraMoveSpeed));
                break;
            case MoveDir.RESET_CAMERA:
                mainCamera.transform.position = startCameraPos;
                break;
            default:
                break;
        }
    }
	
	private void UpdateObjectAxisPosition(Vector3 vec3)
	{
		mainCameraTransform.Translate(vec3);
		gazeCameraControlCanvasTransform.Translate(vec3);
	}
}