using Tool_Menagement.Models;

namespace Tool_Menagement.Interfaces
{
    public interface IRejestracjaRepository
    {
        bool IsOrderValid(int orderId);
        void AddRejestracja(Rejestracja rejestracja);
    }
}
