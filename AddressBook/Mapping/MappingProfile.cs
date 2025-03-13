using AutoMapper;
using RepositoryLayer.Entity;
using ModalLayer.Modal;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddressBookEntry, AddressBookEntryModel>().ReverseMap();
    }
}