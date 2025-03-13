using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ModalLayer.Modal;
using NLog;
using RepositoryLayer.Entity;

[ApiController]
[Route("api/[controller]")]
public class AddressBookController : ControllerBase
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly IMapper _mapper;
    private readonly IValidator<AddressBookEntryModel> _validator;
    private static List<AddressBookEntry> _addressBookEntries = new List<AddressBookEntry>();
    private static int _idCounter = 1;

    public AddressBookController(IMapper mapper, IValidator<AddressBookEntryModel> validator)
    {
        _mapper = mapper;
        _validator = validator;
        _logger.Info("Logger has been integrated");
    }

    [HttpGet]
    public IActionResult GetAllContacts()
    {
        var contacts = _mapper.Map<IEnumerable<AddressBookEntryModel>>(_addressBookEntries);
        return Ok(new ResponseModel<IEnumerable<AddressBookEntryModel>>
        {
            Success = true,
            Message = "Contacts fetched successfully.",
            Data = contacts
        });
    }

    [HttpGet("{id}")]
    public IActionResult GetContactById(int id)
    {
        var contact = _addressBookEntries.FirstOrDefault(e => e.Id == id);
        if (contact == null)
        {
            return NotFound(new ResponseModel<string> { Success = false, Message = "Contact not found." });
        }

        return Ok(new ResponseModel<AddressBookEntryModel>
        {
            Success = true,
            Message = "Contact fetched successfully.",
            Data = _mapper.Map<AddressBookEntryModel>(contact)
        });
    }

    [HttpPost]
    public IActionResult AddContact([FromBody] AddressBookEntryModel request)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ResponseModel<string>
            {
                Success = false,
                Message = "Validation failed.",
                Data = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))
            });
        }

        var newContact = _mapper.Map<AddressBookEntry>(request);
        newContact.Id = _idCounter++;
        _addressBookEntries.Add(newContact);

        return CreatedAtAction(nameof(GetContactById), new { id = newContact.Id }, new ResponseModel<AddressBookEntryModel>
        {
            Success = true,
            Message = "Contact added successfully.",
            Data = _mapper.Map<AddressBookEntryModel>(newContact)
        });
    }

    [HttpPut("{id}")]
    public IActionResult UpdateContact(int id, [FromBody] AddressBookEntryModel request)
    {
        var existingContact = _addressBookEntries.FirstOrDefault(e => e.Id == id);
        if (existingContact == null)
        {
            return NotFound(new ResponseModel<string> { Success = false, Message = "Contact not found." });
        }

        _mapper.Map(request, existingContact);
        return Ok(new ResponseModel<AddressBookEntryModel>
        {
            Success = true,
            Message = "Contact updated successfully.",
            Data = _mapper.Map<AddressBookEntryModel>(existingContact)
        });
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteContact(int id)
    {
        var contactToDelete = _addressBookEntries.FirstOrDefault(e => e.Id == id);
        if (contactToDelete == null)
        {
            return NotFound(new ResponseModel<string> { Success = false, Message = "Contact not found." });
        }

        _addressBookEntries.Remove(contactToDelete);
        return NoContent();
    }
}