using System.Collections.Generic;
using System.Linq;
using Bloghost.Core.Entities;
using Bloghost.Core.Repositories;
using Bloghost.Core.Services.Interfaces;

namespace Bloghost.Core.Services
{
    public class EntryService : IEntryService
    {
        private readonly IEntryRepository _entryRepository;

        public EntryService(IEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
        }

        [UnitOfWork]
        public IEnumerable<Entry> GetEntryList()
        {
            return _entryRepository.GetAll().OrderBy(x => x.CreatedDate).ToList();
        }

        [UnitOfWork]
        public void CreateEntry(Entry entry)
        {
            _entryRepository.Create(entry);
        }

        [UnitOfWork]
        public void UpdateEntry(Entry entry)
        {
            _entryRepository.Update(entry);
        }

        [UnitOfWork]
        public void DeleteEntry(System.Guid id)
        {
            _entryRepository.Delete(id);
        }

        [UnitOfWork]
        public Entry GetEntry(System.Guid id)
        {
            return _entryRepository.Get(id);
        }
    }
}
