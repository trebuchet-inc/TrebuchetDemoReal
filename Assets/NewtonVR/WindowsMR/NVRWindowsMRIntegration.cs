using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;

#if NVR_WindowsMR
using UnityEngine.XR;

namespace NewtonVR
{
	public class NVRWindowsMRIntegration : NVRIntegration 
	{
        public override void Initialize(NVRPlayer player)
		{
			Player = player;
			Player.gameObject.SetActive(false);

            Player.gameObject.AddComponent<WinMRManager>();
			Player.LeftHand.gameObject.AddComponent<WinMRTrackedObj>().node = XRNode.LeftHand;
			Player.RightHand.gameObject.AddComponent<WinMRTrackedObj>().node = XRNode.RightHand;

			Player.gameObject.SetActive(true);
		}

        public override Vector3 GetPlayspaceBounds()
		{
			Vector3 bounds;
			UnityEngine.Experimental.XR.Boundary.TryGetDimensions(out bounds);
			return bounds;
		}

        public override bool IsHmdPresent()
		{
			return XRDevice.isPresent;
		}
	}
}
#else
namespace NewtonVR
{
	public class NVRWindowsMRIntegration : NVRIntegration 
	{
        public override void Initialize(NVRPlayer player){}

        public override Vector3 GetPlayspaceBounds()
		{
			return Vector3.zero;
		}

        public override bool IsHmdPresent()
		{
			return false;
		}
	}
}
#endif