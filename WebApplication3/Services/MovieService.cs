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
        PaginatedList<MovieGetModel> GetAll(int page, DateTime? from = null, DateTime? to = null);
        Movie GetById(int id);
        Movie Create(Movie movie, User addedBy);
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

        public Movie Create(Movie movie, User addedBy)
        {
           // Movie toAdd = MoviePostModel.ToMovie(movie);
            context.Movies.Add(movie);
            movie.Owner = addedBy;
            context.SaveChanges();
            return movie;
        }

        public Movie Delete(int id)
        {
            var existing = context.Movies.Include(c=>c.
            Comments).FirstOrDefault(movie => movie.Id == id);
            if (existing == null)
            {
                return null;
            }

            // o varianta de a sterge si comentariile odata cu filmul
            foreach (var Comment in existing.Comments)
            {
                context.Comments.Remove(Comment);
            }

            context.Movies.Remove(existing);
            context.SaveChanges();

            return existing;
        }

        public PaginatedList<MovieGetModel> GetAll(int page, DateTime? from = null, DateTime? to = null)
        {
            IQueryable<Movie> result = context
                .Movies
                .OrderBy(m => m.Id)
                .Include(c => c.Comments).OrderByDescending(m => m.ReleaseYear);

            PaginatedList<MovieGetModel> paginatedResult = new PaginatedList<MovieGetModel>();
            paginatedResult.CurrentPage = page;
            
            if (from != null)
            {
                result = result.Where(e => e.DateAdded >= from);
            }
            if (to != null)
            {
                result = result.Where(e => e.DateAdded <= to);
            }
            paginatedResult.NumberOfPages = (result.Count() - 1) / PaginatedList<MovieGetModel>.EntriesPerPage + 1;
            result = result
                .Skip((page - 1) * PaginatedList<MovieGetModel>.EntriesPerPage)
                .Take(PaginatedList<MovieGetModel>.EntriesPerPage);
            paginatedResult.Entries = result.Select(m => MovieGetModel.FromMovie(m)).ToList();

            return paginatedResult;
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
