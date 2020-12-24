using System.Threading.Tasks;
using UnityEngine;

[Controller]
public class TestController {
    [AutoWired] 
    private TestService testService;

    public async Task<bool> Initialize() {
        Test();
        return true;
    }

    private void Test() {
        Debug.Log(testService.GetValue());
    }
}