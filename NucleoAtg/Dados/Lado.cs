using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NucleoAtg.Dados
{
    public enum Lado
    {
        [Display(Name = "Compra")]
        Buy,
        [Display(Name = "Venda")]
        Sell
    }
}
