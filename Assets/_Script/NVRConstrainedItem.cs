using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NewtonVR;

[Serializable]
public class AngleConstraints
{
	public bool X;
	public bool Y;
	public bool Z;
}

[Serializable]
public class MovementConstraints
{
	public bool X;
	public bool Y;
	public bool Z;
}

public class NVRConstrainedItem : NVRInteractable {
	public AngleConstraints angleConstraints;
	public float maxAngle;
	//public float ;
	public MovementConstraints movementConstraints;
	public float maxDistance;

	Transform _origin;
	Vector3 _originPosition;
	Quaternion _originRotation;
	Vector3 _localOriginPosition;
	Vector3 _localOriginRotation;

	Vector3 _constainedPosition;
	Vector3 _constainedRotation;

	Vector3 _attachementPoint;

	bool _angleConstrained
	{
		get
		{
			return angleConstraints.X || angleConstraints.Y || angleConstraints.Z;
		}
	}

	bool _movementConstrained
	{
		get
		{
			return movementConstraints.X || movementConstraints.Y || movementConstraints.Z;
		}
	}

	protected override void Start () 
	{
		base.Start();
		_origin = transform.Find("Origin");
		Rigidbody.position = transform.position;
		setJoint();
	}

	public void setJoint()
	{
		_localOriginPosition = transform.localPosition;
		_localOriginRotation = transform.localEulerAngles;

		_originPosition = transform.position;
		_originRotation = transform.rotation;

		_constainedPosition = _localOriginPosition;
		_constainedRotation = _localOriginRotation;

		// if(angleConstraints.X) transform.localEulerAngles += Vector3.right * maxAngle * -0.5f;
		// if(angleConstraints.Y) transform.localEulerAngles += Vector3.up * maxAngle * -0.5f;
		// if(angleConstraints.Z) transform.localEulerAngles += Vector3.forward * maxAngle * -0.5f;

		print(transform.localEulerAngles);
	}
	
	protected override void Update () 
	{
		base.Update();
		if(!IsAttached)
		{
			MoveWithConstraint(transform.localEulerAngles, transform.localPosition);
		}
	}

	public override void InteractingUpdate(NVRHand hand)
	{
		base.InteractingUpdate(hand);
		_origin.position = _originPosition;
		_origin.rotation = _originRotation;
		Vector3 LocalHandPos = _origin.InverseTransformPoint(hand.transform.position) - _attachementPoint;
		Vector3 forward = hand.transform.position - transform.position;
		MoveWithConstraint(Quaternion.LookRotation(forward).eulerAngles, LocalHandPos);
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
		Rigidbody.velocity = Vector3.zero;
		Rigidbody.angularVelocity = Vector3.zero;
	}

	void MoveWithConstraint(Vector3 targetRot, Vector3 targetPos)
	{
		// if(_angleConstrained)
		// {
		// 	_constainedRotation = ApplyRotationConstraints(targetRot);
		// }
		// transform.localEulerAngles = _constainedRotation;

		// if(_movementConstrained)
		// {
		// 	_constainedPosition = ApplyPositionConstraints(targetPos);
		// }
		// transform.localPosition = _constainedPosition;

		if(_angleConstrained)
		{
			_constainedRotation = ApplyRotationConstraints(targetRot);
		}
		transform.localEulerAngles = _constainedRotation;

		if(_movementConstrained)
		{
			_constainedPosition = ApplyPositionConstraints(targetPos);
		}
		transform.localPosition = _constainedPosition;
		
	}

	Vector3 ApplyRotationConstraints(Vector3 targetRot)
	{
		if(angleConstraints.X && (targetRot.x <= maxAngle || targetRot.x >= 360 - maxAngle))_constainedRotation.x = targetRot.x;
		if(angleConstraints.Y && (targetRot.y <= maxAngle || targetRot.y >= 360 - maxAngle))_constainedRotation.y = targetRot.y;
		if(angleConstraints.Z && (targetRot.z <= maxAngle || targetRot.z >= 360 - maxAngle) )_constainedRotation.z = targetRot.z;
		return _constainedRotation;
	}

	Vector3 ApplyPositionConstraints(Vector3 targetPos)
	{
		Vector3 distance = targetPos - _localOriginPosition;
		if(movementConstraints.X && distance.x <= maxDistance && distance.x > 0)_constainedPosition.x = targetPos.x;
		if(movementConstraints.Y && distance.y <= maxDistance && distance.y > 0)_constainedPosition.y = targetPos.y;
		if(movementConstraints.Z && distance.z <= maxDistance && distance.z > 0)_constainedPosition.z = targetPos.z;
		return _constainedPosition;
	}
}
