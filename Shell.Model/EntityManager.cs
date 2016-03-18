using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell.Model
{
    public abstract class EntityManager
    {
        #region Constructors
        
        public EntityManager(EntityContext entityContext)
        {
            #region Assertions
            Debug.Assert(entityContext != null);
            #endregion

            _entityContext = entityContext;
        }

        #endregion

        #region Public Properties

        private readonly EntityContext _entityContext;
        public EntityContext EntityContext
        {
            get { return _entityContext; }
        }

        #endregion
    }
}
