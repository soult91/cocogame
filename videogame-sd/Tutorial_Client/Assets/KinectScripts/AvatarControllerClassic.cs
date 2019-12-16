using UnityEngine;
//using Windows.Kinect;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text; 

public class AvatarControllerClassic : AvatarController
{	
	// Public variables that will get matched to bones. If empty, the Kinect will simply not track it.
	public Transform HipCenter;
	public Transform Spine;
	public Transform Neck;
	public Transform Head;
	
	public Transform LeftClavicle;
	public Transform LeftUpperArm;
	public Transform LeftElbow; 
	public Transform LeftHand;
	public Transform LeftWrist;
	public Transform LeftFingers;
	public Transform LeftCapitate;
	public Transform LeftPisiform;
	public Transform LeftThumb;
	public Transform LeftThumb1;
	public Transform LeftThumb2;
	public Transform LeftIndex;
	public Transform LeftIndex1;
	public Transform LeftIndex2;

	public Transform LeftShoulder;
	public Transform LeftScapule;
	
	public Transform RightClavicle;
	public Transform RightUpperArm;
	public Transform RightElbow;
	public Transform RightHand;
	public Transform RightWrist;
	public Transform RightFingers;
	public Transform RightCapitate;
	public Transform RightPisiform;
	public Transform RightThumb;
	public Transform RightThumb1;
	public Transform RightThumb2;
	public Transform RightIndex;
	public Transform RightIndex1;
	public Transform RightIndex2;

	public Transform RightShoulder;
	public Transform RightScapule;
		
	public Transform LeftThigh;
	public Transform LeftKnee;
	public Transform LeftFoot;
	private Transform LeftToes = null;
	
	public Transform RightThigh;
	public Transform RightKnee;
	public Transform RightFoot;
	private Transform RightToes = null;

	public Transform BodyRoot;
	public GameObject OffsetNode;
	

	// If the bones to be mapped have been declared, map that bone to the model.
	protected override void MapBones()
	{
		bones[0] = HipCenter;
		bones[1] = Spine;
		bones[2] = Neck;
		bones[3] = Head;
	
		bones[4] = LeftClavicle;
		bones[5] = LeftUpperArm;
		bones[6] = LeftElbow;
		bones[7] = LeftHand;
		
		bones[8] = LeftFingers;
	
		bones[9] = RightClavicle;
		bones[10] = RightUpperArm;
		bones[11] = RightElbow;
		bones[12] = RightHand;
		bones[13] = RightFingers;
	
		bones[14] = LeftThigh;
		bones[15] = LeftKnee;
		bones[16] = LeftFoot;
		bones[17] = LeftToes;
	
		bones[18] = RightThigh;
		bones[19] = RightKnee;
		bones[20] = RightFoot;
		bones[21] = RightToes;

		bones[22] = LeftWrist;
		bones[23] = RightWrist;
		bones[24] = LeftCapitate;
		bones[25] = LeftPisiform;
		bones[26] = LeftThumb;
		bones[27] = RightCapitate;
		bones[28] = RightPisiform;
		bones[29] = RightThumb;
		bones[30] = RightThumb1;
		bones[31] = RightThumb2;
		bones[32] = RightIndex;
		bones[33] = RightIndex1;
		bones[34] = RightShoulder;
		bones[35] = RightScapule;
		bones[36] = LeftIndex;
		bones[37] = LeftIndex1;
		bones[38] = LeftIndex2;
		bones[39] = LeftThumb;
		bones[40] = LeftThumb1;
		bones[41] = LeftThumb2;	
		bones[42] = LeftScapule;
		bones[43] = LeftShoulder;




		
 		// body root and offset
		bodyRoot = BodyRoot;
		offsetNode = OffsetNode;

		if(offsetNode == null)
		{
			offsetNode = new GameObject(name + "Ctrl") { layer = transform.gameObject.layer, tag = transform.gameObject.tag };
			offsetNode.transform.position = transform.position;
			offsetNode.transform.rotation = transform.rotation;
			offsetNode.transform.parent = transform.parent;
			
			transform.parent = offsetNode.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
		}

//		if(bodyRoot == null)
//		{
//			bodyRoot = transform;
//		}
	}
	
}

