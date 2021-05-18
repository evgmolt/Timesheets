using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Models;
using Timesheets.Models.Dto;

namespace Timesheets.Data.Interfaces
{
    /// <summary> Интерфейс репозитория Person </summary>
    public interface IRepository
    {
        public int Create(PersonRequest personRequest);

        public bool Update(Person person);

        public Person GetPersonById(int id);

        public bool Delete(int id);

        public IEnumerable<Person> GetPersonsByName(string name);

        public IEnumerable<Person> GetPersonsWithPagination(int skip, int take);
    }
}
