using System.Collections.Generic;
using Code.Players;
using Code.Settings;
using Code.UI;
using UnityEngine;
using Zenject;

namespace Code
{
    public sealed class EntryInstaller : MonoInstaller
    {
        [SerializeField]
        private CameraView cameraView;
        
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
            
            Container.BindInterfacesAndSelfTo<CameraView>()
                .FromInstance(cameraView)
                .AsSingle();

            Container.Bind<PlayerFactory>()
                .AsSingle();

            Container.Bind<PlayerRepository>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<GameManager>()
                .AsSingle()
                .WithArguments(playersViewData);

            Container.Bind<PlayerViewRepository>()
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

            Container.Bind<HpBarPresenter>()
                .AsSingle();
        }
    }
}