using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharaFace : MonoBehaviour {
  [SerializeField]
  float interval = 0.1f;

  // lip
  public class Lip {
    public bool isSync;
    public Coroutine routine;
    public int current;
  }
  Lip lip = new Lip();

  public virtual int maxLip { get { return 1; } }

  // expression
  public struct State {
    public bool canBlink;
    public bool mouthOpen;
  }

  public virtual int maxExp { get { return 1; } }
  public event Action<State> OnExpChanged;
  public string[] faceNames;

  protected virtual void Awake() {
    OnExpChanged += (exp) => {};
  }

  public void SetExpressionIntensity ( int index ){
  	
  }

  public void SetExpression(int index) {
    lip.current = ExpToLip(index);

    SetFace (index);
    if (!lip.isSync) {
      SetMouth (lip.current);
    }

    OnExpChanged (ToState(index));
  }

  public virtual void SetMouth(Vector2 uv){
  }

  public virtual void SetFace(int key) {
  }

  public virtual void SetMouth(int key) {
  }

  public virtual int LipSyncIndex(int key) {
    return 0;
  }

  public virtual int ExpToLip(int key) {
    return key;
  }

  public virtual State ToState(int index) {
    return new State ();
  }

  public void StartLipSync() {
    if (!lip.isSync) { 
      lip.isSync = true;
      lip.routine = StartCoroutine (LipSync ());
    }
  }

  public void StopLipSync() {
    lip.isSync = false;
    if (lip.routine != null) {
      SetMouth (lip.current);

      StopCoroutine (lip.routine);
      lip.routine = null;
    }
  }

  protected virtual Vector2 GetRandomLipFlap(){
    return Vector2.zero;
  }
  IEnumerator LipSync() {
    while (true) {
      
      //var key = UnityEngine.Random.Range (0, maxLip);
      SetMouth (GetRandomLipFlap());
      //SetMouth (LipSyncIndex(key));

      yield return new WaitForSeconds (interval);
    }
  }

}
