using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VL_OVRLipSyncContextMorphTarget : OVRLipSyncContextMorphTarget
{

	
	public bool BlendMouthEmotion;
	/// mouth close dampening (max is no effect
	[Range(1,30)]
	public float closeSpeed = 30f;

	/// mouth open dampening (max is no effect
	[Range(1,30)]
	public float openSpeed = 30f;

	/// General smoothing (max is no effect
	[Range(1,30)]
	public float lerpWeight = 30f;

    float[] aeiouValue = new float[5];

	CafeFace cafeFace;

    int[] visemeToAEIOU = new int[]
    {
        -1,
        4,
        4,
        1,
        1,
        1,
        4,
        2,
        1,
        4,
        0,
        1,
        2,
        3,
        4
    };

    void Awake ()
    {	
		    	    	
    	cafeFace = FindObjectOfType<CafeFace>();
    }


    void OnValidate () 
    {    	
    	if ( skinnedMeshRenderer == null )
    	{
			skinnedMeshRenderer = VLUtils.GetHeadMesh(gameObject);
    	}
    }

    protected override void SetVisemeToMorphTarget(OVRLipSync.Frame frame)
    {
    	var mouthOpenId = -1;
		var mouthCloseId = -1;
		if ( cafeFace != null ){
			mouthOpenId = cafeFace.CurrentPose.mouthOpenId;
			mouthCloseId = cafeFace.CurrentPose.mouthid;

		}

    	var input_aeiouValue = new float[aeiouValue.Length];
    	        
        for(int i = 0; i < 15; i++)
        {
            var aeiouIndex = visemeToAEIOU[i];
            if(aeiouIndex == -1)
            {
                continue;
            }
			input_aeiouValue[aeiouIndex] = Mathf.Max(input_aeiouValue[aeiouIndex], frame.Visemes[i]);
        }

        // open mouth for emotion
		var mouthEmotionDivisor = 1f;



		if ( mouthOpenId > 0 )
		{			
			mouthEmotionDivisor = 2f; // used later

			var total = 0f;
			for(int i = 0; i < 5; i++)
				total += aeiouValue[i];
			var mouthOpenEmotionValue = total / 2f * 100.0f;

			skinnedMeshRenderer.SetBlendShapeWeight(
				mouthOpenId,
				mouthOpenEmotionValue );			

			skinnedMeshRenderer.SetBlendShapeWeight(
				mouthCloseId,
				100f - mouthOpenEmotionValue * 3f );				
			
		}   


        for(int i = 0; i < 5; i++)
        {
			var prev = aeiouValue[i];
			var input = input_aeiouValue[i];
			var result = input;

			// cut if there is an open mouth for the emotion
			result = result / mouthEmotionDivisor;

			// apply general lerp
			result = Mathf.Lerp(prev,result,Time.deltaTime * lerpWeight);

			// apply open and close restriction
        	if ( result > aeiouValue[i]){
				result = Mathf.MoveTowards(aeiouValue[i], result, openSpeed * Time.deltaTime);
        	} else {
				result = Mathf.MoveTowards(aeiouValue[i], result, closeSpeed * Time.deltaTime);
        	}

			// set blendshape
            skinnedMeshRenderer.SetBlendShapeWeight(
                visemeToBlendTargets[10 + i],
                result * 100.0f);

            // store previous
			aeiouValue[i]  = result;
        }
    }
}
