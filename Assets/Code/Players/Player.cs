using System.Collections.Generic;
using System.Linq;
using Code.Data;

namespace Code.Players
{
    public sealed class Player
    {
        public readonly int TeamId;
        public readonly Parameter HP;
        public readonly Parameter Armor;
        public readonly Parameter Damage;
        public readonly Parameter Vampirism;

        public List<Buff> Buffs { get; }
        
        public Player(
            int teamId,
            Parameter hp,
            Parameter armor,
            Parameter damage,
            Parameter vampirism,
            List<Buff> buffs)
        {
            TeamId = teamId;
            HP = hp;
            Armor = armor;
            Damage = damage;
            Vampirism = vampirism;

            Buffs = buffs;
        }

        public Parameter GetParameterById(int statId)
        {
            var temp = new List<Parameter>()
            {
                HP, Armor, Damage, Vampirism
            };
            
            return temp.FirstOrDefault(x => x.ID == statId);
        }

        public float TakeDamage(float damage)
        {
            if (damage < 0)
                damage = 0;
            
            var resDamage = (HP.InitValue - Armor.Value) / HP.InitValue * damage;
            HP.Value -= resDamage;
            
            return resDamage;
        }

        public void VampirismRestore(float resDamage)
        {
            if (resDamage < 0)
                resDamage = 0;
            
            var restoredHp = Vampirism.Value / HP.InitValue * resDamage;
            HP.Value += restoredHp;
        }
    }
}