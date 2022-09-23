using System.Collections.Generic;
using System.Linq;

namespace Code
{
    public sealed class PlayerRepository
    {
        private readonly Dictionary<int, Player> _container = new Dictionary<int, Player>();

        public List<Player> GetAll()
        {
            return _container.Values.ToList();
        }

        public void Clear()
        {
            _container.Clear();
        }

        public void AddPlayer(int teamId, Player player)
        {
            _container.Add(teamId, player);
        }

        public Player Get(int teamId)
        {
            return _container[teamId];
        }

        public Player GetOtherPlayer(int teamId)
        {
            return _container.FirstOrDefault(x => x.Key != teamId).Value;
        }
    }
}