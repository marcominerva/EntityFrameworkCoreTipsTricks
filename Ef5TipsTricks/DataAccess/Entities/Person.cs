using Ef5TipsTricks.DataAccess.Entities.Common;
using System.Collections.Generic;

namespace Ef5TipsTricks.DataAccess.Entities
{
    public class Person : DeletableEntity
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
