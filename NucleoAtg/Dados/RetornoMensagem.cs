using System;
using System.Collections.Generic;
using System.Text;

namespace NucleoAtg.Dados
{
    public class RetornoMensagem
    {
        public int? Id { get; set; }

        public bool? Status { get; set; }

        public string[] Msgs { get; set; }
    }
}
