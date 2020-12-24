using System.Threading.Tasks;

[UnityBean.Repository]
public class TestRepository {
    public async Task<bool> Initialize() {
        return true;
    }

    public string ReadValue() {
        return "Hello";
    }
}
