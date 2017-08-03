using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TCC.CL.Core.Entities;

namespace TCC.CL.Core.Business
{
    public static class AccessoBussiness
    {
        public static bool Add(Acesso access)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    access = Session.Current.Merge<Acesso>(access);
                    Session.Current.Save(access);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
       
    }
}
