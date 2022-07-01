using Microsoft.AspNetCore.Mvc;
using SingleStoneScreen.Interfaces;
using SingleStoneScreen.Models;

namespace SingleStoneScreen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactsDataService _contactsDataService;
        public ContactsController(IContactsDataService contactsDataService)
        {
            _contactsDataService = contactsDataService;
        }

        [HttpGet]
        public string GetContacts()
        {
            return _contactsDataService.GetContacts();
        }

        [HttpPost]
        public int CreateContact([FromBody] Contact contact)
        {
            return _contactsDataService.CreateContact(contact);
        }

        [HttpGet]
        [Route("{id}")]
        public string GetContact([FromRoute] int id)
        {
            return _contactsDataService.GetContact(id);
        }

        [HttpPut]
        [Route("{id}")]
        public bool UpdateContact([FromRoute] int id, [FromBody] Contact contact)
        {
            return _contactsDataService.UpdateContact(id, contact);
        }

        [HttpDelete]
        [Route("{id}")]
        public bool DeleteContact([FromRoute] int id)
        {
            return _contactsDataService.DeleteContact(id);
        }

        [HttpGet]
        [Route("call-list")]
        public string GetCallList()
        {
            return _contactsDataService.GetCallList();
        }

    }
}
