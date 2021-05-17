using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Data.Implementations;
using Timesheets.Data.Interfaces;
using Timesheets.Models;
using Timesheets.Models.Dto;

namespace Timesheets.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly ILogger<PersonsController> _logger;
        private readonly IRepository _personsRepository;

        public PersonsController(IRepository personsRepository, ILogger<PersonsController> logger)
        {
            _logger = logger;
            _personsRepository = personsRepository;
        }

        /// <summary>
        /// Получает данные человек по Id
        /// </summary>
        /// <param name="personId"></param>
        /// <returns> Данные человека </returns>
        [HttpGet("person/{personId}")]
        public ActionResult GetPerson([FromRoute] int personId)
        {
            _logger.LogDebug("GetPerson");
            var response = _personsRepository.GetPerson(personId);
            return Ok(response);
        }

        /// <summary>
        /// Получает список людей с заданным именем
        /// </summary>
        /// <param name="personName"></param>
        /// <returns></returns>
        [HttpGet("personName/{personName}")]
        public ActionResult GetPersonByName([FromRoute] string personName)
        {
            var response = _personsRepository.GetPersonsByName(personName);
            return Ok(response);
        }

        /// <summary>
        /// Получает список людей в заданном диапазоне
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [HttpGet("personsWithPagination")]
        public ActionResult GetPersonWithPagination(int skip, int take)
        {
            var response = _personsRepository.GetPersonsWithPagination(skip, take);
            return Ok(response);
        }

        /// <summary>
        /// Создает новый объект Person
        /// </summary>
        /// <param name="personRequest"></param>
        /// <returns> id созданного объекта </returns>
        [HttpPost("create")]
        public ActionResult CreatePerson([FromBody] PersonRequest personRequest)
        {
            int id = _personsRepository.Create(personRequest);
            return Ok(id);
        }

        /// <summary>
        /// Модифицирует данные человека
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public ActionResult UpdatePerson([FromBody] Person person)
        {
            _personsRepository.Update(person);
            return Ok();
        }

        /// <summary>
        /// Удаляет данные человека
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public ActionResult DeletePerson(int personId)
        {
            _personsRepository.Delete(personId);
            return Ok();
        }
    }
}
