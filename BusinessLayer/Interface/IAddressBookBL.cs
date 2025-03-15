using ModalLayer.Modal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IAddressBookBL
    {
        Task<IEnumerable<AddressBookEntryModel>> GetAllContactsAsync();
        Task<AddressBookEntryModel> GetContactByIdAsync(int id);
        Task<AddressBookEntryModel> AddContactAsync(RequestModel request);
        Task<AddressBookEntryModel> UpdateContactAsync(int id, RequestModel request);
        Task<bool> DeleteContactAsync(int id);
    }
}
