using EvoGen.Domain.Collections;
using EvoGen.Domain.Interfaces.Repositories;
using EvoGen.Domain.Interfaces.Services;
using System;
using System.Linq;

namespace EvoGen.Domain.Services
{
    public class LogService : ServiceBase<Log>, ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
            : base(logRepository)
        {
            this._logRepository = logRepository;
        }

        public Log GetByNomenclature(string formula)
        {
            return _logRepository.GetAll().FirstOrDefault(x => x.Nomenclature == formula);
        }

        public int GetCounter(string formula)
        {
            var log = GetByNomenclature(formula);
            if (log != null)
                return log.SearchCounter;
            return 0;
        }

        public void NewSearch(string formula)
        {
            var log = GetByNomenclature(formula);
            if (log != null)
            {
                log.SearchCounter = log.SearchCounter + 1;
                _logRepository.Update(log);
            }
            else
            {
                _logRepository.Create(new Log()
                {
                    Nomenclature = formula,
                    SearchCounter = 1
                });
            }
        }
    }
}
