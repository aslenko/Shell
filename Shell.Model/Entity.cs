using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Shell.Model
{
    public abstract class Entity
    {
        private readonly Guid _objectId = Guid.NewGuid();

        #region Constructors

        public Entity()
        {
           
        }

        public Entity(EntityContext entityContext)
        {
            #region Assertions
            Debug.Assert(entityContext != null);
            #endregion

            //
            // Entities must always have a key, even before the database has assigned the value.  
            // As such, a surrogate key is used until the entity has been persisted.
            // Assign private field to avoid raising the assign key event.
            //            
            _entityContext = entityContext;
        }

        #endregion

        #region Public Properties

        private EntityContext _entityContext;
        public EntityContext EntityContext
        {
            get { return _entityContext; }
            internal set
            {
                if (_entityContext == null)
                {
                    _entityContext = value;
                }
                else if (_entityContext != value)
                {
                    throw new InvalidOperationException("An entity context has already been assigned to the current instance.");
                }
            }
        }

        #endregion

        #region Protected Methods

        protected void OnPropertyChanging(object oldValue, object newValue, [CallerMemberName] string propertyName = "")
        {

        }

        #endregion
    }
}
