using System.Collections.Generic;
using System.Linq;
using Code.Data;
using Code.Settings;
using UnityEngine;

namespace Code.Players
{
    public sealed class PlayerFactory
    {
        private readonly SettingsRepository _settingsRepository;
        
        public PlayerFactory(SettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public Player CreatePlayerWithBuffs(int teamId)
        {
            var buffs = GetRandomBuffs();
            
            return new Player(
                teamId: teamId,
                hp: CreateById(StatsId.LIFE_ID, buffs),
                armor: CreateById(StatsId.ARMOR_ID, buffs),
                damage: CreateById(StatsId.DAMAGE_ID, buffs),
                vampirism: CreateById(StatsId.LIFE_STEAL_ID, buffs),
                buffs: buffs);
        }
        
        public Player CreatePlayer(int teamId)
        {
            var buffs = new List<Buff>();

            return new Player(
                teamId:teamId, 
                hp: CreateById(StatsId.LIFE_ID, buffs),
                armor: CreateById(StatsId.ARMOR_ID, buffs),
                damage: CreateById(StatsId.DAMAGE_ID, buffs),
                vampirism: CreateById(StatsId.LIFE_STEAL_ID, buffs),
                buffs:buffs);
        }

        private Parameter CreateById(int statId, List<Buff> buffs)
        {
            var list = _settingsRepository.Settings.stats.ToList();
            var statData = list.FirstOrDefault(x => x.id == statId);

            if (statData == null)
            {
                Debug.LogError($"Couldn't create parameter for ID {statId}");
                return null;
            }

            var value = statData.value;
            
            foreach (var buff in buffs)
            {
                var buffStat = buff.stats.FirstOrDefault(x => x.statId == statId);
                if (buffStat != null)
                    value += buffStat.value;
            }
            
            return new Parameter(statId, value);
        }
        
        private List<Buff> GetRandomBuffs()
        {
            List<Buff> buffList = new List<Buff>();
            
            var buffCount = Random.Range(_settingsRepository.Settings.settings.buffCountMin, _settingsRepository.Settings.settings.buffCountMax);

            for (var i = 0; i < buffCount; i++)
            {
                var randomIndex = Random.Range(0, _settingsRepository.Settings.buffs.Length - 1);
                var newBuff = _settingsRepository.Settings.buffs[randomIndex];

                if (_settingsRepository.Settings.settings.allowDuplicateBuffs || !buffList.Contains(newBuff))
                    buffList.Add(newBuff);
            }

            return buffList;
        }
    }
}