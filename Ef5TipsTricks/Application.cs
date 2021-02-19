using Ef5TipsTricks.DataAccess;
using Ef5TipsTricks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ef5TipsTricks
{
    public class Application
    {
        private readonly IDataContext dataContext;

        public Application(IDataContext dataContext)
            => this.dataContext = dataContext;

        public async Task RunAsync()
        {
            //var newPerson = new Person
            //{
            //    FirstName = " Scrooge",
            //    LastName = "McDuck "
            //};

            //dataContext.Add(newPerson);
            //await dataContext.SaveChangesAsync();

            var people = await dataContext.Get<Person>()
                .Include(p => p.Addresses).ToListAsync();

            //var person = await dataContext.People.FirstOrDefaultAsync();
            //dataContext.Remove(person);
            //await dataContext.SaveChangesAsync();
        }
    }
}
