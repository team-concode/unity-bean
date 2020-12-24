using System.Threading.Tasks;
using UnityEngine;

[UnityBean.Controller]
public class TestController {
    [UnityBean.AutoWired] 
    private TestService testService;

    public async Task<bool> Initialize() {
        Test();
        return true;
    }

    private void Test() {
        Debug.Log(testService.GetValue());
    }
}