using ItLabs.MBox.Contracts.Data_Structures;
using ItLabs.MBox.Contracts.Enums;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface ISearchManager
    {
        PriorityQueue<object> Search(string searchValue, SearchType type);
    }
}
