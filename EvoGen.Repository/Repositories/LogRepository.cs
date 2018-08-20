using EvoGen.Domain.Collections;
using EvoGen.Domain.Interfaces.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EvoGen.Repository.Repositories
{
    public class LogRepository : RepositoryBase<Log>, ILogRepository
    {

    }
}
