using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pc_system.Model
{
    class Worker
    {
        public static bool validate(string id, string password)
        {
            using (var db = new dbEntities())
            {
                int wId = -1;
                if (!Int32.TryParse(id, out wId))
                {
                    return false;
                }
                var result = (from c in db.workers where c.worker_id == wId && c.worker_password.Equals(password) select c);
                if (result.Count() == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
