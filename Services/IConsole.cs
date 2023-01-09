using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutofacTutorial.Services
{
    public interface IConsole
    {
        void Create();
        void Remove();
        void Update();
        void Get();
        void  GetAll();

    }
}
