using AutoMapper;
using BankProject.Data;
using BankProject.Models;
using BankProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ServiceFabric.Actors;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Fabric.Description;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BankProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ClientService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<ClientController> _logger;
        public ClientController(AppDbContext dbContext, ClientService service, IMapper mapper, ILogger<ClientController> logger)
        {
            _dbContext = dbContext;
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<ClientDisplayDto>> GetClients()
        {
            //Using Service:

            var data = await _service.GetAll();
            if (data == null)
            {
                return NotFound();
            }
            _logger.LogInformation("Get clients is successful");
            return Ok(data);


            //Using dbcontext:

            //if (_dbContext.Clients == null)
            //{
            //    _logger.LogInformation("Clients Database does not exist");
            //    return NotFound();
            //}

            //_logger.LogInformation("Get clients is successful");
            //return Ok(_dbContext.Clients.Select(client => _mapper.Map<ClientDto>(client)));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(string id)
        {
            //Using Service:

            //var data =  await _service.GetClientbyID(id);
            //if(data == null)
            //{
            //    return NotFound();
            //}

            //_logger.LogInformation("Get client with id = {id} is successful", id);
            //return data;



            //Using dbcontext:

            if (_dbContext.Clients == null)
            {
                return NotFound();
            }

            var ent = await _dbContext.Clients.FindAsync(id);

            if (ent == null)
            {
                return NotFound();
            }

            _logger.LogInformation("Get client with id = {id} is successful", id);
            return ent;
        }

        [HttpPost("AddClient")]
        public async Task<IActionResult> AddClient(ClientDto client)
        {
            try
            {
                var newClient = _mapper.Map<Client>(client);
                _dbContext.Clients.Add(newClient);

                await _dbContext.SaveChangesAsync();
                //var clientResp = await _service.AddClient(client);

                _logger.LogInformation("Client added successfully");

                return Created($"/client/{newClient.NationalId}", newClient);

            }
            catch
            {
                return StatusCode(500, "An error occured while adding client");
            }
        }

        

        //[HttpPost("AddAddress")]

        //public async Task<IActionResult> AddAddress(string id, string street, string country, string city)
        //{
        //    try
        //    {
        //        if (ClientAvail(id))
        //        {
        //            Address clientAddresss = new Address(street, country, city);
        //            var client = await _dbContext.Clients.FindAsync(id);
        //            var address = await _dbContext.Addresses.FindAsync(id);
        //            if (address == null) { return NotFound(); }
        //            else
        //            {
        //                if (client != null)
        //                {
        //                    address.Client = client;
        //                }
        //                else { return NotFound(); }

        //            }

        //            client.Addresses.Add(address);
        //            await _dbContext.SaveChangesAsync();
        //            _logger.LogInformation("Address added to client with id = {0}", id);
        //            return Ok();

        //        }
        //        else
        //        {
        //            return StatusCode(500, "Client does not exist.");
        //        }
        //    }
        //    catch
        //    {
        //        return StatusCode(500, "An error occured while adding client address");
        //    }
        //}

        [HttpPut]

        public async Task<ActionResult<Client>> UpdateClient(string id, Client client)
        {
            if (id != client.NationalId)
            {
                return BadRequest();
            }

            _dbContext.Entry(client).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientAvail(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            _logger.LogInformation("Client updated successfully");
            return Ok();
        }

        private bool ClientAvail(string id)
        {
            return (_dbContext.Clients?.Any(x => x.NationalId == id)).GetValueOrDefault();
        }

        [HttpDelete]

        public async Task<ActionResult<Client>> DeleteClient(string id)
        {
            if (_dbContext.Clients == null)
            {
                return NotFound();
            }

            var client = await _dbContext.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            _dbContext.Clients.Remove(client);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
