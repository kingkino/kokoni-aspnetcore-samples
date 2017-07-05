using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kokoni_aspnetcore_samples.Models.Tutorial2;

namespace kokoni_aspnetcore_samples.Models
{
    public class MvcMovieContext : DbContext
    {
        public MvcMovieContext (DbContextOptions<MvcMovieContext> options)
            : base(options)
        {
        }

        public DbSet<kokoni_aspnetcore_samples.Models.Tutorial2.Movie> Movie { get; set; }
    }
}
