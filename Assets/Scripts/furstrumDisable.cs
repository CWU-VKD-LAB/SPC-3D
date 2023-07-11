using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class furstrumDisable : MonoBehaviour
{
	/// <summary>
	/// Frustum culling will be disabled for this game object based on this camera.
	/// Alternatively, you could remove this property and use Camera.main if you always
	/// want to disable frustum culling on the main camera.
	/// </summary>
	private Camera camera;


	void Awake()
    {
		camera = Camera.main;
	}

	void Update()
	{
		// boundsTarget is the center of the camera's frustum, in world coordinates:
		Vector3 camPosition = camera.transform.position;
		Vector3 normCamForward = Vector3.Normalize(camera.transform.forward);
		float boundsDistance = (camera.farClipPlane - camera.nearClipPlane) / 2 + camera.nearClipPlane;
		Vector3 boundsTarget = camPosition + (normCamForward * boundsDistance);

		// The game object's transform will be applied to the mesh's bounds for frustum culling checking.
		// We need to "undo" this transform by making the boundsTarget relative to the game object's transform:
		Vector3 realtiveBoundsTarget = this.transform.InverseTransformPoint(boundsTarget);

		// Set the bounds of the mesh to be a 1x1x1 cube (actually doesn't matter what the size is)
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.bounds = new Bounds(realtiveBoundsTarget, Vector3.one);
	}
}

/*using UnityEngine;

/// <summary>
/// Disables frustum culling for a game object based on the provided camera.
/// This is done by moving mesh.bounds into the center of the camera's
/// frustum every update.
/// </summary>
public class DisableFrustumCulling : MonoBehaviour
{
	
}*/
