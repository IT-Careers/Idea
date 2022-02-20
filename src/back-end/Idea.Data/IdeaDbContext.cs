using Idea.Data.Models;
using Idea.Data.Models.CelestialObjects;
using Idea.Data.Models.Locations;
using Idea.Data.Models.Materials;
using Idea.Data.Models.Ships;
using Idea.Data.Models.Stations;
using Microsoft.EntityFrameworkCore;

namespace Idea.Data
{
    public class IdeaDbContext : DbContext
    {
        public virtual DbSet<IdeaUser> Users { get; set; }

        public virtual DbSet<BasicChemical> BasicChemicals { get; set; }
        
        public virtual DbSet<BasicMaterial> BasicMaterials { get; set; }
        
        public virtual DbSet<ComplexMaterial> ComplexMaterials { get; set; }
        
        public virtual DbSet<ComplexMaterialRequirement> ComplexMaterialRequirements { get; set; }

        public virtual DbSet<Coordinate> Coordinates { get; set; }

        public virtual DbSet<LocationType> LocationTypes { get; set; }

        public virtual DbSet<Location> Locations { get; set; }

        public virtual DbSet<Spaceship> Ships { get; set; }

        public virtual DbSet<FarmStation> FarmStations { get; set; }

        public virtual DbSet<NecessitiesStation> NecessitiesStations { get; set; }

        public virtual DbSet<NuclearStation> NuclearStations { get; set; }

        public virtual DbSet<ProcessingStation> ProcessingStations { get; set; }

        public virtual DbSet<RecyclingStation> RecyclingStations { get; set; }

        public virtual DbSet<Star> Stars { get; set; }

        public virtual DbSet<PlanetaryType> PlanetaryTypes { get; set; }

        public virtual DbSet<Planetary> Planetaries { get; set; }

        public virtual DbSet<MaterialDeposit> MaterialDeposits { get; set; }

        public IdeaDbContext(DbContextOptions<IdeaDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
