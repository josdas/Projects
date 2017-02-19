using UnityEngine;
using System.Collections;

public class myPhy : MonoBehaviour {
	/*public class myVector{
		public double x;
		public double y;
		public double z;
		public static myVector zero = new myVector(0, 0, 0);

		public myVector() : this(0, 0, 0){
    		
		}
		public myVector(double X, double Y, double Z){
			x = X;
			y = Y;
			z = Z;
    	}
		public myVector(Vector3 T){
			x = T.x;
			y = T.y;
			z = T.z;
    	}
		public static myVector operator +(myVector c1, myVector c2)  {  
       		return new myVector(c1.x + c2.x, c1.y + c2.y, c1.z + c2.z);  
    	}
		public static Vector3 operator +(Vector3 c1, myVector c2)  {  
       		return new Vector3(c1.x + (float)c2.x, c1.y + (float)c2.y, c1.z + (float)c2.z);  
    	}
		public static Vector3 operator -(Vector3 c1, myVector c2)  {  
       		return new Vector3(c1.x - (float)c2.x, c1.y - (float)c2.y, c1.z - (float)c2.z);  
    	}
		public static myVector operator -(myVector c1, myVector c2)  {  
       		return new myVector(c1.x - c2.x, c1.y - c2.y, c1.z - c2.z);  
    	}
		public static myVector operator /(myVector c1, double l)  {  
       		return new myVector(c1.x / l, c1.y / l, c1.z / l);  
    	}
		public static myVector operator *(myVector c1, double l)  {  
       		return new myVector(c1.x * l, c1.y * l, c1.z * l);  
    	}
		public static double Distance(myVector c1, myVector c2){
			return Mathf.Sqrt((float)((c1.x - c2.x) * (c1.x - c2.x) + (c1.y - c2.y) * (c1.y - c2.y) + (c1.z - c2.z) * (c1.z - c2.z)));
		}
		public static double Distance(Vector3 c1, Vector3 c2){
			return Distance(new myVector(c1), new myVector(c2));
		}
		public static myVector Cross(myVector a, myVector b){
			return new myVector(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
		}
		public myVector normalized(){
			return this / Distance(this, zero);
		}
	}*/
	
	
	public Vector3d force = Vector3d.zero;
	public Vector3d acceleration = Vector3d.zero;
	public Vector3d velocity = Vector3d.zero;
	public bool inversTime = false;
	public bool isKinematic = false;
	public double mass = 1;
	
	public void AddForce(Vector3d tempForce){
		force = tempForce;
	}
	public void updPhy(){
		acceleration = force / mass;
		double time = Time.deltaTime;
		if(!inversTime){
			velocity += acceleration * time;
			gameObject.transform.position = (Vector3d)gameObject.transform.position + velocity * time;
		}
		else{
			velocity -= acceleration * time;
			gameObject.transform.position = (Vector3d)gameObject.transform.position - velocity * time;
		}
		force = Vector3d.zero;	
	}
}
