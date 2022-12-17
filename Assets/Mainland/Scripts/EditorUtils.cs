using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MeshGenerator))]
public class EditorUtils : Editor
{
    public override void OnInspectorGUI()
    {
        MeshGenerator meshGen = (MeshGenerator) target;
        if(DrawDefaultInspector())
        {
            meshGen.GenerateTerrain();
        }
    }
}
