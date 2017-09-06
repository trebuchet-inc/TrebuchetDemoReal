using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NewtonVR;

public class NVRConstrainedItem : NVRInteractable {
	public bool rotationLock;
	public bool movementLock;

	Transform _origin;
	Vector3 _originPosition;
	Quaternion _originRotation;
	Vector3 _originLocalPosition;
	Vector3 _attachementPoint;

	Joint _joint;

	protected override void Start () 
	{
		base.Start();
		_origin = transform.Find("Origin");
		_joint = GetComponent<Joint>();
		setJoint();
	}

	public void setJoint()
	{
		_originPosition = transform.position;
		_originRotation = transform.rotation;
		_originLocalPosition = transform.localPosition;

		_joint.connectedAnchor = transform.position;

		rigidbody.centerOfMass = _originLocalPosition;
	}

	protected override void Update()
	{
		if(!movementLock)
		{
			Vector3 velocity = rigidbody.velocity;
			ConstrainPosition((ConfigurableJoint)_joint, velocity, out velocity);
			rigidbody.velocity = velocity;
		}
	}
	
	public override void InteractingUpdate(NVRHand hand)
	{
		base.InteractingUpdate(hand);
		_origin.position = _originPosition;
		_origin.rotation = _originRotation;
		Vector3 LocalHandPos = _origin.InverseTransformPoint(hand.transform.position) - _attachementPoint;
		Vector3 forward = hand.transform.position - transform.position;
		SetVelocity(Quaternion.LookRotation(forward).eulerAngles, LocalHandPos);
	}

	public override void BeginInteraction(NVRHand hand)
	{
		base.BeginInteraction(hand);
		_origin.position = transform.position;
		_origin.rotation = _originRotation;
		_attachementPoint = _origin.InverseTransformPoint(hand.transform.position);
	}

	public override void EndInteraction(NVRHand hand)
	{ 
		base.EndInteraction(hand);
	}

	void SetVelocity(Vector3 targetRot, Vector3 targetPos)
	{
		if(!movementLock)
		{
			ConfigurableJoint _confJoint = _joint as ConfigurableJoint;
			Vector3 velocity = (targetPos - transform.localPosition) * Time.deltaTime * 1000;

			ConstrainPosition(_confJoint, velocity, out velocity);
			rigidbody.velocity = new Vector3(_confJoint.xMotion != ConfigurableJointMotion.Locked ? velocity.x : 0,
											_confJoint.yMotion != ConfigurableJointMotion.Locked ? velocity.y : 0,
											_confJoint.zMotion != ConfigurableJointMotion.Locked ? velocity.z : 0);
		} 

		if(!rotationLock)
		{
			Vector3 torque = targetRot - transform.eulerAngles;

			torque.x = Math.Abs(torque.x) > 180 ? torque.x - 360 * Math.Sign(torque.x) : torque.x;
			torque.y = Math.Abs(torque.y) > 180 ? torque.y - 360 * Math.Sign(torque.y) : torque.y;
			torque.z = Math.Abs(torque.z) > 180 ? torque.z - 360 * Math.Sign(torque.z) : torque.z;
			torque *= Time.deltaTime * 100;

			rigidbody.angularVelocity = new Vector3(_joint.axis.x != 0 ? torque.x : 0,
													_joint.axis.y != 0 ? torque.y : 0,
													_joint.axis.z != 0 ? torque.z : 0);
		}
	}

	void ConstrainPosition(ConfigurableJoint confJoint, Vector3 velocity, out Vector3 newVelocity)
	{
		Vector3 pos = transform.localPosition;
		newVelocity = velocity;

		if(confJoint.xMotion != ConfigurableJointMotion.Locked && pos.x < _originLocalPosition.x)
		{
			pos.x = _originLocalPosition.x;
			newVelocity.x = 0;
		}

		if(confJoint.yMotion != ConfigurableJointMotion.Locked && pos.y < _originLocalPosition.z)
		{
			pos.y = _originLocalPosition.y;
			newVelocity.y = 0;
		}

		if(confJoint.zMotion != ConfigurableJointMotion.Locked && pos.z < _originLocalPosition.z)
		{
			pos.z = _originLocalPosition.z;
			newVelocity.z = 0;
		}

		transform.localPosition = pos;
	}
}