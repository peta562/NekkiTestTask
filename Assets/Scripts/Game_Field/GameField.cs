using UnityEngine;

namespace Game_Field
{
    public class GameField : MonoBehaviour
    {
        [SerializeField] private Transform _leftBoundary;
        [SerializeField] private Transform _rightBoundary;
        [SerializeField] private Transform _topBoundary;
        [SerializeField] private Transform _bottomBoundary;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private SpriteRenderer _fieldSprite;
        
        private Camera _mainCamera;

        public Transform SpawnPoint => _spawnPoint;
        
        private float Left => _leftBoundary.position.x;
        private float Right => _rightBoundary.position.x;
        private float Top => _topBoundary.position.y;
        private float Bottom => _bottomBoundary.position.y;

        public void Initialize()
        {
            _mainCamera = Camera.main;
            SetupCamera();
            ResizeFieldSprite();
        }

        private void SetupCamera()
        {
            float width = Right - Left;
            float height = Top - Bottom;

            float screenRatio = (float)Screen.width / Screen.height;
            float targetRatio = width / height;

            if (screenRatio >= targetRatio)
            {
                _mainCamera.orthographicSize = height / 2;
            }
            else
            {
                float differenceInSize = targetRatio / screenRatio;
                _mainCamera.orthographicSize = height / 2 * differenceInSize;
            }

            _mainCamera.transform.position = new Vector3(
                (Left + Right) / 2,
                (Bottom + Top) / 2,
                _mainCamera.transform.position.z
            );
        }

        private void ResizeFieldSprite()
        {
            float width = Right - Left;
            float height = Top - Bottom;

            Vector3 fieldCenter = new Vector3((Left + Right) / 2, (Bottom + Top) / 2, 0);
            _fieldSprite.transform.position = fieldCenter;

            Sprite sprite = _fieldSprite.sprite;

            Vector2 spriteSize = sprite.bounds.size;
            Vector3 scale = _fieldSprite.transform.localScale;

            scale.x = width / spriteSize.x;
            scale.y = height / spriteSize.y;

            _fieldSprite.transform.localScale = scale;
        }
    }
} 