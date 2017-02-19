using UnityEngine;
using System.Collections;

public class main : MonoBehaviour {
	public double GravityConst = 6.67428; // -11
	public Color[] colorObject;
	public GameObject[] phyObject;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("Phy");
		int size = 0;
		for(int i = 0; i < temp.Length; i++){
			if(temp[i] != null){
				size++;
			}
		}
		phyObject = new GameObject[size];
		size = 0;
		for(int i = 0; i < temp.Length; i++){
			if(temp[i] != null){
				phyObject[size] = temp[i];
				size++;
			}
		}
	}
	
	public void Pause(){
		for(int i = 0; i < phyObject.Length; i++){
			if(phyObject[i] != null){
				phyObject[i].GetComponent<law_of_gravity>().OnPauseGame();
			}	
		}
	}
	public void Resume(){
		for(int i = 0; i < phyObject.Length; i++){
			if(phyObject[i] != null){
				phyObject[i].GetComponent<law_of_gravity>().OnResumeGame();
			}	
		}
	}
	
	public void inverse(bool inv){
		for(int i = 0; i < phyObject.Length; i++){
			if(phyObject[i] != null){
				if(inv){
					phyObject[i].GetComponent<law_of_gravity>().inversTime = true;
				}
				else{
					phyObject[i].GetComponent<law_of_gravity>().inversTime = false;
				}
			}	
		}
	}
}
