using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatFormService.AsyncDataServices;
using PlatFormService.Data;
using PlatFormService.Dtos;
using PlatFormService.Models;
using PlatFormService.SyncDataServices.Http;

namespace PlatFormService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(IPlatformRepo repository
        , IMapper mapper
        , ICommandDataClient commandDataClient
        , IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;

        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("-->Getting Platforms..");
            var platformItem = _repository.GetAllPlatforms();
            var mapper = _mapper.Map<IEnumerable<PlatformReadDto>>(platformItem);
            return Ok(mapper);

        }


        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            Console.WriteLine("--> ");

            var platformItem = _repository.GetPlatformById(id);

            if (platformItem != null)
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {

            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            // Send Sync


            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"--> Could no send synchronously: {ex.Message} ");
            }

            //send Asyn Message
            try
            {
                var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
                platformPublishedDto.Event = "Platform_Published";

                _messageBusClient.PublishNewPlatform(platformPublishedDto);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could no send asynchronously: {ex.Message} ");
            }
            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);

        }

    }
}