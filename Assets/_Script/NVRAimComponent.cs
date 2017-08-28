using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NVRAimComponent : MonoBehaviour {
	GameObject _aimingLine;
	
	void Start ()
	{
		_aimingLine = GetComponentInChildren<LineRenderer>().gameObject;
		HideAim();
	}
	
	public void ShowAim ()
	{
		_aimingLine.SetActive(true);
	}

	public void HideAim ()
	{
		_aimingLine.SetActive(false);
	}
}
