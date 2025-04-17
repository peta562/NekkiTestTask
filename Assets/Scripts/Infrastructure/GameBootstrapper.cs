using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class GameBootstrapper : IInitializable
    {
        private readonly MainGameController _mainGameController;

        [Inject]
        public GameBootstrapper(MainGameController mainGameController)
        {
            _mainGameController = mainGameController;
        }   
        
        public void Initialize()
        {
            InitGameAsync().Forget();
        }

        private async UniTaskVoid InitGameAsync()
        {
          
            await UniTask.DelayFrame(1);
            _mainGameController.StartGameAsync().Forget();
        }
    }
} 