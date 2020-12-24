using System.Threading.Tasks;
using UnityEngine;

[Service]
public class TestService {
    [AutoWired] 
    private TestRepository repository;
    
    public async Task<bool> Initialize() {
        return true;
    }

    public string GetValue() {
        return repository.ReadValue();
    }
}