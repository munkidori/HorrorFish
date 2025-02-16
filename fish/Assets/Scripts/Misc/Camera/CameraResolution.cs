using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
        AdjustFieldOfView();
    }

    void AdjustFieldOfView()
    {
        float targetAspect = 16f / 9f; // Default aspect ratio (e.g., 1920x1080)
        float currentAspect = (float)Screen.width / Screen.height;

        if (currentAspect > targetAspect)
        {
            // Wider than 16:9, reduce the FOV
            _camera.fieldOfView = Mathf.Lerp(60f, 50f, (currentAspect - targetAspect));
        }
        else
        {
            // Keep default FOV
            _camera.fieldOfView = 60f;
        }
    }
}
