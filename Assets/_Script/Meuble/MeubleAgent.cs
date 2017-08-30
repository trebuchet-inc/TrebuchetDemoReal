using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class MeubleAgent : MonoBehaviour {
	public LayerMask physicalLayer;
	public Vector2 anchor;
	public float baseY = 0;

	NVRInteractableItem _obj;
	RaycastHit _hit;
	GameObject _highlightMeuble;
	Quaternion _targetRotation;
	Vector3 _targetPosition;
	float _deltaY;
	bool _isShowingHL;
	
	void Start () 
	{
		_obj = GetComponent<NVRInteractableItem>();
		_highlightMeuble = transform.Find("HighlightMeuble").gameObject;
	}
	
	void Update () 
	{
		if(_obj.IsAttached)
		{
			if(TestRaycast(out _targetPosition, out _targetRotation))
			{
				if(!_highlightMeuble.activeSelf)
				{
					_obj.AttachedHand.AimComponent.ShowAim();
					_highlightMeuble.SetActive(true);
				}
				_highlightMeuble.transform.rotation = _targetRotation;
				_highlightMeuble.transform.position = _targetPosition + anchor.y * _highlightMeuble.transform.up;

				if(_obj.AttachedHand.UseButtonDown)
				{
					Snap();
					return;
				}
			}
			else if(_highlightMeuble.activeSelf)
			{
				_highlightMeuble.SetActive(false);
			}

			_deltaY -= _obj.AttachedHand.UseButtonAxisVector.x;
		}
		else if(_highlightMeuble.activeSelf)
		{
			_highlightMeuble.SetActive(false);
		}
	}

	void Snap()
	{
		_targetPosition = _highlightMeuble.transform.position;
		_targetRotation = _highlightMeuble.transform.rotation;
		_obj.EndInteraction(_obj.AttachedHand);
		transform.rotation = _targetRotation;
		transform.position = _targetPosition;
		_highlightMeuble.SetActive(false);
		_deltaY = 0;
	}
	bool TestRaycast(out Vector3 pos, out Quaternion rot)
	{
		Transform handTransform = _obj.AttachedHand.transform;
		if(Physics.Raycast(handTransform.position, handTransform.forward, out _hit, 5, physicalLayer))
		{
			if(_hit.normal.y > 0.5)
			{
				pos = _hit.point;
				rot = Quaternion.LookRotation(handTransform.forward * -1);
				rot = Quaternion.Euler(new Vector3(0,rot.eulerAngles.y + baseY + _deltaY,0));
			}
			else
			{
				RaycastHit _groundHit;
				Vector3 o = _hit.point + anchor.x * _hit.normal;
				if(Physics.Raycast(o, Vector3.down, out _groundHit, 5))
				{
					pos = _groundHit.point;
					rot = Quaternion.LookRotation(_hit.normal);
				}
				else
				{
					pos = _hit.point;
					rot = Quaternion.LookRotation(handTransform.forward * -1);
					rot = Quaternion.Euler(new Vector3(0,rot.eulerAngles.y + baseY + _deltaY,0));
				}
			}
			return true;	
		}
		pos = Vector3.zero;
		rot = Quaternion.identity;
		return false;
	}
}
