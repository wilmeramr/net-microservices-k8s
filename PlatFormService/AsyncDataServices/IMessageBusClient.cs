using PlatFormService.Dtos;

namespace PlatFormService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);

    }
}