using System.Collections.Generic;
using System.Threading.Tasks;
using EvoGen.Repository.Collection;
using EvoGen.Repository.Factory;
using EvoGen.Repository.Interfaces.Service;
using EvoGen.Repository.Repositories;

namespace EvoGen.Repository.Service
{
    public class ElementService : ServiceBase<Element>, IElementService
    {
        private static readonly ElementRepository _elementRepository = new ElementRepository();
        private static readonly ElementFactory _elementFactory = new ElementFactory();
        
        public ElementService() : base(_elementRepository, _elementFactory)
        {
            
        }
    }
}
