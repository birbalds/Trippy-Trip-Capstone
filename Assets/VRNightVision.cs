using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class VRNightVision : MonoBehaviour {
    
    private Material material;
    private Camera cam;

    [Tooltip("Grain size in pixel. Should be greater than 1 with supersampling.")]
    [Range(1, 100)]
    public float scaling = 6;
    
    [Tooltip("Areas that are brighter than this value won't receive any grain.")]
    public float cutoff = 0.0025f;
    [Tooltip("Proportion of dark grains.")]
    [Range(0, 1)]
    public float distribution = 0.9f;
    [Tooltip("Brightness of light grains.")]
    public float lightBrightness = 0.0025f;
    [Tooltip("Brightness of dark grains. Raising it can improve user experience.")]
    public float darkBrightness = 0.000f;


    // Creates a private material used to the effect
    void Awake() {
        material = new Material(Shader.Find("VRNightVision"));
    }

    void Start() {
        cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.Depth;
    }

    private Matrix4x4 world2head;

    void LateUpdate() {
        world2head = GL.GetGPUProjectionMatrix(cam.projectionMatrix, false) * cam.worldToCameraMatrix;
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Matrix4x4 world2Screen = GL.GetGPUProjectionMatrix(cam.projectionMatrix, false) * cam.worldToCameraMatrix;
        Matrix4x4 screen2World = world2Screen.inverse;
        material.SetMatrix("_EyeMat", screen2World);
        material.SetMatrix("_HeadMat", world2head);
        material.SetVector("_Resolution", new Vector2(source.width / scaling, source.height / scaling));
        material.SetFloat("_Cutoff", cutoff);
        material.SetFloat("_Distribution", distribution);
        material.SetFloat("_Light", lightBrightness);
        material.SetFloat("_Dark", darkBrightness);
        Graphics.Blit(source, destination,material);
    }
}