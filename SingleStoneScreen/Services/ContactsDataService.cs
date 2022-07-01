using LiteDB;
using SingleStoneScreen.Interfaces;
using SingleStoneScreen.Models;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System;
using System.IO;
using Newtonsoft.Json.Serialization;

namespace SingleStoneScreen.Services
{
    public class ContactsDataService : IContactsDataService
    {
        private readonly LiteDatabase _liteDatabase;
        private readonly object _locker = new object();
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public ContactsDataService()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Contacts.db");
            _liteDatabase = new LiteDatabase(path);
            _jsonSerializerSettings = new JsonSerializerSettings();
            _jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

        }

        ~ContactsDataService()
        {
            _liteDatabase.Dispose();
        }

        public string GetContacts()
        {
            IEnumerable<Contact> contacts;
            lock (_locker)
            {
                contacts = _liteDatabase.GetCollection<Contact>("contacts").Query().ToList();
            }
            return JsonConvert.SerializeObject(contacts, _jsonSerializerSettings);
        }

        public string GetContact(int id)
        {
            Contact contact;
            lock (_locker)
            {
                contact = _liteDatabase
                    .GetCollection<Contact>("contacts")
                    .FindOne(contact => contact.Id == id);
                ;
            }
            return JsonConvert.SerializeObject(contact, _jsonSerializerSettings);
        }

        public int CreateContact(Contact contact)
        {
            int id;
            lock (_locker)
            {
                var bson = _liteDatabase.GetCollection<Contact>("contacts").Insert(contact);
                id = (int)bson;
            }
            return id;
        }

        public bool UpdateContact(int id, Contact contact)
        {
            bool updated;
            contact.Id = id;
            lock (_locker)
            {
                updated = _liteDatabase.GetCollection<Contact>("contacts").Update(contact);
            }
            return updated;
        }


        public bool DeleteContact(int id)
        {
            bool deleted;
            lock (_locker)
            {
                deleted = _liteDatabase.GetCollection<Contact>("contacts").Delete(id);
            }
            return deleted;
        }

        public string GetCallList()
        {
            List<Contact> contactList;
            lock (_locker)
            {
                contactList = _liteDatabase.GetCollection<Contact>("contacts")
                    .Query()
                    .Where(
                        contact =>
                            contact.Phone
                                .Select(phone => phone.Type)
                                .Any(phoneType => phoneType == "home")
                    )
                    .OrderBy(contact => contact.Name.Last + ", " + contact.Name.First)
                    .ToList()
                ;
            }
            var callList = contactList.Select(
                contact => new
                {
                    Name = contact.Name,
                    Phone = contact.Phone.First(phone => phone.Type == "home").Number
                }
            );
            return JsonConvert.SerializeObject(callList, _jsonSerializerSettings);
        }
    }
}
