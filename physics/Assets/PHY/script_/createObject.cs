using UnityEngine;
using System.Collections;
using System;

public class createObject : MonoBehaviour {
	private main MainObject;
	public GameObject curObject;
	public GameObject mesh;
	public GameObject targetObject;
	private Vector3d position;
	public GameObject target;
	private float curMass = 1;
 	private float curSize = 1;
 	private float speedPercent = 1;
	public GameObject planet;
	private bool isTarget;
	
	// Use this for initialization
	void Start () {
		MainObject = GameObject.FindGameObjectWithTag("Main").GetComponent<main>();
		isTarget = false;
	
	}
    private bool toggleMesh = false;
	private bool isFroze;
    private bool saveVelocity = true;
    private bool togglePause = false;
	private bool oldTogglePause = false;
	private bool inverseFlag = false;
	
	private int TimePositionX = 0;
	private int TimePositionY = 110;
	private int CreatePositionX = 0;
	private int CreatePositionY = 0;
	
	void OnGUI(){
        GUI.Box(new Rect(CreatePositionX, CreatePositionY, 270, 105), "Create object");
		
        if (GUI.Button(new Rect(140 + CreatePositionX, 70 + CreatePositionY, 50, 25), "Create") && isTarget){
        	GameObject temp = (GameObject)Instantiate(planet, position.toVector3(), Quaternion.identity);
			isTarget = false;
			if(togglePause){
				temp.GetComponent<law_of_gravity>().OnPauseGame();	
			}
			temp.GetComponent<law_of_gravity>().mass = curMass;
			temp.GetComponent<law_of_gravity>().size = curSize;
			temp.GetComponent<law_of_gravity>().fix = isFroze;
		}
		
        if (GUI.Button(new Rect(200 + CreatePositionX, 40 + CreatePositionY, 50, 25), "Delete") && curObject != null && curObject.tag == "Phy"){
			Destroy(curObject);
			curObject = null;
		}
		
        if (GUI.Button(new Rect(200 + CreatePositionX, 70 + CreatePositionY, 50, 25), "Update") && curObject != null && curObject.tag == "Phy"){
			curObject.GetComponent<law_of_gravity>().mass = curMass;
			curObject.GetComponent<law_of_gravity>().size = curSize;
			curObject.GetComponent<law_of_gravity>().fix = isFroze;
		}
		GUI.Label(new Rect (70 + CreatePositionX, 40 + CreatePositionY, 50, 20), "Mass");   
		
		string tempMass = GUI.TextField(new Rect(10 + CreatePositionX, 40 + CreatePositionY, 60, 20), curMass.ToString("#0.00000"), 25);
		if (float.TryParse(tempMass, out curMass)){
	         curMass = Single.Parse(tempMass);
	    }
		
		GUI.Label(new Rect (70 + CreatePositionX, 60 + CreatePositionY, 50, 20), "Size");    
		string tempSize = GUI.TextField(new Rect(10 + CreatePositionX, 60 + CreatePositionY, 60, 20), curSize.ToString("#0.00000"), 25);

		if (float.TryParse(tempSize, out curSize)){
	         curSize = Single.Parse(tempSize);
	    }
        toggleMesh = GUI.Toggle(new Rect(10 + CreatePositionX, 20 + CreatePositionY, 100, 30), toggleMesh, "Plane");
        togglePause = GUI.Toggle(new Rect(10 + CreatePositionX, 2 + CreatePositionY, 100, 30), togglePause, "Pause");
        isFroze = GUI.Toggle(new Rect(115 + CreatePositionX, 20 + CreatePositionY, 100, 30), isFroze, "Froze");
	
		
		
		
		GUI.Box(new Rect(275, 0, 245, 105), "Force");
        saveVelocity = GUI.Toggle(new Rect(10 + 280, 45, 100, 30), saveVelocity, "Save velocity");
		speedPercent = GUI.HorizontalScrollbar(new Rect(20 + 270, 25, 215, 30), speedPercent, 0.00F, 0.05F, 2.0F);	
        if (GUI.Button(new Rect(35 + 140 + 270, 70, 60, 25), "Select")){
			if(targetObject != null){
				(targetObject.GetComponent("law_of_gravity") as law_of_gravity).Select(0);
			}
			if(curObject != null && curObject.tag == "Phy"){
				targetObject = curObject;
				(curObject.GetComponent("law_of_gravity") as law_of_gravity).Select(2);
				curObject = null;
			}
			else{
				targetObject = null;
			}
		}	
        if (GUI.Button(new Rect(35 + 70 + 270, 70, 60, 25), "Push")){
			if(curObject != null && curObject.tag == "Phy" && targetObject != null && targetObject != curObject){
				double M = targetObject.GetComponent<law_of_gravity>().mass;
				double Dis = Vector3.Distance(targetObject.transform.position, curObject.transform.position);
				double S = Mathd.Sqrt(MainObject.GravityConst * M / Dis);
				Vector3d directionVector = Vector3d.Cross((Vector3d)curObject.transform.up, (Vector3d)(targetObject.transform.position - curObject.transform.position)).normalized;
				Vector3d veclocity;
				if(saveVelocity){
					veclocity = directionVector * S * speedPercent + targetObject.GetComponent<myPhy>().velocity;
				}
				else{
					veclocity = directionVector * S * speedPercent;
				}
				curObject.GetComponent<law_of_gravity>().equatVelocity(veclocity);
			}
		}
		
	
		
        GUI.Box(new Rect(TimePositionX, TimePositionY, 520, 50), "Scene");
		Time.timeScale = GUI.HorizontalScrollbar(new Rect(10 + TimePositionX, 25 + TimePositionY, 200, 30), Time.timeScale, 0.05F, 0F, 5.0F);	
        /*if (GUI.Button(new Rect(70 + TimePositionX, 40 + TimePositionY, 90, 25), "Inverse time")){
			inverseFlag = !inverseFlag;
			MainObject.inverse(inverseFlag);
		}*/
		
		GUI.Label(new Rect (100 + TimePositionX, 5 + TimePositionY, 50, 20), "Time");   
		if (GUI.Button(new Rect(220 + TimePositionX, 20 + TimePositionY, 90, 25), "Start scene")){
			Application.LoadLevel(0);	
		}
		if (GUI.Button(new Rect(320 + TimePositionX, 20 + TimePositionY, 90, 25), "Moon 1")){
			Application.LoadLevel(1);	
		}
		if (GUI.Button(new Rect(420 + TimePositionX, 20 + TimePositionY, 90, 25), "Moon 2")){
			Application.LoadLevel(2);	
		}
    }
	
	// Update is called once per frame
	void Update () {
		if(isTarget){
			target.transform.position = position;
		}
		else{
			target.transform.position = new Vector3(0, 0, 231210);	
		}
		mesh.renderer.enabled = toggleMesh;	
		mesh.collider.enabled = toggleMesh;	

		if(oldTogglePause != togglePause){
			if(togglePause){
				MainObject.Pause();
			}
			else{
				MainObject.Resume();
			}
			oldTogglePause = togglePause;
		}
		
		if(Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
				if(hit.collider.gameObject.tag == "Phy"){
					if(curObject != null && curObject.tag == "Phy"){
						curObject.GetComponent<law_of_gravity>().Select(0);
					}
					curObject = hit.collider.gameObject;	
					curObject.GetComponent<law_of_gravity>().Select(1);
				}
				if(hit.collider.gameObject.tag == "Plane"){
					position = hit.point;
					isTarget = true;
					target.transform.localScale = Vector3d.one * curSize * 1.5;
				}
			}
		}
	}
}
