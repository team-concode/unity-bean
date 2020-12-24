using System.Threading.Tasks;
using UnityEngine;

[UnityBean.Service]
public class TestService {
    [UnityBean.AutoWired] 
    private TestRepository repository;
    
    public async Task<bool> Initialize() {
        return true;
    }

    public string GetValue() {
        return repository.ReadValue();
    }
}