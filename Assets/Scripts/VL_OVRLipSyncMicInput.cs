using System;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Threading;
using System.Collections;

public class VL_OVRLipSyncMicInput : OVRLipSyncMicInput {
    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        audioSource.loop = true;     // Set the AudioClip to loop
        audioSource.mute = false;

        if (Microphone.devices.Length != 0)
        {
            selectedDevice = Microphone.devices[selectedDeviceIndex];
            micSelected = true;
            GetMicCaps();
        }

        StartCoroutine(Jogmic());
    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    void Update()
    {
        if (!Application.isPlaying)
        {
            StopMicrophone();
            return;
        }

        audioSource.volume = (micInputVolume / 100);

        //Hold To Speak
        if (micControl == micActivation.HoldToSpeak)
        {
            if (Input.GetKey(micActivationKey))
            {
                if (!Microphone.IsRecording(selectedDevice))
                {
                    StartMicrophone();
                }
            }
            else
            {
                if (Microphone.IsRecording(selectedDevice))
                {
                    StopMicrophone();
                }
            }
        }

        //Push To Talk
        if (micControl == micActivation.PushToSpeak)
        {
            if (Input.GetKeyDown(micActivationKey))
            {
                if (Microphone.IsRecording(selectedDevice))
                {
                    StopMicrophone();
                }
                else if (!Microphone.IsRecording(selectedDevice))
                {
                    StartMicrophone();
                }
            }
        }

        //Constant Speak
        if (micControl == micActivation.ConstantSpeak)
        {
            if (!Microphone.IsRecording(selectedDevice))
            {
                StartMicrophone();
            }
        }


        //Mic Selected = False
        if (enableMicSelectionGUI)
        {
            if (Input.GetKeyDown(micSelectionGUIKey))
            {
                micSelected = false;
            }
        }
    }

    IEnumerator Jogmic()
    {
        while(true)
        {
            yield return new WaitForSeconds(10f);
            StopMicrophone();
            StartMicrophone();
        }
    }
}
