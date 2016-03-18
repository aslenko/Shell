using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell.Model.Entities
{
    public class Unit : Entity
    {        
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Unit()
        {            
        }
       
        public Unit(EntityContext entityContext)
            : base(entityContext)
        {
            
        }

        #endregion

        #region Nested Types

        public static class Constants
        {
            public const string XYZ = "XYZ";
        }

        #endregion

        #region Public Properties

        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                OnPropertyChanging(_id, value);
                _id = value;
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                OnPropertyChanging(_name, value);
                _name = value;
            }
        }

        private double _value;
        public double Value
        {
            get { return _value; }
            set
            {
                OnPropertyChanging(_value, value);
                _value = value;
            }
        }

        #endregion

    }
}
