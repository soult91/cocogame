using UnityEngine;
using System.Collections;
using System;

public class GestureHandControl : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    public GUIText GestureInfo;

    private bool leftHandCursor;
    private bool rightHandCursor;


    public bool IsLeftHandCursor()
    {
        if(leftHandCursor){
            leftHandCursor = false;
            return true;
        }
        return false;
    }

    public bool IsRightHandCursor()
    {
        if(rightHandCursor){
            rightHandCursor = false;
            return true;
        }
        return false;
    }

    public void UserDetected(uint userId, int userIndex)
    {
        KinectManager manager = KinectManager.Instance;

        manager.DetectGesture(userId, KinectGestures.Gestures.LeftHandCursor);
        manager.DetectGesture(userId, KinectGestures.Gestures.RightHandCursor);


        if(GestureInfo != null){
            GestureInfo.GetComponent<GUIText>().text = "Close your hand to click objects.";
            Debug.Log("hand cursor");
        }
    }

    public void UserLost(uint userId, int userIndex)
    {
        if(GestureInfo != null){
            GestureInfo.GetComponent<GUIText>().text = string.Empty;
        }
    }

    public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture,
                                    float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screePos)
    {
        //dont do anything here
    }

    public bool GestureCompleted(uint userId, int userIndex, KinectGestures.Gestures gesture, 
    KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {
        string sGestureText = gesture + "detected";
        Debug.Log("gesture detected");
        if(GestureInfo != null){
            GestureInfo.GetComponent<GUIText>().text = sGestureText;
        } 

        if(gesture == KinectGestures.Gestures.LeftHandCursor){
            leftHandCursor = true;
        }
        if(gesture == KinectGestures.Gestures.RightHandCursor){
            rightHandCursor = true;
        }

        return true;
    }

    public bool GestureCancelled(uint userId, int userIndex, KinectGestures.Gestures gestures, 
    KinectWrapper.NuiSkeletonPositionIndex  joint)
    {
        //dont do anything here, just reset the gesture state
        return true;
    }
}
