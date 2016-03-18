using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shell.Model.Entities;

namespace Shell.Model.Managers
{
    public class UnitManager : EntityManager
    {
        #region Constructors
        
        public UnitManager(EntityContext entityContext)
            : base(entityContext)
        {
        }

        #endregion

        #region Public Methods

        public List<Unit> GetAll()
        {
            List<Unit> result = new List<Unit>();
            result.Add(new Unit() { Id= 1, Name = "XSmall", Value = 5.0 });
            result.Add(new Unit() { Id = 2, Name = "Small", Value = 10.0 });
            result.Add(new Unit() { Id = 3, Name = "Medium", Value = 20.0 });
            result.Add(new Unit() { Id = 4, Name = "Large", Value = 30.0 });
            result.Add(new Unit() { Id = 5, Name = "XLarge", Value = 40.0 });
            result.Add(new Unit() { Id = 6, Name = "XXLarge", Value = 50.0 });
            return result;
        }

        #endregion

    }
}
