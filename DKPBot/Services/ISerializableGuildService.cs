using System.Threading.Tasks;

namespace DKPBot.Services
{
    public interface ISerializableGuildService : IGuildService
    {
        Task PopulateAsync();
        Task SerializeAsync();
    }
}