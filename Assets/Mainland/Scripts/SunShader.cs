using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunShader : MonoBehaviour
{
    void Start()
    {
        Shader.SetGlobalVector("_SunDirection", transform.forward);
    }
}
