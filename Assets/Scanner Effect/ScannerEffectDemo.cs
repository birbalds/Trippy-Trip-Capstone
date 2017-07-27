using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ScannerEffectDemo : MonoBehaviour
{
	public Transform ScannerOrigin;
	public Material EffectMaterial;
	public float ScanDistance;
	public LayerMask excludeLayers = 0;
	public CurrentPlayer player;

	private GameObject tmpCam = null;
	private Camera _camera;

	// Demo Code
	bool _scanning;
	Scannable[] _scannables;

	void Start()
	{
		_scannables = FindObjectsOfType<Scannable>();
    }

	void Update()
	{
		if (_scanning)
		{
			ScanDistance += Time.deltaTime * 3;
			foreach (Scannable s in _scannables)
			{
				if (Vector3.Distance(ScannerOrigin.position, s.transform.position) <= ScanDistance)
					s.Ping();
			}
		}

		if (player.tookPill)
		{
			_scanning = true;

			ScanDistance = 0;
		}


	}

	void LateUpdate(){
		player.tookPill = false;
		}
	// End Demo Code

	void OnEnable()
	{
		_camera = GetComponent<Camera>();
		_camera.depthTextureMode = DepthTextureMode.Depth;
	}

	[ImageEffectOpaque]
	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		EffectMaterial.SetVector("_WorldSpaceScannerPos", ScannerOrigin.position);
		EffectMaterial.SetFloat("_ScanDistance", ScanDistance);
		RaycastCornerBlit(src, dst, EffectMaterial);
		Camera cam = null;
		if (excludeLayers.value != 0) cam = GetTmpCam();

		if (cam && excludeLayers.value != 0)
		{
			cam.targetTexture = dst;
			cam.cullingMask = excludeLayers;
			cam.Render();
		}
	}

	void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
	{
		// Compute Frustum Corners
		float camFar = _camera.farClipPlane;
		float camFov = _camera.fieldOfView;
		float camAspect = _camera.aspect;

		float fovWHalf = camFov * 0.5f;

		Vector3 toRight = _camera.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
		Vector3 toTop = _camera.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

		Vector3 topLeft = (_camera.transform.forward - toRight + toTop);
		float camScale = topLeft.magnitude * camFar;

		topLeft.Normalize();
		topLeft *= camScale;

		Vector3 topRight = (_camera.transform.forward + toRight + toTop);
		topRight.Normalize();
		topRight *= camScale;

		Vector3 bottomRight = (_camera.transform.forward + toRight - toTop);
		bottomRight.Normalize();
		bottomRight *= camScale;

		Vector3 bottomLeft = (_camera.transform.forward - toRight - toTop);
		bottomLeft.Normalize();
		bottomLeft *= camScale;

		// Custom Blit, encoding Frustum Corners as additional Texture Coordinates
		RenderTexture.active = dest;

		mat.SetTexture("_MainTex", source);

		GL.PushMatrix();
		GL.LoadOrtho();

		mat.SetPass(0);

		GL.Begin(GL.QUADS);

		GL.MultiTexCoord2(0, 0.0f, 0.0f);
		GL.MultiTexCoord(1, bottomLeft);
		GL.Vertex3(0.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 0.0f);
		GL.MultiTexCoord(1, bottomRight);
		GL.Vertex3(1.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 1.0f);
		GL.MultiTexCoord(1, topRight);
		GL.Vertex3(1.0f, 1.0f, 0.0f);

		GL.MultiTexCoord2(0, 0.0f, 1.0f);
		GL.MultiTexCoord(1, topLeft);
		GL.Vertex3(0.0f, 1.0f, 0.0f);

		GL.End();
		GL.PopMatrix();
	}

	Camera GetTmpCam()
	{
		if (tmpCam == null)
		{
			if (_camera == null) _camera = GetComponent<Camera>();

			string name = "_" + _camera.name + "_GrayScaleTmpCam";
			GameObject go = GameObject.Find(name);

			if (null == go) // couldn't find, recreate
			{
				tmpCam = new GameObject(name, typeof(Camera));
			} else
			{
				tmpCam = go;
			}
		}

		tmpCam.hideFlags = HideFlags.DontSave;
		tmpCam.transform.position = _camera.transform.position;
		tmpCam.transform.rotation = _camera.transform.rotation;
		tmpCam.transform.localScale = _camera.transform.localScale;
		tmpCam.GetComponent<Camera>().CopyFrom(_camera);

		tmpCam.GetComponent<Camera>().enabled = false;
		tmpCam.GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
		tmpCam.GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;

		return tmpCam.GetComponent<Camera>();
	}
}
