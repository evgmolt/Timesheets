using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Timesheets.Data.Interfaces;
using Timesheets.Models;
using Timesheets.Models.Dto;

namespace Timesheets.Data.Implementations
{
    /// <summary> Реализация интерфейса репозитория Person </summary>
    public class PersonsRepository : IRepository
    {
        private readonly string _jsonFileName = "persons.json";

        private readonly ILogger<PersonsRepository> _logger;
        private List<Person> _persons;

        public PersonsRepository(ILogger<PersonsRepository> logger)
        {
            _logger = logger;
            try
            {
                var json = File.ReadAllText(_jsonFileName);
                _persons = JsonSerializer.Deserialize<List<Person>>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _persons = new List<Person>();
            }        
        }

        private bool UpdatePersonsFile()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            try
            {
                var json = JsonSerializer.Serialize(_persons, options);
                File.WriteAllText(_jsonFileName, json);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public int Create(PersonRequest personRequest)
        {
            int maxId;
            try
            {
                maxId = _persons.Max(p => p.Id);
            }
            catch (InvalidOperationException)
            {
                maxId = 0;
            }
            maxId++;
            var person = new Person
            {
                Id = maxId,
                FirstName = personRequest.FirstName,
                LastName = personRequest.LastName,
                Email = personRequest.Email,
                Company = personRequest.Company,
                Age = personRequest.Age
            };
            _persons.Add(person);
            return UpdatePersonsFile() ? person.Id : 0;
        }

        public Person GetPersonById(int id)
        {
            try
            {
                return _persons.Single(p => p.Id == id);
            }
            catch (InvalidOperationException)
            {
                return default;
            }
        }

        public IEnumerable<Person> GetPersonsByName(string name)
        {
            try
            {
                return _persons.Where(p => p.FirstName == name || p.LastName == name);
            }
            catch (InvalidOperationException)
            {
                return default;
            }
        }

        public IEnumerable<Person> GetPersonsWithPagination(int skip, int take)
        {
            try
            {
                return _persons.Where(p => p.Id > skip && p.Id < (skip + take + 1));
            }
            catch (InvalidOperationException)
            {
                return default;
            }
        }

        public bool Update(Person person)
        {
            Person personForUpdate = null;
            try
            {
                personForUpdate = _persons.Single(p => p.Id == person.Id);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            if (_persons.Remove(personForUpdate))
            {
                _persons.Add(person);
            }
            else
            {
                return false;
            }
            return UpdatePersonsFile();
        }

        public bool Delete(int id)
        {
            Person personForDelete;
            try
            {
                personForDelete = _persons.Single(p => p.Id == id);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            return _persons.Remove(personForDelete) && UpdatePersonsFile();
        }

    }
}
