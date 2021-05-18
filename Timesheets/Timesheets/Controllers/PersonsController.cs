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
            if (id == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                return Ok(id);
            }
        }

        /// <summary>
        /// Получает данные человек по Id
        /// </summary>
        /// <param name="personId"></param>
        /// <returns> Данные человека </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("person/{personId}")]
        public ActionResult GetPersonById([FromRoute] int personId)
        {
            var response = _personsRepository.GetPersonById(personId);
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
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
            if (response.Count() == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
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
            if (response.Count() == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
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
            if (_personsRepository.Update(person))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Удаляет данные человека
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public ActionResult DeletePerson(int personId)
        {
            if (_personsRepository.Delete(personId))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
