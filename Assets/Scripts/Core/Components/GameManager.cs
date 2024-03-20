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
        [SerializeField] private InGameUI _ui;
        [SerializeField] private FallingObjectAsset _fallingObjectAsset;
        [SerializeField] private Input _input;
        [SerializeField] private AudioSource _source;

        private readonly ReactiveProperty<bool> _isGameOver = new();
        private readonly Random _random = new();

        public void Start()
        {
            GenerateFallingObject();
            GameOver();
        }

        private async void GameOver()
        {
            _isGameOver.Subscribe(_ => _ui.SetUIActive(_isGameOver.Value));
            await _failedArea.GetAsyncTriggerEnter2DTrigger()
                .OnTriggerEnter2DAsync();

            _isGameOver.Value = true;
        }

        private void GenerateFallingObject()
        {
            if(_isGameOver.Value) return;

            var randomValue = _random.Next(0, _fallingObjectAsset.FallingObjectList.Count);
            var prefab = _fallingObjectAsset.FallingObjectList[randomValue];
            
            var obj = Instantiate(prefab);
            _input.FallingObject = obj;
            obj.OnFall.Subscribe(_ =>
            {
                _source.PlayOneShot(obj.Clip);
                GenerateFallingObject();
            }).AddTo(this);
        }
    }
}
