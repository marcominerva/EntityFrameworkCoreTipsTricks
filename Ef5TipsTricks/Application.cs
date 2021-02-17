using Ef5TipsTricks.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ef5TipsTricks
{
    public class Application
    {
        private readonly DataContext dataContext;

        public Application(DataContext dataContext)
            => this.dataContext = dataContext;

        public async Task RunAsync()
        {
            var people = await dataContext.People.Include(p => p.Addresses).ToListAsync();
            await dataContext.Addresses.ToListAsync();

            //var person = await dataContext.People.FirstOrDefaultAsync();
            //dataContext.Remove(person);
            //await dataContext.SaveChangesAsync();
        }
    }
}
