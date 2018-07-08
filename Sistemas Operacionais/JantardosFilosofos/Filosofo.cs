using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JantardosFilosofos
{
    public class Filosofo
    {
        bool estaPensando;

        public bool EstaPensando
        {
            get { return estaPensando; }
            set { estaPensando = value; }
        }

        public string Texto
        {
            get
            {
                return texto;
            }

            set
            {
                texto = value;
            }
        }

        string texto;
    }
}
