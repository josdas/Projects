using UnityEngine;
using System.Collections;

public class law_of_gravity : myPhy {
	private main MainObject;
	public float size = 1;
	public bool fix = false;
	public Color color, basicColor;
	private int typeSelect;
	public ParticleSystem myParticle;
	public Vector3 startVelocity;
	
	// Use this for initialization
	void Start () {
		velocity = startVelocity;
		MainObject = GameObject.FindGameObjectWithTag("Main").GetComponent<main>();
		basicColor = color = MainObject.colorObject[Random.Range(0, MainObject.colorObject.Length)];
	}
	// -8.853541	0.07951737 // -12.81867  5.980037
	// -7.843163 0.5065749 // -11.80829 6.407094
	
	double getForce(GameObject to){
		double massFirst = mass;
		double massSecond = to.GetComponent<law_of_gravity>().mass;
		double averageSize = (size + to.GetComponent<law_of_gravity>().size);
		double distanse = Vector3d.Distance(gameObject.transform.position, to.transform.position); //Mathf.Max((float)averageSize / 1.1f, Vector3.Distance(gameObject.transform.position, to.transform.position));
		double force = (double)(MainObject.GravityConst * massFirst * massSecond / distanse / distanse);
		return force;
	}
	
	Vector3d getGravity(GameObject to){
		Vector3d target = to.transform.position - gameObject.transform.position;
		double F = getForce(to);
		return target.normalized * F;
	}
	
	void updateProperties(){
		myParticle.startSize = size * 0.2f;
		myParticle.startLifetime = size * 20f;
		gameObject.transform.localScale = new Vector3(size, size, size);
		gameObject.renderer.material.color = color;
	}
	
	void updateForce(){
		Vector3d force = new Vector3d(0, 0, 0);
		for(int i = 0; i < MainObject.phyObject.Length; i++){
			if(MainObject.phyObject[i] != null && MainObject.phyObject[i] != gameObject){
				force += getGravity(MainObject.phyObject[i]);
			}
		}
		AddForce(force);
		if(!isKinematic && !fix){
			updPhy();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		startVelocity = velocity;
		if(Vector3.Distance(transform.position, MainObject.transform.position) > 100000){
			Destroy(gameObject);
			return;
		}
		updateProperties();
		updateForce();
		
	}
	
     
    public void OnPauseGame() {
        isKinematic = true;
    }
 
    public void OnResumeGame() {
        isKinematic = false;
    }
	
	public void equatVelocity(Vector3 T){
		velocity = T;
	}
	
	public void Select(int t){
		if(t == 0){
			color = basicColor;	
		}
		if(t == 1){
			color = Color.red;
		}
		if(t == 2){
			color = Color.black;	
		}
	}
	
}
