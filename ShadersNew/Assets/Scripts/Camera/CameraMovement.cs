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
	public float moveSpeed;

	private Camera mainCamera;
	private GameObject cameraHolder;
	private GameObject target;
	private float horizontalRotation;
	private float verticalRotation;
	private CameraManager camManger;
	private ManagersManager tManage;
	private Vector3 moveDirection = Vector3.zero;

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
		//Vector3 targetInfo = target.transform.transform.position;
	   // cameraHolder.transform.position = new Vector3(targetInfo.x , targetInfo.y , targetInfo.z );
		
		if (Input.GetKey(KeyCode.Mouse2))
		{
			Vector3 newRotation = new Vector3(0, horizontalRotation, verticalRotation);
			horizontalRotation += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
			newRotation = new Vector3(0, horizontalRotation, verticalRotation);
			cameraHolder.transform.localRotation = Quaternion.Euler(newRotation);		
			//Vector3 newRotation = new Vector3(0, horizontalRotation, verticalRotation);
			verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
			//float tempAngle = newRotation.z;

			//verticalRotation = tempAngle;
			newRotation = new Vector3(Mathf.Clamp(verticalRotation, vertMin, vertMax), horizontalRotation,0 );
			cameraHolder.transform.localRotation = Quaternion.Euler(newRotation);
			if (verticalRotation < vertMin)
			{
				verticalRotation = vertMin;
			}
			if (verticalRotation > vertMax){
				verticalRotation = vertMax;
			}
		}
		else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
		{
			Vector3 newRotation = new Vector3(0, horizontalRotation,verticalRotation);
			horizontalRotation -= Input.GetAxis("Horizontal") * mouseSensitivity * Time.deltaTime;
			newRotation = new Vector3(verticalRotation, horizontalRotation, 0);
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
		
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
		{
			Vector3 newRotation = new Vector3(0, horizontalRotation, verticalRotation);
			verticalRotation -= Input.GetAxis("Vertical") * mouseSensitivity * Time.deltaTime;
			//float tempAngle = newRotation.z;
			
			//verticalRotation = tempAngle;
			newRotation = new Vector3(Mathf.Clamp(verticalRotation, vertMin, vertMax), horizontalRotation, 0);
			cameraHolder.transform.localRotation = Quaternion.Euler(newRotation);
				if (verticalRotation < vertMin)
			{
					verticalRotation = vertMin;
			}
				if (verticalRotation > vertMax){
				verticalRotation = vertMax;
		}
		}

		// camera 3d movement
		if (Input.GetKey (KeyCode.A)) {
			transform.position = transform.position + (-transform.right * moveSpeed * Time.deltaTime);
		}if (Input.GetKey (KeyCode.D)) {
			transform.position = transform.position + (transform.right * moveSpeed * Time.deltaTime);
		}if (Input.GetKey (KeyCode.W)) {
			transform.position = transform.position + (transform.forward * moveSpeed * Time.deltaTime);
		}if (Input.GetKey (KeyCode.S)) {
			transform.position = transform.position + (-transform.forward * moveSpeed * Time.deltaTime);
		}
		transform.position = new Vector3(transform.position.x,0.5f,transform.position.z);

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