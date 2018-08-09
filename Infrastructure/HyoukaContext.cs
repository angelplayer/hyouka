using System;
using hyouka_api.Domain;
using Microsoft.EntityFrameworkCore;


namespace hyouka_api.Infrastructure 
{
    public class HyoukaContext : DbContext 
    {
        private readonly string _databasename = Startup.DATABASE_FILE;

        public HyoukaContext(string databaseName): base() {
            this._databasename = databaseName;
        }

        public HyoukaContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Episode> Episodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuidler) {
            optionBuidler.UseSqlite($"Data Source={_databasename}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuidler) 
        {
            modelBuidler.Entity<MovieGenre>(m => 
            {
                m.HasKey(x => new { x.GenreId, x.MovieId });
            });
        }

    }
}