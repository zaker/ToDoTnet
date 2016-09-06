using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoTnet.DataEntities;
using ToDoTnet.Logic;

namespace ToDoTnet.Models
{
    public class ToDoTask : IAttachDbEntity<ToDo>
    {
        private ToDo _dbEnt;

        public ToDoTask()
        {
            _dbEnt = null;
        }
        public ToDoTask(ToDo dbEnt)
        {
            _dbEnt = dbEnt;
        }
        public void AttachDbEntity(ToDo ent)
        {



            if (_dbEnt != null)
                return;

            _dbEnt = ent;
        }


        public string DoerName
        {
            get
            {
                using (var db = new ToDoContext()) // use your DbConext
                {
                    // Fetch - again - your user from the DB with the Id.
                    return (from u in db.Users
                            where u.UserID == _dbEnt.UserID
                            select u).FirstOrDefault()?.Name;
                }

            }


        }


        public string Title
        {
            set
            {
                _dbEnt.Title = value;
            }
            get
            {
                return _dbEnt.Title;
            }
        }

        public string Id
        {
            get
            {
                return _dbEnt.ToDoID.ToString();
            }
        }

        public string Description
        {
            set
            {
                _dbEnt.Description = value;
            }
            get
            {
                return _dbEnt.Description;
            }
        }

        public string Product
        {
            set
            {
                _dbEnt.Product = value;
            }
            get
            {
                return _dbEnt.Product;
            }
        }

        public string Type
        {
            set
            {
                _dbEnt.Type = value;
            }
            get
            {
                return _dbEnt.Type;
            }
        }

        public DateTime? DoneDate
        {
            set
            {
                _dbEnt.DoneDate = value;
            }
            get
            {
                return _dbEnt.DoneDate;
            }
        }

        public bool IsDone
        {
            get
            {
                return _dbEnt.DoneDate.HasValue;
            }
        }
    }
}
