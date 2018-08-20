using EvoGen.Domain.Collections;
using System;

namespace EvoGen.Domain.Interfaces.Services
{
    public interface ILogService : IServiceBase<Log>
    {
        Log GetByNomenclature(string formula);
        int GetCounter(string formula);
        void NewSearch(string formula);
    }
}
