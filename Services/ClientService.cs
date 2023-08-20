using AutoMapper;
using BankProject.Data;
using BankProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography;
using System.Text;

namespace BankProject.Services
{
    public class ClientService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public ClientService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        //add client
        public async Task<Client> AddClient(ClientDto client)
        {
            var newClient = _mapper.Map<Client>(client);
            _dbContext.Clients.Add(newClient);

            await _dbContext.SaveChangesAsync();

            return newClient;
        }

        public async Task<ClientDisplayDto> GetAll()
        {
            var clients = await _dbContext.Clients.Include(_ => _.Addresses).ToListAsync();
            var clientsDto = _mapper.Map<ClientDisplayDto>(clients);
            return clientsDto;
        }

        public async Task<ActionResult<Client>> GetClientbyID(string id)
        {
            return await _dbContext.Clients.FindAsync(id);
            
        }

    }
}
