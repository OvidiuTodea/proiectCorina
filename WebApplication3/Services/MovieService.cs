using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.Services
{
    public interface IMovieService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        IEnumerable<MovieGetModel> GetAll(DateTime? from = null, DateTime? to = null);
        Movie GetById(int id);
        Movie Create(Movie movie);
        Movie Upsert(int id, Movie movie);
        Movie Delete(int id);
    }
    public class MovieService : IMovieService
    {
        private MoviesDbContext context;
        public MovieService(MoviesDbContext context)
        {
            this.context = context;
        }

        public Movie Create(Movie movie)
        {
            
            context.Movies.Add(movie);
            context.SaveChanges();
            return movie;
        }

        public Movie Delete(int id)
        {
            var existing = context.Movies.Include(c=>c.
            Comments).FirstOrDefault(flower => flower.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Movies.Remove(existing);
            context.SaveChanges();

            return existing;
        }

        public IEnumerable<MovieGetModel> GetAll(DateTime? from = null, DateTime? to = null)
        {
            IQueryable<Movie> result = context.Movies.Include(c => c.Comments).OrderByDescending(m => m.ReleaseYear);

            if (from == null && to == null)
            {
                return result.Select(m=>MovieGetModel.FromMovie(m));
            }
            if (from != null)
            {
                result = result.Where(e => e.DateAdded >= from);
            }
            if (to != null)
            {
                result = result.Where(e => e.DateAdded <= to);
            }
            return result.Select(m => MovieGetModel.FromMovie(m));
        }

        public Movie GetById(int id)
        {
            // sau context.Movies.Find()
            return context.Movies
                .Include(f => f.Comments)
                .FirstOrDefault(f => f.Id == id);
        }

        public Movie Upsert(int id, Movie movie)
        {
            var existing = context.Movies.AsNoTracking().FirstOrDefault(f => f.Id == id);
            if (existing == null)
            {
                context.Movies.Add(movie);
                context.SaveChanges();
                return movie;
            }
            movie.Id = id;
            context.Movies.Update(movie);
            context.SaveChanges();
            return movie;
        }

    }
}
