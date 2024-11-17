using AvtoService_3cursAA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvtoService_3cursAA
{
    internal class DataBase
    {
        private static readonly Avtoservice3cursAaContext instance = new Avtoservice3cursAaContext();
        public static Avtoservice3cursAaContext Instance => instance;
    }
}
