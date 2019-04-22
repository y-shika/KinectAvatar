using System;
using System.Collections.Generic;
using UnityEngine;

public class CafeFace : CharaFace
{
    public Vector2 currentTex;
    [SerializeField]
    SkinnedMeshRenderer face;

    public BlendshapeSetter faceSetter;

    [SerializeField] private AutoBlink _autoBlink = null;
    //private VLARKitEyeTracking _vlARKitEyeTracking = null;
    //[SerializeField] SkinBlendManager skinBlend;

    /// <summary>
    ///  PoseのdetailIndexで設定。表示したいメシュの名前
    /// </summary>
    public List<string> detailMeshNames;

    /// <summary>
    /// detail mesh表示のスピード
    /// </summary>
    [Range(1,50)]
    public float detailMeshFadeSpeed = 10;

    List<MeshRenderer> detailMeshRenderers; 
    int detailMeshTarget = -1;

    int currentPatternIndex = 0;
    bool useSkinBlend = false;

    public int faceChangeIndex = 0;
    private int currentFaceIndex = 0;

    [Serializable]
    public class Pose
    {
        public int eyeid;
        public int blinkId = -1;
        public float eyestrength = 100f;
        public int mouthid;
        public int mouthOpenId = -1;
        public float mouthstrength = 100f;
        public bool isOpen;

        public bool withBlink;

        public int skinIndex;
        /// <summary>
        /// detailMeshNamesのやつ
        /// </summary>
        public int detailIndex = -1;
    }

    [Serializable]
    public class PoseDef
    {
        public Pose normal;
        public Pose smile;
        public Pose bashful;
        public Pose sad;
        public Pose angry;
        public Pose doya;
        public Pose niko;
        public Pose hotblood;
        public Pose scornful;
        public Pose pale;
    }


    public override int maxExp { get { return (int)Expression.Max; } }
    public override int maxLip { get { return 4; } }

    [SerializeField]
    public PoseDef poses;
    public Pose CurrentPose { get; private set; }
    Pose[] pattern;
    public List<int> lipPattern;

    MaterialPropertyBlock prop;

    public enum Expression
    {
        Normal,
        Proud,
        Proud2,
        Happy,
        Happy2,
        Sad,
        Angry,
        Surprise,
        Scornful,
        Max
    }

    protected override void Awake()
    {
        InitDetailMeshes();
        faceSetter = GetComponentInChildren<BlendshapeSetter>();

        if (_autoBlink == null)
        {
            _autoBlink = GetComponent<AutoBlink>();
        }

        // iPhoneX(ARKit)を使う時にだけ作用するようにする
//        var vlARKitEyeTracking = GetComponent<VLARKitEyeTracking>();
//        if(vlARKitEyeTracking != null && vlARKitEyeTracking.enabled)
//        {
//            _vlARKitEyeTracking = vlARKitEyeTracking;
//        }
//
//        useSkinBlend = skinBlend != null;

        base.Awake();

    }

    void Start()
    {
        prop = new MaterialPropertyBlock();
        pattern = new Pose[] {
        poses.normal,
        poses.smile,
        poses.bashful,
        poses.sad,
        poses.angry,
        poses.doya,
        poses.niko,
        poses.hotblood,
        poses.scornful,
        poses.pale,
      };
		CurrentPose = pattern[0];
        faceNames = new string[maxExp];
        for (var i = 0; i < maxExp; i++)
        {
            faceNames[i] = ((Expression)i).ToString();
        }

        SetExpression(0);

        // はじめにノーマルとして、表情をセットする（ARKitによるFaceTrackingにて瞬きのBlendshapeIDを取得する際、モデルによって瞬きIDが違ったため）
        SetPose(pattern[0]);
    }

    public override void SetMouth(Vector2 uv)
    {
        currentTex.x = uv.x;
        currentTex.y = uv.y;
        prop.SetFloat("_u", uv.x);
        prop.SetFloat("_v", uv.y);
        face.SetPropertyBlock(prop);
    }

