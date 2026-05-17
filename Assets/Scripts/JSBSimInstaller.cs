using Assets.Scripts.JSBSim;
using Zenject;

namespace Assets.Scripts
{
    public class JSBSimInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IJSBSimService>()
                .To<JSBSimService>()
                .AsSingle();

            Container.Bind<IJSBSimAircraft>()
                .To<JSBSimAircraft>()
                .AsSingle();
        }
    }
}
