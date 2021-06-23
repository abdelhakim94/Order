using System.Threading.Tasks;
using Order.Shared.Contracts;

namespace Order.Client.Services
{
    /// <summary>
    /// https://github.com/arivera12/BlazorHistory
    /// </summary>
    public interface IHistoryService : IScopedService
    {
        ValueTask GoBack();
    }
}
