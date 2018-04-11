using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	public static CameraManager instance;
	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != null) {
			Destroy (this);
		}
	}
	public ManagersManager tManage;
	public CameraMovement camMove;

	void Start () 
	{
		tManage = ManagersManager.manager;
		tManage.tCamManage = instance;
		camMove = CameraMovement.instance;
	}

	public void changeCamera()
	{
		camMove.ChangeCameraTarget ();
	}

}
