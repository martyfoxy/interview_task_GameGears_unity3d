using System.Collections.Generic;

namespace Code.Players
{
    public sealed class PlayerViewRepository
    {
        private readonly Dictionary<int, PlayerView> _container = new Dictionary<int, PlayerView>();
        
        public void AddView(int teamId, PlayerView view)
        {
            _container.Add(teamId, view);
        }

        public PlayerView Get(int teamId)
        {
            return _container[teamId];
        }
    }
}