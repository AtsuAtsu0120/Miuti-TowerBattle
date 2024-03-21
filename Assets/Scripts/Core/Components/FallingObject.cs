using Cysharp.Threading.Tasks.Triggers;
using R3;
using UnityEngine;

namespace Core.Components
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FallingObject : MonoBehaviour
    {
        public Observable<Unit> OnFall => _onFall;
        public AudioClip Clip => _clip;
        public int Score { get; private set; }
        
        [SerializeField] private Rigidbody2D _rigid;
        [SerializeField] private AudioClip _clip;
        
        private readonly Subject<Unit> _onFall = new();
        public void Start()
        {
            _rigid.gravityScale = 0;
            NextObj();
        }
        
        private async void NextObj()
        {
            await this.GetAsyncCollisionEnter2DTrigger()
                .OnCollisionEnter2DAsync(destroyCancellationToken);
            
            _onFall.OnNext(Unit.Default);
        }

        public void StartFail()
        {
            _rigid.gravityScale = 1;
        }
        
        #if UNITY_EDITOR
        private void Reset()
        {
            _rigid = GetComponent<Rigidbody2D>();
        }
        #endif
    }
}