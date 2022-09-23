using System.Collections.Generic;
using Code.Parameters;
using Code.Settings;
using Code.UI;
using UnityEngine;
using Zenject;

namespace Code
{
    public sealed class EntryInstaller : MonoInstaller
    {
        [SerializeField]
        private List<PlayerViewData> playersViewData;

        [SerializeField]
        private UiContainer uiContainer;

        [SerializeField]
        private PrefabsSettings prefabsSettings;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SettingsRepository>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerFactory>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerRepository>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<GameManager>()
                .AsSingle()
                .WithArguments(playersViewData);

            Container.BindInterfacesAndSelfTo<PlayerViewRepository>()
                .AsSingle();
            
            Container.Bind<ActionPerformer>()
                .AsSingle();
            
            InstallUI();
        }

        private void InstallUI()
        {
            Container.Bind<PrefabsSettings>()
                .FromInstance(prefabsSettings)
                .AsSingle();
            
            Container.Bind<UiContainer>()
                .FromInstance(uiContainer)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<UiController>()
                .AsSingle()
                .WithArguments(playersViewData);

            Container.BindInterfacesAndSelfTo<HpBarPresenter>()
                .AsSingle();
        }
    }
}