﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharedLibraryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraryCore.Configuration
{
    public class BaseConfigurationHandler<T> : IConfigurationHandler<T> where T : IBaseConfiguration
    {
        string Filename;
        IConfigurationRoot ConfigurationRoot { get; set; }
        T _configuration;

        public BaseConfigurationHandler(string fn)
        {
            Filename = fn;
            Build();
        }

        public void Build()
        {
            ConfigurationRoot = new ConfigurationBuilder()
              .AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}{Filename}.json", true)
              .Build();

            _configuration = ConfigurationRoot.Get<T>();

            if (_configuration == null)
                _configuration = default(T);
        }

        public Task Save()
        {
            var appConfigJSON = JsonConvert.SerializeObject(_configuration, Formatting.Indented);
            return Task.Factory.StartNew(() =>
            {
                File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}{Filename}.json", appConfigJSON);
            });
        }

        public T Configuration() => _configuration;

        public void Set(T config)
        {
            _configuration = config;
        }
    }
}
