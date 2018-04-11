using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
	public static CameraMovement instance;
	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != null) {
			Destroy (this);
		}
	}
	public float mouseSensitivity = 5;
	//public float cameraDistOffset = 10;
	public float ScrollSpeed;
	public float vertMin;
	public float vertMax;
	
	private Camera mainCamera;
	private GameObject cameraHolder;
	private GameObject target;
	private float horizontalRotation;
	private float verticalRotation;
	private CameraManager camManger;
	private ManagersManager tManage;
	
	// Use this for initialization
	void Start()
	{
		camManger = CameraManager.instance;
		tManage = ManagersManager.manager;
		mainCamera = GetComponentInChildren<Camera>();
		cameraHolder = gameObject;
		ChangeCameraTarget ();
		
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 targetInfo = target.transform.transform.position;
	    cameraHolder.transform.position = new Vector3(targetInfo.x , targetInfo.y , targetInfo.z );
		
		if (Input.GetKey(KeyCode.Mouse2))
		{
			Vector3 newRotation = new Vector3(0, horizontalRotation, verticalRotation);
			horizontalRotation += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
			newRotation = new Vector3(0, horizontalRotation, verticalRotation);
			cameraHolder.transform.localRotation = Quaternion.Euler(newRotation);		
		}
		else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
		{
			Vector3 newRotation = new Vector3(0, horizontalRotation,verticalRotation);
			horizontalRotation -= Input.GetAxis("Horizontal") * mouseSensitivity * Time.deltaTime;
			newRotation = new Vector3(0, horizontalRotation, verticalRotation);
			cameraHolder.transform.localRotation = Quaternion.Euler(newRotation);		
		}	
		
		if (Input.GetAxis("Mouse ScrollWheel") > 0  && Vector3.Distance(cameraHolder.transform.position, mainCamera.transform.position) > 5)
		{
			
			mainCamera.transform.position += mainCamera.transform.forward * Time.deltaTime * ScrollSpeed;
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0 && Vector3.Distance(cameraHolder.transform.position, mainCamera.transform.position) < 15)
		{
			mainCamera.transform.position -= mainCamera.transform.forward * Time.deltaTime * ScrollSpeed;
		}
		
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
		{
			Vector3 newRotation = new Vector3(0, horizontalRotation, verticalRotation);
			verticalRotation -= Input.GetAxis("Vertical") * mouseSensitivity * Time.deltaTime;
			//float tempAngle = newRotation.z;
			
			//verticalRotation = tempAngle;
			newRotation = new Vector3(0, horizontalRotation, Mathf.Clamp(verticalRotation, vertMin, vertMax));
			cameraHolder.transform.localRotation = Quaternion.Euler(newRotation);
				if (verticalRotation < vertMin)
			{
					verticalRotation = vertMin;
			}
				if (verticalRotation > vertMax){
				verticalRotation = vertMax;
		}
		}
	}

	public void ChangeCameraTarget()
	{
		if (tManage.tTurn.playersTurn == true)
		{
			target = PlayerManager.currentPlayer;
		}
		else if (tManage.tTurn.playersTurn == false)
		{
			target = EnemyManager.currentEnemy;
		}
	}
}