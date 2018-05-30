using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Tobii.Gaming;
using UnityEngine.UI;
using System;

//Code based on Tobii's "GazePlotter.cs". All theirs
public class EyegazeBrowserPointer : EyegazeGeneralPointer {

    [SerializeField]
    public GameObject mBrowser;
    [SerializeField]
    public GameObject mBrowserCanvas;

    
    public override void UpdateGazePointerPosition(GazePoint gazePoint)
    {
        Vector3 gazePointInWorld = ProjectToPlaneInWorld(gazePoint);
        mPointer.transform.position = Smoothify(gazePointInWorld);
        mPointer.transform.position = new Vector3(mPointer.transform.position.x, mPointer.transform.position.y, mBrowserCanvas.transform.position.z - 1);
        mPointer.transform.localScale = new Vector3(1 - (mCamera.transform.position.z / 100), 1 - (mCamera.transform.position.z / 100), 1);
    }
    

    /// <summary>
    /// Uses to world space pointer to send a mouse click to the browser.
    /// Translates positions from world space to browser transform.
    /// Sends a click+release event as mouse to browser
    /// </summary>
    public override void SendClickMessage()
    {
        var raycaster = mBrowserCanvas.GetComponent<GraphicRaycaster>();
        Vector2 localPos;
		RectTransform mBrowserRectTrns = mBrowser.transform as RectTransform;

        Vector3 pointerInWorldSpace = ProjectToPlaneInWorld(gazePoint);
        pointerInWorldSpace = Smoothify(pointerInWorldSpace);

        Vector3 pointerInBrowserSpace = mCamera.WorldToScreenPoint(pointerInBrowser);

        
        //Translate from screen space to local space in browser(Transform)
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mBrowserRectTrns, pointerInBrowserSpace, raycaster.eventCamera, out localPos);

        localPos.y = mBrowserRectTrns.rect.height - localPos.y;

        //Send mouse event to browser
        //Sends full press (Click+release), or else it will lock 
        mBrowser.GetComponent<SimpleWebBrowser.WebBrowser2D>().SendMouseButtonEvent((int)localPos.x, (int)localPos.y, MessageLibrary.MouseButton.Left, MessageLibrary.MouseEventType.ButtonDown);
        mBrowser.GetComponent<SimpleWebBrowser.WebBrowser2D>().SendMouseButtonEvent((int)localPos.x, (int)localPos.y, MessageLibrary.MouseButton.Left, MessageLibrary.MouseEventType.ButtonUp);
    }

}
