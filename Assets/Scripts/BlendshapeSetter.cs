using System.Collections.Generic;
using UnityEngine;

public class BlendshapeSetter : MonoBehaviour
{
    public List<SkinnedMeshRenderer> Meshes;
    public int currentid;
    public float currentStrength;
    public int currentidsub;
    public float currentSubStrength;

    public float speed = .3f;
    float target = 0f;
    float current = 0f;

    void Awake()
    {
        if(Meshes == null || Meshes.Count == 0)
        {
            Meshes = GetBlendShapeMeshes();
        }

        string d = "BlendshapeSetter Init--- " + gameObject.name + "(" + Meshes.Count + "\n";
        foreach(var tgt in Meshes)
        {
            d += "[" + tgt.name + "]\n";
            if(tgt.sharedMesh != null)
            {
                for(int i = 0; i < tgt.sharedMesh.blendShapeCount; i++)
                {
                    d += i + ". " + tgt.sharedMesh.GetBlendShapeName(i) + "\n";
                }
            }
            else
            {
                d += "no mesh!\n";
            }
        }
        Debug.Log(d);
    }

    public void Set(int id, float strength, int idsub, float substrength)
    {
        currentid = id;
        currentStrength = strength;
        currentidsub = idsub;
        currentSubStrength = substrength;
    }

    public void Set(int id)
    {
        currentid = (ushort) id;
    }

    void Update()
    {
        foreach(var tgt in Meshes)
        {
            for(int i = 0; i < tgt.sharedMesh.blendShapeCount; i++)
            {
                current = tgt.GetBlendShapeWeight(i);
                if(i == currentid)
                {
                    target = currentStrength;
                }
                else if(i == currentidsub)
                {
                    target = currentSubStrength;
                }
                else
                {
                    target = 0f;
                }

                if(target == 0 && current < 1)
                {
                    tgt.SetBlendShapeWeight(i, 0);
                }
                else
                {
                    tgt.SetBlendShapeWeight(i, Mathf.Lerp(current, target, speed));
                }
            }
        }
    }

    public static List<SkinnedMeshRenderer> GetBlendShapeMeshes(GameObject go)
    {
        var d = "Auto set meshes \n";

        var ret = new List<SkinnedMeshRenderer>();
        var meshes = go.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach(var m in meshes)
        {
            if(m.sharedMesh != null && m.sharedMesh.blendShapeCount > 0)
            {
                d += "adding " + m.name + "\n";
                ret.Add(m);
            }
        }
        d += "Auto set meshes " + ret.Count + "\n";
        Debug.Log(d);
        return ret;
    }

    public List<SkinnedMeshRenderer> GetBlendShapeMeshes()
    {
        return GetBlendShapeMeshes(gameObject);
    }
}
