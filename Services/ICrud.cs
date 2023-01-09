using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutofacTutorial
{
    public interface ICrud<T>
    {
        public bool Create(T entity);
        public bool Update(T entity);
        public bool Delete(int id); 

        public T Get(int id);

        public ICollection<T> GetAll();

    }
}
