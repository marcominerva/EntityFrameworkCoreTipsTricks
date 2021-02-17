using Ef5TipsTricks.DataAccess.Entities.Common;

namespace Ef5TipsTricks.DataAccess.Entities
{
    public class Address : DeletableEntity
    {
        public int Id { get; set; }

        public int PersonId { get; set; }

        public AddressType Type { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public virtual Person Person { get; set; }
    }

    public enum AddressType
    {
        Home,
        Work
    }
}