    public override void SetFace(int index)
    {
        var anim = GetComponent<Animator>();
        //anim.SetInteger("Exp", (int)index);
        SetEyes(pattern[index].eyeid);
    }

    public override void SetMouth(int index)
    {
    }

    public void SetEyes(int index)
    {
        if (faceSetter != null)
        {
            faceSetter.Set(index);
        }
    }

    Vector2 IndexToVector(int idx)
    {
        int j = idx / 4;
        int k = idx % 4;

        var vec = new Vector2(k * 0.25f, j * 0.25f);

        return vec;
    }
    protected override Vector2 GetRandomLipFlap()
    {

        var i = lipPattern[UnityEngine.Random.Range(0, lipPattern.Count)];

        return IndexToVector(i);
    }

    public override int LipSyncIndex(int key)
    {
        return lipPattern[key];
    }

    public override State ToState(int index)
    {
        return new State()
        {
            canBlink = pattern[index].withBlink,
            mouthOpen = pattern[index].isOpen
        };
    }

    void Update()
    {
    	UpdateDetailMesh();

        if (PS4Controller.isConnect())
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if(PS4Controller.FacialButtonDown(i))
                {
					SetPose(pattern[i]);
                }
                // 押している間、表情変わり、離すと表情戻る機能だと少々使いにくかったため、ひとまずコメントアウトしておく
                // else if(PS4Controller.FacialButtonUp(i))
                // {
				// 	SetPose(pattern[0]);
                // }
            }
        }

		var pressedKeyboardNumber = InputHelp.GetNumberDown();
        if ( Input.GetKey(KeyCode.F) && pattern.HasIndex(pressedKeyboardNumber) ){
			if (useSkinBlend)
            {
//				if (pattern[pressedKeyboardNumber].skinIndex == 0)
//                {
//                    skinBlend.SetTexture(0);
//                }
//				else if (currentPatternIndex == pressedKeyboardNumber)
//                {
//					skinBlend.SetTexture(pattern[pressedKeyboardNumber].skinIndex);
//                }

				currentPatternIndex = pressedKeyboardNumber;
            }
			SetPose(pattern[pressedKeyboardNumber]);
        }

        if (faceChangeIndex != currentFaceIndex)
        {
            int i = faceChangeIndex;
            SetPose(pattern[i]);
			currentFaceIndex = faceChangeIndex;
        }
    }

    public void SetPose ( Pose p )
    {
		_autoBlink.CanBlink = p.withBlink;
        faceSetter.Set(p.eyeid, p.eyestrength, p.mouthid, p.mouthstrength);
		CurrentPose = p;
        _autoBlink.SetBlinkId(p.blinkId, p.eyeid);
        SetDetailMesh(p.detailIndex);

        // 使わないことも想定されるためコンポーネントがアクティブになっていることを確認する
//        if (_vlARKitEyeTracking != null)
//        {
//            _vlARKitEyeTracking.SetBlink(p);
//        }
    }
       
    void InitDetailMeshes()
    {
        detailMeshRenderers = new List<MeshRenderer>();
        foreach ( var n in detailMeshNames)
        {
            detailMeshRenderers.Add(null);
        }

        foreach ( var r in GetComponentsInChildren<MeshRenderer>())
        {
            if ( detailMeshNames.Contains( r.name))
            {
                detailMeshRenderers[detailMeshNames.IndexOf(r.name)] = r;
            }
        }
    }

    void UpdateDetailMesh ()
    {
		for ( int i  = 0; i < detailMeshRenderers.Count; i++)
        {
        	float targ = 0;
			if ( i == detailMeshTarget ){
				targ = 1;
            } 

            var r = detailMeshRenderers[i];
            if ( r != null ){
				var c = r.material.color;
				c.a = Mathf.Lerp(c.a,targ,detailMeshFadeSpeed * Time.deltaTime);
				r.material.color = c;
				r.enabled = r.material.color.a > 0.001f;
            }
        }        
    }

    void SetDetailMesh(int setid)
    {
    	detailMeshTarget = setid;        
    }
}
