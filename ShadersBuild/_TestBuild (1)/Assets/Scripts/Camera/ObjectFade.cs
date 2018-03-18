using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectFade : MonoBehaviour
{
	private Transform player;
	private Transform cameraPos;
	private List<Transform> hiddenObjects = new List<Transform>();

	public LayerMask layerMask;

	// Use this for initialization
	void Start()
	{
		player = GameObject.Find("Player").transform;
		cameraPos = GameObject.Find("RayPoint").transform;
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 dir = player.position - cameraPos.position;
		float dist = dir.magnitude;
	
		RaycastHit[] hits = Physics.RaycastAll(cameraPos.position, dir, dist, layerMask);
		
		for (int i = 0; i < hits.Length; i++)
		{
			Transform currentHit = hits[i].transform;

			if (!hiddenObjects.Contains(currentHit))
			{
				hiddenObjects.Add(currentHit);
				MeshRenderer renderer = currentHit.GetComponent<MeshRenderer>();
				renderer.enabled = false;	
			
			}
		}

		for (int i = 0; i < hiddenObjects.Count; i++)
		{

			bool isHit = false;
			for (int j = 0; j < hits.Length; j++)
			{
				if (hits[j].transform == hiddenObjects[i])
				{
					isHit = true;
					break;
				}
			}
			
			if (!isHit)
			{
				Transform wasHidden = hiddenObjects[i];
				MeshRenderer hiddenRenderer = wasHidden.GetComponent<MeshRenderer>();
				hiddenRenderer.enabled = true;
				hiddenObjects.RemoveAt(i);
				i--;
			}
		}
		

	}
}
