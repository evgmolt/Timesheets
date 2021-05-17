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
    public class PersonsRepository : IRepository
    {
        private readonly string _jsonFileName = "persons.json";

        private List<Person> persons;

        public PersonsRepository()
        {
            if (File.Exists(_jsonFileName))
            {
                var json = File.ReadAllText(_jsonFileName);
                persons = JsonSerializer.Deserialize<List<Person>>(json);
            }
        }

        private void UpdatePersonsFile()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var json = JsonSerializer.Serialize(persons, options);
            File.WriteAllText(_jsonFileName, json);
        }

        public int Create(PersonRequest personRequest)
        {
            int maxId = persons.Max(p => p.Id);
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
            persons.Add(person);
            UpdatePersonsFile();
            return person.Id;
        }

        public void Delete(int id)
        {
            Person personForDelete = persons.Single(p => p.Id == id);
            persons.Remove(personForDelete);
            UpdatePersonsFile();
        }

        public Person GetPerson(int id)
        {
            return persons.Single(p => p.Id == id);
        }

        public IEnumerable<Person> GetPersonsByName(string name)
        {
            return persons.Where(p => p.FirstName == name || p.LastName == name);
        }

        public IEnumerable<Person> GetPersonsWithPagination(int skip, int take)
        {
            return persons.Where(p => p.Id > skip && p.Id < (skip + take + 1));
        }

        public void Update(Person person)
        {
            var personForUpdate = persons.Single(p => p.Id == person.Id);
            persons.Remove(personForUpdate);
            persons.Add(person);
            UpdatePersonsFile();
        }
    }
}
