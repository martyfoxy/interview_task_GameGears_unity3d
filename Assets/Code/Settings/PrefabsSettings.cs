using Code.UI;
using UnityEngine;

namespace Code.Settings
{
    [CreateAssetMenu(fileName = "PrefabsSettings", menuName = "Prefabs Settings")]
    public sealed class PrefabsSettings : ScriptableObject
    {
        public StatView StatPrefab;
        public DamageTakenView DamageTakenPrefab;
    }
}