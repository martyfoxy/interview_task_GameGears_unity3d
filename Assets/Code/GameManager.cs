using System.Collections.Generic;
using Code.Players;
using Code.Settings;
using Code.UI;
using Zenject;

namespace Code
{
    public sealed class GameManager : IInitializable
    {
        private readonly PlayerFactory _playerFactory;
        private readonly PlayerRepository _playerRepository;
        private readonly PlayerViewRepository _playerViewRepository;
        private readonly HpBarPresenter _hpBarPresenter;
        private readonly UiController _uiController;
        private readonly SettingsRepository _settingsRepository;
        private readonly List<PlayerViewData> _playersViewData;
        
        public GameManager(
            PlayerFactory playerFactory,
            PlayerRepository playerRepository,
            PlayerViewRepository playerViewRepository,
            HpBarPresenter hpBarPresenter,
            UiController uiController,
            SettingsRepository settingsRepository,
            List<PlayerViewData> playersViewData)
        {
            _playerFactory = playerFactory;
            _playerRepository = playerRepository;
            _playerViewRepository = playerViewRepository;
            _hpBarPresenter = hpBarPresenter;
            _uiController = uiController;
            _settingsRepository = settingsRepository;
            _playersViewData = playersViewData;
        }

        public void Initialize()
        {
            _uiController.SetStartGameClickHandler(StartGame);
            _uiController.SetStartGameWithBuffsClickHandler(StartGameWithBuffs);

            for (var i = 0; i < _settingsRepository.Settings.settings.playersCount; i++)
                _playerViewRepository.AddView(i, _playersViewData[i].playerView);

            StartGame();
        }

        private void StartGame()
        {
            _playerRepository.Clear();
            
            for (var i = 0; i < _settingsRepository.Settings.settings.playersCount; i++)
            {
                var newPlayer = _playerFactory.CreatePlayer(i);
                _playerRepository.AddPlayer(i, newPlayer);
            }
            
            ReInitGame();
        }

        private void StartGameWithBuffs()
        {
            _playerRepository.Clear();
            
            for (var i = 0; i < _settingsRepository.Settings.settings.playersCount; i++)
            {
                var newPlayer = _playerFactory.CreatePlayerWithBuffs(i);
                _playerRepository.AddPlayer(i, newPlayer);
            }
            
            ReInitGame();
        }

        private void ReInitGame()
        {
            foreach (var player in _playerRepository.GetAll())
            {
                var playerView = _playerViewRepository.Get(player.TeamId);
                playerView.ResetAnimator(player.HP);
            }
            
            _hpBarPresenter.ReInit();
            _uiController.ReInit();
        }
    }
}