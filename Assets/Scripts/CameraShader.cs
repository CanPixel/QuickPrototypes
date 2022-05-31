using UnityEngine;
 
[ExecuteInEditMode]
public class CameraShader : MonoBehaviour
{
    public Material material;
    
	void Start() {
        material = new Material(Shader.Find("Custom/Scanlines"));
    }
 
    public void OnRenderImage(RenderTexture source, RenderTexture destination) {
        material.SetTexture("_MainTex", source);
        Graphics.Blit(source, destination, material);
    }
}