using SingleStoneScreen.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SingleStoneScreen.Interfaces {
    public interface IContactsDataService
    {
        public string GetContacts();
        public string GetContact(int id);
        public int CreateContact(Contact contact);
        public bool UpdateContact(int id, Contact contact);
        public bool DeleteContact(int id);
        public string GetCallList();
    }
}
