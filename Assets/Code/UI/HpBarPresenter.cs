using Code.Parameters;
using Code.Settings;
using Zenject;

namespace Code.UI
{
    public sealed class HpBarPresenter : IInitializable
    {
        private readonly PlayerRepository _playerRepository;
        private readonly PrefabsSettings _prefabsSettings;
        private readonly SettingsRepository _settingsRepository;
        private readonly PlayerViewRepository _playerViewRepository;

        private Pool<DamageTakenView> _pool;
        
        public HpBarPresenter(
            PlayerRepository playerRepository,
            PrefabsSettings prefabsSettings,
            SettingsRepository settingsRepository, 
            PlayerViewRepository playerViewRepository)
        {
            _playerRepository = playerRepository;
            _prefabsSettings = prefabsSettings;
            _settingsRepository = settingsRepository;
            _playerViewRepository = playerViewRepository;
        }

        public void Initialize()
        {
            _pool = new Pool<DamageTakenView>(_prefabsSettings.DamageTakenPrefab, 3);
        }

        public void ReInit()
        {
            for (var i = 0; i < _settingsRepository.Settings.settings.playersCount; i++)
            {
                var player = _playerRepository.Get(i);
                var hpBarView = _playerViewRepository.Get(i).HpBarView;
                
                UpdateHpBar(hpBarView, player.HP);
                
                player.HP.OnChanged += parameter =>
                {
                    UpdateHpBar(hpBarView, parameter);
                    
                    var takenDamage = parameter.OldValue - parameter.Value;
                    if (takenDamage > 0f)
                    {
                        var view = _pool.Spawn(hpBarView.transform);
                        view.ShowDamage(takenDamage, () => _pool.Despawn(view));
                    }
                };
            }
        }

        private void UpdateHpBar(HpBarView hpBarView, Parameter parameter)
        {
            hpBarView.SetValue(parameter);
        }
    }
}