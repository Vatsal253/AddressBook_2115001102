using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ModalLayer.Modal;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AddressBookController : ControllerBase
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly IAddressBookBL _addressBookBL;

    public AddressBookController(IAddressBookBL addressBookBL)
    {
        _addressBookBL = addressBookBL;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllContacts()
    {
        try
        {
            var contacts = await _addressBookBL.GetAllContactsAsync();
            return Ok(new ResponseModel<IEnumerable<AddressBookEntryModel>>
            {
                Success = true,
                Message = "Contacts fetched successfully.",
                Data = contacts
            });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error fetching contacts");
            return StatusCode(500, new ResponseModel<string>
            {
                Success = false,
                Message = "An error occurred while fetching contacts."
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContactById(int id)
    {
        try
        {
            var contact = await _addressBookBL.GetContactByIdAsync(id);
            if (contact == null)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Contact not found."
                });
            }

            return Ok(new ResponseModel<AddressBookEntryModel>
            {
                Success = true,
                Message = "Contact fetched successfully.",
                Data = contact
            });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error fetching contact with ID: {id}");
            return StatusCode(500, new ResponseModel<string>
            {
                Success = false,
                Message = "An error occurred while fetching the contact."
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddContact([FromBody] RequestModel request)
    {
        try
        {
            var newContact = await _addressBookBL.AddContactAsync(request);
            return CreatedAtAction(nameof(GetContactById), new { id = newContact.Id }, new ResponseModel<AddressBookEntryModel>
            {
                Success = true,
                Message = "Contact added successfully.",
                Data = newContact
            });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error adding contact");
            return StatusCode(500, new ResponseModel<string>
            {
                Success = false,
                Message = "An error occurred while adding the contact."
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(int id, [FromBody] RequestModel request)
    {
        try
        {
            var updatedContact = await _addressBookBL.UpdateContactAsync(id, request);
            if (updatedContact == null)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Contact not found."
                });
            }

            return Ok(new ResponseModel<AddressBookEntryModel>
            {
                Success = true,
                Message = "Contact updated successfully.",
                Data = updatedContact
            });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error updating contact with ID: {id}");
            return StatusCode(500, new ResponseModel<string>
            {
                Success = false,
                Message = "An error occurred while updating the contact."
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        try
        {
            var deleted = await _addressBookBL.DeleteContactAsync(id);
            if (!deleted)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Contact not found."
                });
            }

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Contact deleted successfully."
            });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error deleting contact with ID: {id}");
            return StatusCode(500, new ResponseModel<string>
            {
                Success = false,
                Message = "An error occurred while deleting the contact."
            });
        }
    }
}
