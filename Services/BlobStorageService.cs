using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using Api_face_recognition.Domain;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Api_face_recognition.Services
{
    public class BlobStorageService 
    {
        private readonly BlobContainerClient _container ;
        public BlobStorageService(string ConnectionString, string ContainerName)
        {
           _container = new BlobContainerClient(ConnectionString, ContainerName);
        }
    }
}
