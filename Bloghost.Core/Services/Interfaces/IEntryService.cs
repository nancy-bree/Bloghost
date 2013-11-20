using System.Collections.Generic;
using Bloghost.Core.Entities;

namespace Bloghost.Core.Services.Interfaces
{
    public interface IEntryService
    {
        IEnumerable<Entry> GetEntryList();

        Entry GetEntry(System.Guid id);

        void CreateEntry(Entry entry);

        void UpdateEntry(Entry entry);

        void DeleteEntry(System.Guid id);
    }
}
