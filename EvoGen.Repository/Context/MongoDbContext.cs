﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace EvoGen.Repository.Context
{
    public class MongoDbContext<TColletion> where TColletion: class
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext()
        {
            var database = ConfigurationManager.AppSettings["DataBase"];
            var mongoConect = ConfigurationManager.AppSettings["MongoConect"];
            var username = ConfigurationManager.AppSettings["Username"];
            var password = ConfigurationManager.AppSettings["Password"];
            var port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            var credential = MongoCredential.CreateCredential(database, username, password);
            var mongoClientSettings = new MongoClientSettings
            {
                Server = new MongoServerAddress(mongoConect, port),
                Credential = credential
            };

            if (string.IsNullOrEmpty(mongoConect) || string.IsNullOrEmpty(database))
                throw new System.Exception("Cannot find MongoConect or DataBase");

            //var client = new MongoClient(mongoConect);
            var client = new MongoClient(mongoClientSettings);
            if (client != null)
                _database = client.GetDatabase(database);
        }

        public IMongoCollection<TColletion> Collection
        {
            get
            {
                return _database.GetCollection<TColletion>(typeof(TColletion).Name);
            }
        }
    }
}
