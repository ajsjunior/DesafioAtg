using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NucleoAtg.Dados
{
    [Table("Mensagem")]
    public class Mensagem
    {
        public int Id { get; set; }

        [Display(Name = "Lado"), Required(ErrorMessage = "O lado é obrigatório."), StringLength(30)]
        public string Side { get; set; }

        [DataType(DataType.Currency), Display(Name = "Preço"), Required(ErrorMessage = "O preço é obrigatório."), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = false), UIHint("Currency")]
        public decimal Price { get; set; }

        [Display(Name = "Quantidade"), Required(ErrorMessage = "A quantidade é obrigatória.")]
        public int Quantity { get; set; }

        [Display(Name = "Símbolo"), StringLength(30)]
        public string Symbol { get; set; }
    }
}
