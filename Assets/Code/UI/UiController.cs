using System;
using System.Collections.Generic;
using System.Globalization;
using Code.Data;
using Code.Settings;
using UnityEngine;
using Zenject;

namespace Code.UI
{
    public sealed class UiController : IInitializable
    {
        private readonly UiContainer _uiContainer;
        private readonly PlayerRepository _playerRepository;
        private readonly SettingsRepository _settingsRepository;
        private readonly ActionPerformer _actionPerformer;
        private readonly List<PlayerViewData> _playersViewData;
        private readonly Pool<StatView> _pool;

        private readonly Dictionary<TeamStatRelation, StatView> _statViews = new Dictionary<TeamStatRelation, StatView>();
        private readonly Dictionary<TeamStatRelation, StatView> _buffsViews = new Dictionary<TeamStatRelation, StatView>();
        private readonly Dictionary<int, Sprite> _statsSpriteCache = new Dictionary<int, Sprite>();
        private readonly Dictionary<int, Sprite> _buffsSpriteCache = new Dictionary<int, Sprite>();

        private Action _startGameClick;
        private Action _startGameWithBuffsClick;
        

        public UiController(
            UiContainer uiContainer, 
            PlayerRepository playerRepository,
            PrefabsSettings prefabsSettings,
            SettingsRepository settingsRepository,
            ActionPerformer actionPerformer,
            List<PlayerViewData> playersViewData)
        {
            _uiContainer = uiContainer;
            _playerRepository = playerRepository;
            _settingsRepository = settingsRepository;
            _actionPerformer = actionPerformer;
            _playersViewData = playersViewData;
            _pool = new Pool<StatView>(prefabsSettings.StatPrefab, 8);
        }

        public void Initialize()
        {
            _uiContainer.buffReloadBtn.onClick.AddListener(() => _startGameWithBuffsClick?.Invoke());
            _uiContainer.bufflessReloadBtn.onClick.AddListener(() => _startGameClick?.Invoke());

            for (var i = 0; i < _settingsRepository.Settings.settings.playersCount; i++)
            {
                var i1 = i;
                _playersViewData[i].panelView.attackButton.onClick.AddListener(() => _actionPerformer.Attack(i1));
            }
        }

        public void ReInit()
        {
            ClearPanel();

            for (var i = 0; i < _settingsRepository.Settings.settings.playersCount; i++)
            {
                var bindingData = _playersViewData[i];
                var player = _playerRepository.Get(i);
                
                //Добавляем иконки статов
                foreach (var stat in _settingsRepository.Settings.stats)
                    AddStatToPanel(i, stat, bindingData.panelView.statsPanel);
                
                //Выставление значений
                if (_statViews.TryGetValue(new TeamStatRelation(i, player.Armor.ID), out var armorView))
                    armorView.SetLabel(player.Armor.Value.ToString(CultureInfo.InvariantCulture));
                
                if (_statViews.TryGetValue(new TeamStatRelation(i, player.Damage.ID), out var damageView))
                    damageView.SetLabel(player.Damage.Value.ToString(CultureInfo.InvariantCulture));
                
                if (_statViews.TryGetValue(new TeamStatRelation(i, player.Vampirism.ID), out var vampirView))
                    vampirView.SetLabel(player.Vampirism.Value.ToString(CultureInfo.InvariantCulture));

                //Подписка на изменение HP
                if (_statViews.TryGetValue(new TeamStatRelation(i, player.HP.ID), out var hpView))
                {
                    hpView.SetLabel(player.HP.Value.ToString(CultureInfo.InvariantCulture));
                    player.HP.OnChanged += parameter =>
                    {
                        hpView.SetLabel(parameter.Value.ToString(CultureInfo.InvariantCulture));
                    };
                }

                //Добавляем иконки баффов
                foreach (var playerBuff in player.Buffs)
                    AddBuffToPanel(i, playerBuff, bindingData.panelView.statsPanel);
            }
        }

        public void AddStartGameHandler(Action handler)
        {
            _startGameClick = handler;
        }

        public void AddStartGameWithBuffsHandler(Action handler)
        {
            _startGameWithBuffsClick = handler;
        }
        
        private void AddStatToPanel(int teamId, Stat stat, Transform panel)
        {
            if (!_statsSpriteCache.TryGetValue(stat.id, out var sprite))
            {
                sprite = Resources.Load<Sprite>("Icons/" + stat.icon);
                _statsSpriteCache.Add(stat.id, sprite);
            }
            
            var statView = SpawnStatView(panel, sprite);
            _statViews.Add(new TeamStatRelation(teamId, stat.id), statView);
        }

        private void ClearPanel()
        {
            foreach (var view in _buffsViews.Values)
                _pool.Despawn(view);
                
            foreach (var view in _statViews.Values)
                _pool.Despawn(view);
            
            _statViews.Clear();
            _buffsViews.Clear();
        }

        private void AddBuffToPanel(int teamId, Buff buff, Transform panel)
        {
            if (!_buffsSpriteCache.TryGetValue(buff.id, out var sprite))
            {
                sprite = Resources.Load<Sprite>("Icons/" + buff.icon);
                _buffsSpriteCache.Add(buff.id, sprite);
            }

            var buffView = SpawnStatView(panel, sprite, buff.title);
            _buffsViews.Add(new TeamStatRelation(teamId, buff.id), buffView);
        }
        
        private StatView SpawnStatView(Transform panel, Sprite icon, string text = default)
        {
            var statView = _pool.Spawn(panel);
            statView.SetIcon(icon);
            statView.SetLabel(text);

            return statView;
        }
    }

    public struct TeamStatRelation
    {
        public int TeamId;
        public int StatId;

        public TeamStatRelation(int teamId, int statId)
        {
            TeamId = teamId;
            StatId = statId;
        }
    }
}