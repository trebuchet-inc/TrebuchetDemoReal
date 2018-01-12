using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.XR.WSA;

public class WinMRTrackedObj : MonoBehaviour 
{
	public XRNode node;

	void Start () 
	{
		if(!XRSettings.enabled)
		{
			Debug.LogError("XR not enable");
			this.enabled = false;
		}
	}
	
	void Update () 
	{
		transform.localPosition = InputTracking.GetLocalPosition(node);
		transform.rotation = InputTracking.GetLocalRotation(node);
	}
}
