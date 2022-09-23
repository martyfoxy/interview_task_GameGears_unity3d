using System;
using System.Collections.Generic;
using System.Globalization;
using Code.Data;
using Code.Players;
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
            _uiContainer.ReloadWithBuffsBtn.onClick.AddListener(() => _startGameWithBuffsClick?.Invoke());
            _uiContainer.ReloadBtn.onClick.AddListener(() => _startGameClick?.Invoke());

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
                
                foreach (var stat in _settingsRepository.Settings.stats)
                {
                    var statView = AddStatToPanel(i, stat, bindingData.panelView.statsPanel);
                    var parameter = player.GetParameterById(stat.id);
                    statView.SetLabel(parameter.Value.ToString(CultureInfo.InvariantCulture));

                    //Динамически изменяется только HP
                    if (stat.id == StatsId.LIFE_ID)
                    {
                        player.HP.OnChanged += hpParameter =>
                        {
                            statView.SetLabel(hpParameter.Value.ToString(CultureInfo.InvariantCulture));
                        };
                    }
                }
                
                foreach (var playerBuff in player.Buffs)
                    AddBuffToPanel(i, playerBuff, bindingData.panelView.statsPanel);
            }
        }

        public void SetStartGameClickHandler(Action handler)
        {
            _startGameClick = handler;
        }

        public void SetStartGameWithBuffsClickHandler(Action handler)
        {
            _startGameWithBuffsClick = handler;
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
        
        private StatView AddStatToPanel(int teamId, Stat stat, Transform panel)
        {
            if (!_statsSpriteCache.TryGetValue(stat.id, out var sprite))
            {
                sprite = Resources.Load<Sprite>("Icons/" + stat.icon);
                _statsSpriteCache.Add(stat.id, sprite);
            }
            
            var statView = SpawnStatView(panel, sprite);
            _statViews.Add(new TeamStatRelation(teamId, stat.id), statView);

            return statView;
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