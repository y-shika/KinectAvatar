
using UnityEngine;

public class VLUtils  {

	public static SkinnedMeshRenderer GetHeadMesh ( GameObject go )
	{
		SkinnedMeshRenderer skinnedMeshRenderer = null;

		foreach ( var r in go.GetComponentsInChildren<SkinnedMeshRenderer>())
		{
			if ( r.sharedMesh.blendShapeCount > 0 && r.name.Contains("head") ){				
				skinnedMeshRenderer = r;
				break;
			}
		}

		return skinnedMeshRenderer;    	
	}
}
