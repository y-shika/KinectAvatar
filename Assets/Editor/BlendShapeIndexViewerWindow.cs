using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BlendShapeIndexViewerWindow : EditorWindow
{
    [MenuItem("Tools/BlendShapeIndexViewer")]
    private static void Create()
    {
        GetWindow<BlendShapeIndexViewerWindow>("BlendShapeIndexViewer");
    }
    SkinnedMeshRenderer renderer;
    Vector2 scrollPos = Vector2.zero;

    string filter;
    string[] blendShapeArray;

    string result;
    bool filterChanged;

    private void OnGUI()
    {
        using(var scope = new EditorGUI.ChangeCheckScope())
        {
            renderer = EditorGUILayout.ObjectField("Skinned Mesh Renderer", renderer, typeof(SkinnedMeshRenderer), true) as SkinnedMeshRenderer;
            if(scope.changed)
            {
                blendShapeArray = null;
            }
        }

        using(var scope = new EditorGUI.ChangeCheckScope())
        {
            filter = EditorGUILayout.TextField("Search", filter);
            if(scope.changed)
            {
                filterChanged = true;
            }
        }

        if(GUILayout.Button("選択中オブジェクトから取得"))
        {
            renderer = Selection.activeGameObject.GetComponent<SkinnedMeshRenderer>();
            blendShapeArray = null;
        }

        if(renderer == null)
        {
            return;
        }

        using(var scope = new EditorGUILayout.ScrollViewScope(scrollPos))
        {
            scrollPos = scope.scrollPosition;

            if(blendShapeArray == null)
            {
                var tmpList = new List<string>();
                var sharedMesh = renderer.sharedMesh;

                for(int i = 0; i < sharedMesh.blendShapeCount; i++)
                {
                    tmpList.Add(string.Format("{0} : {1}", i, sharedMesh.GetBlendShapeName(i)));
                }
                blendShapeArray = tmpList.ToArray();
                filterChanged = true;
            }

            if(filterChanged)
            {
                if(string.IsNullOrEmpty(filter))
                {
                    result = string.Join("\n", blendShapeArray);
                }
                else
                {
                    result = string.Join("\n", blendShapeArray.Where(s => s.Contains(filter)).ToArray());
                }
                filterChanged = false;
            }

            EditorGUILayout.SelectableLabel(result, GUI.skin.label, GUILayout.ExpandHeight(true));
        }
    }
}
