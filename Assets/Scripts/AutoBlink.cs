using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AutoBlink : MonoBehaviour
{
    [Serializable]
    public class Target
    {
        public SkinnedMeshRenderer morph;
        public int closeEyeIndex = -1;
    }

    [SerializeField] Target target;

    [SerializeField]
    float interval = 1.5f;

    [SerializeField]
    float prob = 0.5f;

    [SerializeField]
    float speed = 5f;

    public List<int> closeEyeIds;
    public bool CanBlink { get; set; }

    WaitForSeconds waitForSeconds;

    [SerializeField] private float resetSpeed = 0.9f;

    private Coroutine blink;

    void Start()
    {
        waitForSeconds = new WaitForSeconds(interval);

        if (target.morph == null)
        {
            Debug.LogError("Target中のMorphにSkinned Mesh Rendererを入れてください");
        }
        else
        {
            if (target.closeEyeIndex == -1)
            {
                Debug.LogError("Target中のClose Eye Indexに閉じ目のBlendshapeインデックスを入れてください");
            }
            else
            {
                CanBlink = true;
                blink =  StartCoroutine(Blink());
            }
        }
    }

    int BlinkId = 0;
    int OpenId = -1;

    public void SetBlinkId(int id, int iOpenId = -1)
    {
        SetCloseID(id);
        SetOpenID(iOpenId);
    }

    private void SetCloseID(int id)
    {
        if(id < 0)
        {
            BlinkId = target.closeEyeIndex;
            return;
        }
        if(BlinkId == id) { return; }

        target.morph.SetBlendShapeWeight(BlinkId, 0);
        BlinkId = id;
    }

    private void SetOpenID(int id)
    {
        if(OpenId == id)
        {
            return;
        }

        if(id >= 0)
        {
            OpenId = id;
        }
    }

    void SetEye(float ratio, float max_open_value)
    {
        var maintgt = target.morph;
        ratio = Mathf.Clamp01(ratio);

        // check closeeyeids
        for (int i = 0; i < maintgt.sharedMesh.blendShapeCount; i++)
        {
            if (closeEyeIds.Contains(i) && maintgt.GetBlendShapeWeight(i) > 5)
            {
                ratio = 0;
        
                break;
            }
        }

        target.morph.SetBlendShapeWeight(BlinkId, ratio * 100f);

        if(OpenId >= 0 && OpenId != BlinkId && OpenId < target.morph.sharedMesh.blendShapeCount)
        {
            target.morph.SetBlendShapeWeight(OpenId, max_open_value *(1 - ratio));
        }
    }

    public void StartAutoBlink()
    {
        CanBlink = true;

        // 閉じ目の状態でトラッキングを外れた場合に、コルーチンを始める前に、目を開けた状態にする。
        float blinkBlendshape = target.morph.GetBlendShapeWeight(BlinkId);
        while(blinkBlendshape > 0)
        {
            blinkBlendshape -= resetSpeed * Time.deltaTime;
            target.morph.SetBlendShapeWeight(BlinkId, blinkBlendshape);
        }
        target.morph.SetBlendShapeWeight(0, 0f);

        blink = StartCoroutine(Blink());
    }

    public void StopAutoBlink()
    {
        StopCoroutine(blink);
    }

    IEnumerator Blink()
    {
        while (true)
        {
            yield return waitForSeconds;
            if (prob <= UnityEngine.Random.value)
            {
                continue;
            }

            if (!CanBlink)
            {
                continue;
            }

            var ratio = target.morph.GetBlendShapeWeight(BlinkId) / 100f;
            var start_ratio = ratio;
            var initial_open_value = target.morph.GetBlendShapeWeight(OpenId);

            // 目を閉じる（瞬き）
            yield return new WaitUntil(() =>
            {
                ratio += speed * Time.deltaTime;
                SetEye(ratio, initial_open_value);
                return ratio > 1f;
            });

            ratio = 1f;
            SetEye(ratio, initial_open_value);
            yield return null;

            // 目を開ける（瞬き）
            yield return new WaitUntil(() =>
            {
                ratio -= speed * Time.deltaTime;
                SetEye(ratio, initial_open_value);
                return ratio < start_ratio;
            });

            // 最初の目の状態に戻す
            SetEye(start_ratio, initial_open_value);
            yield return null;
        }
    }

}
