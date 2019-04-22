using UnityEngine;

[ExecuteInEditMode]
[DefaultExecutionOrder(5)]
public class BlendshapeCopy : MonoBehaviour
{
	public SkinnedMeshRenderer target;
	SkinnedMeshRenderer mymesh;

	void Awake () 
	{
		mymesh = GetComponent<SkinnedMeshRenderer>();		
	}

	void LateUpdate () 
	{
		if ( target != null && mymesh != null && mymesh.sharedMesh != null ){
			for ( int i = 0; i < target.sharedMesh.blendShapeCount; i++ ){
				if ( mymesh.sharedMesh.blendShapeCount > i ){
					mymesh.SetBlendShapeWeight(i,target.GetBlendShapeWeight(i));
				}
			}
		}
	}
}
