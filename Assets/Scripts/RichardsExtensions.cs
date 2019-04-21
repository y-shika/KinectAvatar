using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


public static class RichardsExtensions {

	/// Convert color to hex
	public static string GetHexString ( this Color co ) {
		Color32 color = co;
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return hex;
	}

	/// Disable all colliders on a gameobject
	static public void DisableColliders ( this GameObject go ) {
		foreach ( var c in go.GetComponentsInChildren<Collider>() ) {
			c.enabled = false;
		}
	} 
	
	/// Convert second to MS
	static public int ToMS ( this float f ) {
		return (int)f*1000;
	}
	
	/// <summary>
	/// Gets or add a component. Usage example:
	/// BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
	/// </summary>
	static public T GetOrAddComponent<T> (this Component child) where T: Component {
		
		T result = child.GetComponent<T>();
		if (result == null) {
			result = child.gameObject.AddComponent<T>();
		}
		return result;
	}
	
	/// Destroy all children of a given transform;
	public static void DestroyChildren ( this Transform transform, bool forceImmediate = false ) {
		foreach ( Transform child in transform ) {
			child.gameObject.SetActive ( false );
			if( Application.isEditor || forceImmediate ){
				GameObject.DestroyImmediate ( child.gameObject );
			} else {
				GameObject.Destroy ( child.gameObject );
			}
		}
	}

	/// Set all children of a transform active
	public static void SetChildrenActive ( this Transform transform, bool active ){
		foreach ( Transform child in transform ) {
			child.gameObject.SetActive ( active );			
		}
	}
	
	/// Set active with delay
	public static void SetActive ( this GameObject me, bool setActive, float t ){
		
		me.SetActive( setActive );
		
	}

	/// Set active with delay
	public static IEnumerator IESetActive ( this GameObject me, bool SetActive, float t) {
		yield return new WaitForSeconds ( t);
		me.SetActive(SetActive);
	}

	/// Perform callback after a delay
	public static void DelayAction( this MonoBehaviour me, float delay, System.Action action  ){						
		if ( me != null ) {
			me.StartCoroutine(IEDelayAction(delay,action));
		}
    }
	static IEnumerator IEDelayAction( float delay, System.Action action ){				
        yield return new WaitForSeconds(delay);
        action();
    }

    /// Easy way to use "folder" game objects for sorting
    public static GameObject FindOrMakeGameobj ( string name ) {
		var go = GameObject.Find (name) ?? new GameObject (name);
		return go;
    }

	public static void AddOnce<T>(this List<T> lst, T data)
	{
		if ( !lst.Contains(data) ){
			lst.Add(data);
		}
	}

	public static string RemoveLineEndings(this string value)
	{
	    if(String.IsNullOrEmpty(value))
	    {
	        return value;
	    }
	    string lineSeparator = ((char) 0x2028).ToString();
	    string paragraphSeparator = ((char)0x2029).ToString();

	    return value.Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace(lineSeparator, string.Empty).Replace(paragraphSeparator, string.Empty);
	}

	public static void SetLastFrame ( this Animator animator, string name ){
		animator.Play(Animator.StringToHash(name),0,1f);
	}

	public static bool HasIndex ( this Array array, int idx ){
		return idx < array.Length && idx >= 0;
	}

	public static bool HasIndex ( this IList array, int idx ){				
		
		return  idx < array.Count && idx >= 0;
	}

//	
//	/// Reposition grid with X number of rows
//	public static void Reposition ( this UIGrid me, int cols, int rows ) {
//		me.Reposition();
//		return;
//		
//		int childs = 0;
//		foreach ( Transform c in me.transform ) {
//			if ( c.gameObject.activeSelf ) {
//				childs ++;
//			}
//		}
//		int area = rows * cols;		
//		int max = ( childs + rows - 1 ) / rows;
//		me.maxPerLine = Mathf.Clamp( max, cols, max );		
//		me.Reposition ();		
//	}
}
