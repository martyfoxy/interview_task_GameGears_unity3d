using Code.Players;

namespace Code
{
    public sealed class ActionPerformer
    {
        private readonly PlayerViewRepository _playerViewRepository;
        private readonly PlayerRepository _playerRepository;

        public ActionPerformer(PlayerViewRepository playerViewRepository, PlayerRepository playerRepository)
        {
            _playerViewRepository = playerViewRepository;
            _playerRepository = playerRepository;
        }

        public void Attack(int teamId)
        {
            var attackPlayer = _playerRepository.Get(teamId);
            if (attackPlayer.HP.Value <= float.Epsilon)
                return;
            
            var defencePlayer = _playerRepository.GetOtherPlayer(teamId);
            if (defencePlayer.HP.Value <= float.Epsilon)
                return;
            
            var takenDamage = defencePlayer.TakeDamage(attackPlayer.Damage.Value);
            
            attackPlayer.VampirismRestore(takenDamage);
            
            var playerView = _playerViewRepository.Get(attackPlayer.TeamId);
            var enemyView = _playerViewRepository.Get(defencePlayer.TeamId);
            
            playerView.PlayAttack();
            enemyView.SetHpParameter(defencePlayer.HP);
        }
    }
}