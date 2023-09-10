using Commerce6.Data.Domain.Contact;

namespace Commerce6.Infrastructure.Repositories.Contact
{
    public sealed class AddressRepository : BaseRepository<Address>
    {
        public AddressRepository(Context context) : base(context) { }
    }
}
