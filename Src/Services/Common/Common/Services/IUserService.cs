namespace Common.Services
{
    public interface IUserService
    {
        int GetId();
        string GetName();
        bool IsAdmin();
    }
}
