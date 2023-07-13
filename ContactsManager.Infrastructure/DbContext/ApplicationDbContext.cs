using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Mapping between Model class and db tables
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            //Seeding
            string countriesJSONData = File.ReadAllText("countries.json");

            List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJSONData);
            foreach (Country country in countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }

            string personsJSONData = File.ReadAllText("persons.json");
            List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJSONData);

            foreach (Person person in persons)
            {
                modelBuilder.Entity<Person>().HasData(person);
                modelBuilder.Entity<Person>().Property(temp => temp.Pincode)
                    .HasColumnName("ZipCode")
                    .HasColumnType("varchar(6)")
                    .HasDefaultValue("639005");

                modelBuilder.Entity<Person>().HasCheckConstraint("chk_ZipCodeConstraint", "len([ZipCode])=6");
            }

        }

        public List<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXEC [dbo].[GetAllPersons]").ToList();
        }
        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@PersonID",person.PersonID),
                new SqlParameter("@PersonName",person.PersonName),
                new SqlParameter("@Email", person.Email),
                new SqlParameter("@DateOfBirth", person.DateOfBirth),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@CountryID", person.CountryID),
                new SqlParameter("@Address", person.Address),
                new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters)
            };
            return Database.ExecuteSqlRaw("EXEC [dbo].[InsertPerson] @PersonID,@PersonName,@Email,@DateOfBirth,@Gender,@CountryID,@Address,@ReceiveNewsLetters", sqlParameters);
        }
    }
}
