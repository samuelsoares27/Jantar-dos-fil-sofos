using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JantardosFilosofos
{
    public class Garfo
    {
        bool estaEmUso;

        public bool EstaEmUso
        {
            get
            {
                return estaEmUso;
            }

            set
            {
                estaEmUso = value;
            }
        }
    }
}
