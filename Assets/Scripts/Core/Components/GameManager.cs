using Core.Components.Base;
using R3;
using UnityEngine;
using Cysharp.Threading.Tasks.Triggers;
using Foundation;
using Random = System.Random;

namespace Core.Components
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private FailedArea _failedArea;
        [SerializeField] private GameOverUI _overUI;
        [SerializeField] private FallingObjectAsset _fallingObjectAsset;
        [SerializeField] private Input _input;
        [SerializeField] private InGameUI _inGameUI;
        [SerializeField] private AudioSource _source;
        [SerializeField] private Transform _spawnPosition;

        [Header("Audio")] 
        [SerializeField] private AudioClip _konwaku;
        [SerializeField] private AudioClip _uwa;
        [SerializeField] private AudioClip _dodai;
        [SerializeField] private AudioClip _tigau;

        private readonly ReactiveProperty<bool> _isGameOver = new();
        private readonly ReactiveProperty<float> _score = new();
        private readonly Random _random = new();
        private RaycastHit2D[] _results = new RaycastHit2D[1];
        private float _spawnPositionY;
        private int _generatedObjectCount = 0;

        private const int FallLayerNumber = 3;
        private const float ScoreDefaultPositionY = 2.78f;
        

        public void Start()
        {
            GenerateFallingObject();
            GameOver();

            _score.Subscribe(x =>
            {
                _inGameUI.SetScore((int)(x * 100f));
                if (x > 300)
                {
                    _source.PlayOneShot(_dodai);
                }
            });
        }

        private void FixedUpdate()
        {
            SetScore();
        }

        private async void GameOver()
        {
            _isGameOver.Subscribe(_ =>
            {
                _inGameUI.SetUIActive(!_isGameOver.Value);
                _overUI.SetUIActive(_isGameOver.Value);
                var score = (int)(_score.Value * 100f);
                _overUI.SetResultScore(score);
                SetHighScore(score);
                if (_isGameOver.Value)
                {
                    if (score <= 100)
                    {
                        _source.PlayOneShot(_konwaku);
                    }
                    else if(score <= 200)
                    {
                        _source.PlayOneShot(_tigau);
                    }
                    else if (score <= 300)
                    {
                        _source.PlayOneShot(_uwa);
                    }   
                }
            });
            
            await _failedArea.GetAsyncTriggerEnter2DTrigger()
                .OnTriggerEnter2DAsync();

            _isGameOver.Value = true;
        }

        private void SetScore()
        {
            var hitCount = Physics2D.BoxCastNonAlloc(new Vector2(0f, 100), new Vector2(3f, 0.1f), 0f, Vector2.down, _results, Mathf.Infinity, 1 << 3);
            
            if (hitCount > 0)
            {
                _score.Value = _results[0].point.y + ScoreDefaultPositionY;
                _spawnPosition.position = new ( 0,_results[0].point.y + 5.0f);
            }
        }

        private void SetHighScore(int finalScore)
        {
            if (ScoreManager.Score1 < finalScore)
            {
                ScoreManager.Score1 = finalScore;
            }
            else if (ScoreManager.Score2 < finalScore)
            {
                ScoreManager.Score2 = finalScore;
            }
            else if (ScoreManager.Score3 < finalScore)
            {
                ScoreManager.Score3 = finalScore;
            }
            else if (ScoreManager.Score4 < finalScore)
            {
                ScoreManager.Score4 = finalScore;
            }
            else if (ScoreManager.Score5 < finalScore)
            {
                ScoreManager.Score5 = finalScore;
            }
        }
        private void GenerateFallingObject()
        {
            if(_isGameOver.Value) return;
            
            var randomValue = _random.Next(0, _fallingObjectAsset.FallingObjectList.Count);
            var prefab = _fallingObjectAsset.FallingObjectList[randomValue];
            
            prefab.transform.position = new(0, _spawnPosition.position.y);
            
            var obj = Instantiate(prefab);
            _input.FallingObject = obj;
            
            // 音鳴らす
            _source.PlayOneShot(obj.Clip);
            
            obj.OnFall.Subscribe(_ =>
            {
                GenerateFallingObject();
                obj.gameObject.layer = FallLayerNumber;
            }).AddTo(this);
        }
    }
}
