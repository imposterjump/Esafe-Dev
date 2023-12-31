﻿using AutoMapper;
using BankProject.Data;
using BankProject.Data.Enums;
using BankProject.Entities;
using BankProject.Helpers;
using BankProject.Models.Address;
using BankProject.Models.Client.Request;
using BankProject.Models.Client.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BankProject.Services
{
    public class ClientService
    {
        private readonly Jwt _jwt;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public ClientService(AppDbContext dbContext, IMapper mapper, IOptions<Jwt> optionsJwt)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _jwt = optionsJwt.Value;
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

        public bool VerifyPassword(string password, string passwordHash)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                string hashedPassword = builder.ToString();

                return passwordHash.Equals(hashedPassword, StringComparison.OrdinalIgnoreCase);
            }
        }

        private string generateJwtToken(Client user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwt.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()), new Claim("Role", user.Role.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(_jwt.ExpireTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private string generateJwtTokenAdmin(Admin user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwt.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()), new Claim("Role", user.Role.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(_jwt.ExpireTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }




        private string randomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }


        


        








        //add client
        public async Task<Client> AddClient(ClientDto client)
        {
            Console.WriteLine("eneterd service");
            var newClient = _mapper.Map<Client>(client);
            _dbContext.Clients.Add(newClient);

            Console.WriteLine(newClient.ClientAddresses);

            await _dbContext.SaveChangesAsync();

            return newClient;
        }

        public async Task Register(ClientDto client)
        {
            Console.WriteLine("entered registeration service");
            if (_dbContext.Clients.Any(x => x.NationalId == client.NationalId))
            {
                Console.WriteLine("client with same national id: {0} already exists", client.NationalId);
                throw new Exception("Client with this national already exists");
            }
            else if (_dbContext.Clients.Any(x => x.Username == client.Username))
            {
                Console.WriteLine("client with same username: {0} already exists", client.Username);
            }
            else
            {
                Console.WriteLine("checking pass & conf pass");
                if (client.Password != null && client.ConfirmPassword != null)
                {
                    if (client.Password.Equals(client.ConfirmPassword))
                    {
                        Console.WriteLine("passwords match, checking client role...");
                        
                        var newClient = _mapper.Map<Client>(client);
                        if (newClient.Email.Contains("uaedigitallab"))
                        {
                            newClient.Role = Role.Admin;

                        }
                        else
                        {
                            newClient.Role = Role.Client;
                        }
                        newClient.Password = HashPassword(client.Password);
                        newClient.AccountNo = GetAccountNo();

                        try
                        {
                            _dbContext.Clients.Add(newClient);
                            _dbContext.SaveChanges();
                            Console.WriteLine("client added successfully");
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine("Passwords do not match.");
                        throw new Exception("Passwords do not match");
                    }
                }
                else
                {
                    Console.WriteLine("Password and confirm password fields cannot be empty.");
                    throw new Exception("Password and confirm password fields cannot be empty.");
                }
                
            }
        }

        public static string GetAccountNo()
        {
            return "11111111111";
        }

        //Authentication Service
        public AuthenticateResponse Authenticate(ClientLoginDto client, string ipAddress)
        {
            if (client == null)
            {
                Console.Write("client input is null");
                throw new Exception("error in the input");

            }
            if (_dbContext.Clients.Any(x => x.Username == client.Username))
            {
                var logClient = _dbContext.Clients.FirstOrDefault(entity => entity.Username == client.Username);
                if (VerifyPassword(client.Password, logClient.Password))
                {
                    // authentication successful so generate jwt and refresh tokens
                    var jwtToken = generateJwtToken(logClient);


                    // save refresh token
                    _dbContext.Update(logClient);
                    _dbContext.SaveChanges();


                    var response = _mapper.Map<AuthenticateResponse>(logClient);
                    response.JwtToken = jwtToken;
                    return response;

                }
                else
                {
                    throw new Exception("Wrong password.");
                }

            }
            else if (_dbContext.Admins.Any(x => x.Username == client.Username))
            {
                var logClient = _dbContext.Admins.FirstOrDefault(entity => entity.Username == client.Username);
                if (VerifyPassword(client.Password, logClient.Password))
                {
                    // authentication successful so generate jwt and refresh tokens
                    var jwtToken = generateJwtTokenAdmin(logClient);
                    Console.WriteLine(logClient);
                    _dbContext.Update(logClient);
                    _dbContext.SaveChanges();
                    var response = _mapper.Map<AuthenticateResponse>(logClient);
                    response.JwtToken = jwtToken;
                    return response;

                }
                else
                {
                    throw new Exception("Wrong password.");
                }

            }
            else
            {
                Console.Write("Username does not exist.");
                throw new Exception("Username does not exist.");
            }
        }

        public async Task<ClientDisplayDto> GetClientAuth(Client client)
        {
            var clientResp = _mapper.Map<ClientDisplayDto>(client);
            return clientResp;
        }

        public async Task<List<ClientDisplayDto>> GetAll()
        {
            Console.WriteLine("enetered get all clients service");
            var clients = await _dbContext.Clients.Include(_ => _.ClientAddresses).ToListAsync();
            Console.WriteLine(clients.ToString());
            List<ClientDisplayDto> clientsDto = _mapper.Map<List<ClientDisplayDto>>(clients);
            Console.WriteLine(clientsDto.ToString());
            return clientsDto;
        }

        public async Task<List<ClientDisplayDto>> GetClientbyID(int id)
        {
            var client = await _dbContext.Clients.Where(client => client.Id == id).Include(_ => _.ClientAddresses).ToListAsync();
            Console.WriteLine(client.ToString());
            List<ClientDisplayDto> retClient = _mapper.Map<List<ClientDisplayDto>>(client);
            return retClient;
        }

        public async Task<ClientDisplayDto> AddAddress(int id,AddressDto address)
        {
            AddressDto newaddress = new AddressDto(address.Country, address.City, address.Street);
            var client = await _dbContext.Clients.FindAsync(id);


            var finAdd = _mapper.Map<Address>(newaddress);
            client.ClientAddresses.Add(finAdd);
            finAdd.Client = client;
            finAdd.ClientID = id;
            _dbContext.Addresses.Add(finAdd);
            _dbContext.Entry(client).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
                var retClient = _mapper.Map<ClientDisplayDto>(client);
                Console.WriteLine(client.ToString());
                return retClient;
            }
            catch
            {
                return null;
               
            }

        }

        public async Task UpdatePassword(string password, Client client)
        {
            string pass = HashPassword(password);
            client.Password = pass;
            _dbContext.Entry(client).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

    }
}
