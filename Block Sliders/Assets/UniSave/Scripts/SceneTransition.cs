using System.Collections;
using UnityEngine;

namespace UniSave
{
    public sealed class SceneTransition : MonoBehaviour
    {
        private float _alpha;

        private float _fadeSpeed;
        private int _fadeDir = 1;

        private Color _color;

        private bool _isFadeScreen;
        private float _fadeInSpeed;
        private float _fadeOutSpeed;

        private float _seconds;
        private bool _count;

        void Awake()
        {
            gameObject.AddComponent<GUITexture>();
            gameObject.AddComponent<GUILayer>();
            transform.position = new Vector3(0.5f, 0.5f, 0);
            transform.localScale = new Vector3(1, 1, 1);

            DontDestroyOnLoad(this);
        }

        public void Init(string screen, bool isMultiScene)
        {
            if (isMultiScene)
            {
                if (Camera.main != camera)
                {
                    camera.enabled = false;
                }
            }

            guiTexture.texture = (Texture2D)Resources.Load(screen, typeof(Texture2D));

            StartCoroutine(Run());
        }

        public void Init(float fadeInSpeed, float fadeOutSpeed, bool isMultiScene)
        {
            if (isMultiScene)
            {
                if (Camera.main != camera)
                {
                    camera.enabled = false;
                }
            }

            _color = guiTexture.color;
            _color.a = _alpha;
            guiTexture.color = _color;

            guiTexture.texture = (Texture2D)Resources.Load("black", typeof(Texture2D));

            _fadeInSpeed = fadeInSpeed;
            _fadeOutSpeed = fadeOutSpeed;

            _fadeSpeed = _fadeInSpeed;
            _isFadeScreen = true;

            StartCoroutine(Run());
        }

        public void Init()
        {
            if (Camera.main != camera)
            {
                camera.enabled = false;
            }

            StartCoroutine(Run());
        }

        public IEnumerator Run()
        {
            while (SaveManager.IsLoading)
            {
                yield return null;
            }

            if (_isFadeScreen)
            {
                _fadeSpeed = _fadeOutSpeed;
                _fadeDir = -1;

                while (_alpha > 0)
                {
                    yield return null;
                }
            }

            Destroy(gameObject);
        }

        void Update()
        {
            if (_isFadeScreen)
            {
                _alpha += _fadeDir * Time.deltaTime / (_fadeSpeed * 2);
                _alpha = Mathf.Clamp01(_alpha);

                _color.a = _alpha;
                guiTexture.color = _color;
            }
        }
    }
}