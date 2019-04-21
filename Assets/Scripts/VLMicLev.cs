using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VLMicLev : MonoBehaviour {

	AudioClip microphoneInput;
	public float sensitivity;
	public bool flapped;
	public float level;
	public float max;
	public Image img;
	public float smooth;

	void Awake () {
		img = GetComponentInChildren<Image>();

		//init microphone input
		if (Microphone.devices.Length>0){
			microphoneInput = Microphone.Start(Microphone.devices[0],true,1,44100);
		}
	}

	// Update is called once per frame
	void Update () {
		
		//get mic volume
		int dec = 128;
		float[] waveData = new float[dec];
		int micPosition = Microphone.GetPosition(null)-(dec+1); // null means the first microphone



		microphoneInput.GetData(waveData, micPosition);

		level = GetLevel( ref waveData);

		if ( img != null && max > 0 ) {
			img.fillAmount = Mathf.Lerp( img.fillAmount, level / max, 1 - smooth);
		}

		if (level>sensitivity && !flapped){			
			flapped = true;
		}
		if (level<sensitivity && flapped){
			flapped = false;
		}
	}

	public static float GetLevel ( ref float[] waveData )
	{

		if ( waveData == null ) { return 0; }	
		var len = waveData.Length;
		float levelMax = 0;
		for (int i = 0; i < len; i++) {
			float wavePeak = waveData[i] * waveData[i];
			if (levelMax < wavePeak) {
				levelMax = wavePeak;
			}
		}
		return Mathf.Sqrt(Mathf.Sqrt(levelMax));		
	}

}
