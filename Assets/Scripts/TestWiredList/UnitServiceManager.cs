using System.Collections.Generic;
using System.Threading.Tasks;
using UnityBean;

[Service]
public class UnitServiceManager {
    [AutoWired] private UnitService[] unitServices;

    public async Task<bool> Initialize() {
        foreach (var service in unitServices) {
            service.Print();
        }
        return true;
    }
}
