using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.JSBSim
{
    public interface IJSBSimAircraft
    {
        bool IsLoaded { get; }
        bool Load(string aircraftName);
        void SetInitialConditions();
        void Tick();
        double GetProperty(string property);
    }
}
