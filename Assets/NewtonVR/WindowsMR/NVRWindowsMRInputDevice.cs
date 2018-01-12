using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

#if NVR_WindowsMR

namespace NewtonVR
{
	public class NVRWindowsMRInputDevice : NVRInputDevice 
	{
        UnityEngine.XR.WSA.Input.InteractionSourceHandedness XRHand;

        private Dictionary<NVRButtons, WinMRInput.Inputs> ButtonMapping = new Dictionary<NVRButtons, WinMRInput.Inputs>(new NVRButtonsComparer());

        public override void Initialize(NVRHand hand)
        {
            Hand = hand;
            XRHand = hand.IsLeft ? UnityEngine.XR.WSA.Input.InteractionSourceHandedness.Left : UnityEngine.XR.WSA.Input.InteractionSourceHandedness.Right;
            SetupButtonMapping();
        }

        protected virtual void SetupButtonMapping()
        {
            ButtonMapping.Add(NVRButtons.Trigger, WinMRInput.Inputs.Trigger);
            ButtonMapping.Add(NVRButtons.Grip, WinMRInput.Inputs.Grip);
            ButtonMapping.Add(NVRButtons.Stick, WinMRInput.Inputs.Thumbstick);
            ButtonMapping.Add(NVRButtons.ApplicationMenu, WinMRInput.Inputs.Menu);
            ButtonMapping.Add(NVRButtons.Touchpad, WinMRInput.Inputs.TouchPad);
            ButtonMapping.Add(NVRButtons.DPad_Up, WinMRInput.Inputs.TouchPadUp);
            ButtonMapping.Add(NVRButtons.DPad_Down, WinMRInput.Inputs.TouchPadDown);
            ButtonMapping.Add(NVRButtons.DPad_Left, WinMRInput.Inputs.TouchPadLeft);
            ButtonMapping.Add(NVRButtons.DPad_Right, WinMRInput.Inputs.TouchPadDown);
        }

        private WinMRInput.Inputs GetButtonMap(NVRButtons button)
        {
            if (ButtonMapping.ContainsKey(button) == false)
            {
                return WinMRInput.Inputs.None;
            }
            return ButtonMapping[button];
        }

        public override bool IsCurrentlyTracked
        {
            get
            {
                return WinMRInput.GetTracked(XRHand);
            }
        }

        public override Collider[] SetupDefaultPhysicalColliders(Transform ModelParent)
		{
			return new Collider[0];
		}

        public override GameObject SetupDefaultRenderModel()
		{
			return new GameObject();
		}

        public override bool ReadyToInitialize()
		{
			return true;
		}

        public override Collider[] SetupDefaultColliders()
		{
			return new Collider[0];
		}

        public override string GetDeviceName()
		{
			return "Custom";
		}

        public override void TriggerHapticPulse(ushort durationMicroSec = 500, NVRButtons button = NVRButtons.Touchpad){}

        public override float GetAxis1D(NVRButtons button)
		{
            return WinMRInput.GetAxis1D(GetButtonMap(button), XRHand);
        }

        public override Vector2 GetAxis2D(NVRButtons button)
		{
			return WinMRInput.GetAxis2D(GetButtonMap(button), XRHand);
        }

        public override bool GetPressDown(NVRButtons button)
		{
			return WinMRInput.GetDown(GetButtonMap(button), XRHand);
        }

        public override bool GetPressUp(NVRButtons button)
		{
			return WinMRInput.GetUp(GetButtonMap(button), XRHand);
        }

        public override bool GetPress(NVRButtons button)
		{
			return WinMRInput.GetPressed(GetButtonMap(button), XRHand);
        }

        public override bool GetTouchDown(NVRButtons button)
		{
			return WinMRInput.GetTouchDown(GetButtonMap(button), XRHand);
        }

        public override bool GetTouchUp(NVRButtons button)
		{
			return WinMRInput.GetTouchUp(GetButtonMap(button), XRHand);
        }

        public override bool GetTouch(NVRButtons button)
		{
			return WinMRInput.GetTouch(GetButtonMap(button), XRHand);
        }

        public override bool GetNearTouchDown(NVRButtons button)
		{
			return false;
		}

        public override bool GetNearTouchUp(NVRButtons button)
		{
			return false;
		}

        public override bool GetNearTouch(NVRButtons button)
		{
			return false;
		}
	}
}
#else
namespace NewtonVR
{
	public class  NVRWindowsMRInputDevice : NVRInputDevice
	{
        public override bool IsCurrentlyTracked
        {
            get
            {
                PrintNotEnabledError();
                return false;
            }
        }

        public override float GetAxis1D(NVRButtons button)
        {
            PrintNotEnabledError();
            return 0;
        }

        public override Vector2 GetAxis2D(NVRButtons button)
        {
            PrintNotEnabledError();
            return Vector2.zero;
        }

        public override string GetDeviceName()
        {
            PrintNotEnabledError();
            return "";
        }

        public override bool GetNearTouch(NVRButtons button)
        {
            PrintNotEnabledError();
            return false;
        }

        public override bool GetNearTouchDown(NVRButtons button)
        {
            PrintNotEnabledError();
            return false;
        }

        public override bool GetNearTouchUp(NVRButtons button)
        {
            PrintNotEnabledError();
            return false;
        }

        public override bool GetPress(NVRButtons button)
        {
            PrintNotEnabledError();
            return false;
        }

        public override bool GetPressDown(NVRButtons button)
        {
            PrintNotEnabledError();
            return false;
        }

        public override bool GetPressUp(NVRButtons button)
        {
            PrintNotEnabledError();
            return false;
        }

        public override bool GetTouch(NVRButtons button)
        {
            PrintNotEnabledError();
            return false;
        }

        public override bool GetTouchDown(NVRButtons button)
        {
            PrintNotEnabledError();
            return false;
        }

        public override bool GetTouchUp(NVRButtons button)
        {
            PrintNotEnabledError();
            return false;
        }

        public override bool ReadyToInitialize()
        {
            PrintNotEnabledError();
            return false;
        }

        public override Collider[] SetupDefaultColliders()
        {
            PrintNotEnabledError();
            return null;
        }

        public override Collider[] SetupDefaultPhysicalColliders(Transform ModelParent)
        {
            PrintNotEnabledError();
            return null;
        }

        public override GameObject SetupDefaultRenderModel()
        {
            PrintNotEnabledError();
            return null;
        }

        public override void TriggerHapticPulse(ushort durationMicroSec = 500, NVRButtons button = NVRButtons.Touchpad)
        {
            PrintNotEnabledError();
        }

        private void PrintNotEnabledError()
        {
            Debug.LogError("Enable WinMR in NVRPlayer to allow steamvr calls.");
        }
	}
}
#endif