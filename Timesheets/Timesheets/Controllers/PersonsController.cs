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
        /// Создает новый объект Person
        /// </summary>
        /// <param name="personRequest"></param>
        /// <returns> id созданного объекта </returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult CreatePerson([FromBody] PersonRequest personRequest)
        {
            int id = _personsRepository.Create(personRequest);
            return id == 0 ? StatusCode(StatusCodes.Status500InternalServerError) : (ActionResult)Ok(id);
        }

        /// <summary>
        /// Получает данные человека по Id
        /// </summary>
        /// <param name="personId"></param>
        /// <returns> Данные человека </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("person/{personId}")]
        public ActionResult GetPersonById([FromRoute] int personId)
        {
            var response = _personsRepository.GetPersonById(personId);
            return response == null ? NotFound() : (ActionResult)Ok(response);
        }

        /// <summary>
        /// Получает список людей с заданным именем
        /// </summary>
        /// <param name="personName"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("personName/{personName}")]
        public ActionResult GetPersonByName([FromRoute] string personName)
        {
            var response = _personsRepository.GetPersonsByName(personName);
            return response.Count() == 0 ? NotFound() : (ActionResult)Ok(response);
        }

        /// <summary>
        /// Получает список людей в заданном диапазоне id
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("personsWithPagination")]
        public ActionResult GetPersonWithPagination(int skip, int take)
        {
            var response = _personsRepository.GetPersonsWithPagination(skip, take);
            return response.Count() == 0 ? NotFound() : (ActionResult)Ok(response);
        }

        /// <summary>
        /// Модифицирует данные человека
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdatePerson([FromBody] Person person)
        {
            return _personsRepository.Update(person) ? Ok() : (ActionResult)NotFound();
        }

        /// <summary>
        /// Удаляет данные человека
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeletePerson(int personId)
        {
            return _personsRepository.Delete(personId) ? Ok() : (ActionResult)NotFound();
        }
    }
}
