using Code.Players;
using Code.Settings;
using Zenject;

namespace Code.UI
{
    public sealed class HpBarPresenter
    {
        private readonly PlayerRepository _playerRepository;
        private readonly SettingsRepository _settingsRepository;
        private readonly PlayerViewRepository _playerViewRepository;
        private readonly Pool<DamageTakenView> _pool;
        
        public HpBarPresenter(
            PlayerRepository playerRepository,
            PrefabsSettings prefabsSettings,
            SettingsRepository settingsRepository, 
            PlayerViewRepository playerViewRepository)
        {
            _playerRepository = playerRepository;
            _settingsRepository = settingsRepository;
            _playerViewRepository = playerViewRepository;
            _pool = new Pool<DamageTakenView>(prefabsSettings.DamageTakenPrefab, 3);
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