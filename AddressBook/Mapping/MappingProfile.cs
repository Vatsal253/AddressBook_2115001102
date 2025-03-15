using AutoMapper;
using ModalLayer.Modal;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RequestModel, AddressBookEntry>();
        CreateMap<AddressBookEntry, AddressBookEntryModel>();
    }
}
