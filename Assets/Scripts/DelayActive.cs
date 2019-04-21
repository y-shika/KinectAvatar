using System.Collections;

using UnityEngine;

public class DelayActive : MonoBehaviour {

	public GameObject targ;
	public float delay;

	void Start () {
		StartCoroutine(IEstart());
	}
	

	IEnumerator IEstart () {
		yield return new WaitForSeconds(delay);
		targ.SetActive(true);
		
	}
}
