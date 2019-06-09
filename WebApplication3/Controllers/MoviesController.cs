using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebApplication3.Models;
using WebApplication3.Services;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private IMovieService movieService;
        private IUsersService usersService;
        public MoviesController(IMovieService movieService,IUsersService usersService)
        {
            this.movieService = movieService;
            this.usersService = usersService;
        }
        /// <summary>
        /// Get all the movies
        /// </summary>
        /// <param name="from">date</param>
        /// <param name="to">date</param>
        /// <returns>A list of movie objects</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        // ? permite unui struct sa ia si valoare null
        public PaginatedList<MovieGetModel> Get([FromQuery]DateTime? from, [FromQuery]DateTime? to, [FromQuery]int page = 1)
        {
            page = Math.Max(page, 1);
            return movieService.GetAll(page, from,to);
        }

        /// <summary>
        /// Gets a movie by its ID
        /// </summary>
        /// <param name="id">The Id of the movie</param>
        /// <returns>The movie associated with the id param</returns>
        // GET: api/Movies/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var found = movieService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found);
        }

        /// <summary>
        /// Add a movie 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /movies
        ///  {
        ///   "title": "Test",
        ///   "description": "Description of the movie",
        ///  "duration": 120,
        ///  "genre": 0,
        ///  "releaseYear": 2018,
        ///  "dateAdded": "2019-05-10T09:10:00",
        ///  "director": "Corina",
        ///  "rating": 8,
        ///  "watched": true,
        ///  "comments": [
        /// 	{"text":"comment",
        /// 	"important":true,
        ///    }
        ///	  ]
        ///  }
        ///</remarks>
        /// <param name="movie"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin,Regular")] //va functiona doar pt aceste roluri
        [HttpPost]
        public void Post([FromBody] Movie movie)
        {
            User addedBy = usersService.GetCurrentUser(HttpContext);
            movieService.Create(movie, addedBy);
        }
        /// <summary>
        /// Updates or creates a movie with a given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        // PUT: api/Movies/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin,Regular")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Movie movie)
        {
            var result = movieService.Upsert(id, movie);
            return Ok(result);
        }
        /// <summary>
        /// Deletes a movies by it's id
        /// </summary>
        /// <param name="id">the id of the movie to be deleted</param>
        /// <returns></returns>
        // DELETE: api/ApiWithActions/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin,Regular")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = movieService.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
