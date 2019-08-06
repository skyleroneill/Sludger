using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions{
	public static Vector3 WithX(this Vector3 vect3, float newX){
		return new Vector3(newX, vect3.y, vect3.z);
	}
	public static Vector3 WithY(this Vector3 vect3, float newY){
		return new Vector3(vect3.x, newY, vect3.z);
	}
	public static Vector3 WithZ(this Vector3 vect3, float newZ){
		return new Vector3(vect3.x, vect3.y, newZ);
	}
	public static Vector3 WithXYZ(this Vector3 vect3, float newX, float newY, float newZ){
		return new Vector3(newX, newY, newZ);
	}
	public static Vector3 WithXYZ(this Vector3 vect3, float newVal){
		return new Vector3(newVal, newVal, newVal);
	}
	public static Vector3 WithAddedX(this Vector3 vect3, float newX){
		return new Vector3(newX+vect3.x, vect3.y, vect3.z);
	}
	public static Vector3 WithAddedY(this Vector3 vect3, float newY){
		return new Vector3(vect3.x, newY+vect3.y, vect3.z);
	}
	public static Vector3 WithAddedZ(this Vector3 vect3, float newZ){
		return new Vector3(vect3.x, vect3.y, newZ+vect3.z);
	}
	public static Vector3 WithAddedXYZ(this Vector3 vect3, float newX, float newY, float newZ){
		return new Vector3(newX+vect3.x, newY+vect3.y, newZ+vect3.z);
	}
	public static Vector3 WithAddedXYZ(this Vector3 vect3, float newVal){
		return new Vector3(newVal+vect3.x, newVal+vect3.y, newVal+vect3.z);
	}
	public static Vector3 ToVector3(this Vector2 vect2){
		return new Vector3(vect2.x, 0, vect2.y);
	}
	public static Vector2 ToVector3(this Vector3 vect3){
		return new Vector2(vect3.x, vect3.z);
	}
}	