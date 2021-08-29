using GameKing.Shared.MessagePackObjects;
using GameKing.Unity.NinjaKid.Map;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameKing.Unity.NinjaKid
{
    public class NinjaKidGameInstaller : MonoInstaller
    {
        [SerializeField] private MarkView markPrefab;

        public override void InstallBindings()
        {
            MainThreadDispatcher.Initialize();

            Container.BindInterfacesAndSelfTo<NinjaKidServerService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MapService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MarkService>().AsSingle();

            Container.BindInterfacesAndSelfTo<MapView>().FromInstance(FindObjectOfType<MapView>());
            Container.Bind<MarkView>().WithId("Mark0").FromComponentInNewPrefab(markPrefab).AsTransient();
            Container.Bind<MarkView>().WithId("Mark1").FromComponentInNewPrefab(markPrefab).AsTransient();

            Container.Bind<InGameScreen>().FromInstance(FindObjectOfType<InGameScreen>());

            SignalBusInstaller.Install(Container);
        }
    }
}