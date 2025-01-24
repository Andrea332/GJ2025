using UnityEngine;

namespace Game
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public class CameraAspectRatio : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private bool _previewInEditMode = true;
#endif
        [SerializeField] private Vector2 _aspectRatio = new (16, 9);
        private float ScreenRatio => Screen.width / (float)Screen.height;
        private float ViewportInset => 1f - (ScreenRatio / AspectRatio);
        private Rect _viewport;
        
        private Camera _camera;

        private float AspectRatio => _aspectRatio.x/_aspectRatio.y;

#if UNITY_EDITOR
        private void OnGUI()
        {
            if (_previewInEditMode)
            {
                SetCameraAspectRatio();
            }
            else
            {
                ClearBox();
            }
            
            _camera.rect = _viewport;
        }
#endif
        
        private void Awake()
        {
            _camera = GetComponent<Camera>();
          
            SetCameraAspectRatio();
        }
        
        private void SetCameraAspectRatio()
        {
            if (AspectRatio > ScreenRatio)
            {
                SetLetterbox( ViewportInset);
            }
            else if (AspectRatio < ScreenRatio)
            {
                SetPillarbox(ViewportInset);
            }
            else
            {
                ClearBox();
            }
        }
        
        public void SetLetterbox(float viewportIncet)
        {
            _viewport.Set(0, viewportIncet / 2, 1, 1 - viewportIncet);
        }

        public void SetPillarbox(float viewportIncet)
        {
            _viewport.Set(viewportIncet / 2, 0, 1 - viewportIncet, 1);
        }

        public void ClearBox()
        {
            _viewport.Set(0, 0, 1, 1);
        }
    }
}
