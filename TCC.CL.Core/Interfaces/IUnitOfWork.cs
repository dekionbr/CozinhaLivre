using System;
using System.Collections.Generic;
using System.Linq;

namespace TCC.CL.Core.Infraestrutura
{
    public interface IUnitOfWork : IDisposable
    {

        void Commit();
        void Rollback();
    }
}
