using Microsoft.AspNetCore.Mvc;
using ModalLayer.Modal;
using NLog;

[ApiController]
[Route("api/[controller]")]
public class AddressBookController : ControllerBase
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private static List<AddressBookEntry> _addressBookEntries = new List<AddressBookEntry>();
    private static int _idCounter = 1;

    public AddressBookController()
    {
        _logger.Info("Logger has been integrated");
    }
    /// <summary>
    /// Get contacts 
    /// </summary>
    /// <returns></returns>
    // GET: api/addressbook
    [HttpGet]
    public IActionResult GetAllContacts()
    {
        ResponseModel<IEnumerable<AddressBookEntry>> responseModel = new ResponseModel<IEnumerable<AddressBookEntry>>();

        responseModel.Success = true;
        responseModel.Message = "Contacts fetched successfully.";
        responseModel.Data = _addressBookEntries;

        _logger.Info("Fetched all contacts successfully.");
        return Ok(responseModel);
    }
    /// <summary>
    /// Get contacts by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // GET: api/addressbook/{id}
    [HttpGet("{id}")]
    public IActionResult GetContactById(int id)
    {
        ResponseModel<AddressBookEntry> responseModel = new ResponseModel<AddressBookEntry>();

        var contact = _addressBookEntries.FirstOrDefault(e => e.Id == id);

        if (contact == null)
        {
            _logger.Warn($"Contact with ID {id} not found.");
            responseModel.Success = false;
            responseModel.Message = "Contact not found.";
            return NotFound(responseModel);
        }

        responseModel.Success = true;
        responseModel.Message = "Contact fetched successfully.";
        responseModel.Data = contact;

        _logger.Info($"Fetched contact with ID {id} successfully.");
        return Ok(responseModel);
    }
    /// <summary>
    /// Add Contacts
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    // POST: api/addressbook
    [HttpPost]
    public IActionResult AddContact([FromBody] RequestModel request)
    {
        ResponseModel<AddressBookEntry> responseModel = new ResponseModel<AddressBookEntry>();

        var newContact = new AddressBookEntry
        {
            Id = _idCounter++,
            Name = request.Name,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Address = request.Address
        };

        _addressBookEntries.Add(newContact);

        responseModel.Success = true;
        responseModel.Message = "Contact added successfully.";
        responseModel.Data = newContact;

        _logger.Info($"Added new contact with name {newContact.Name}.");
        return CreatedAtAction(nameof(GetContactById), new { id = newContact.Id }, responseModel);
    }
    /// <summary>
    /// Edit by Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    // PUT: api/addressbook/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateContact(int id, [FromBody] RequestModel request)
    {
        ResponseModel<AddressBookEntry> responseModel = new ResponseModel<AddressBookEntry>();

        var existingContact = _addressBookEntries.FirstOrDefault(e => e.Id == id);

        if (existingContact == null)
        {
            _logger.Warn($"Contact with ID {id} not found for update.");
            responseModel.Success = false;
            responseModel.Message = "Contact not found.";
            return NotFound(responseModel);
        }

        existingContact.Name = request.Name;
        existingContact.PhoneNumber = request.PhoneNumber;
        existingContact.Email = request.Email;
        existingContact.Address = request.Address;

        responseModel.Success = true;
        responseModel.Message = "Contact updated successfully.";
        responseModel.Data = existingContact;

        _logger.Info($"Updated contact with ID {id} successfully.");
        return Ok(responseModel);
    }
    /// <summary>
    /// Delete by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // DELETE: api/addressbook/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteContact(int id)
    {
        ResponseModel<string> responseModel = new ResponseModel<string>();

        var contactToDelete = _addressBookEntries.FirstOrDefault(e => e.Id == id);

        if (contactToDelete == null)
        {
            _logger.Warn($"Contact with ID {id} not found for deletion.");
            responseModel.Success = false;
            responseModel.Message = "Contact not found.";
            return NotFound(responseModel);
        }

        _addressBookEntries.Remove(contactToDelete);

        responseModel.Success = true;
        responseModel.Message = "Contact deleted successfully.";

        _logger.Info($"Deleted contact with ID {id} successfully.");
        return NoContent();
    }
}
