using System.Threading.Tasks;
using PlatFormService.Dtos;

namespace PlatFormService.SyncDataServices.Http
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDto plat);
    }

}