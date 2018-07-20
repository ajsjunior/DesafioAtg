using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace NucleoAtg.Dados
{
    [Table("Mensagem")]
    public class Mensagem
    {
        [Key]
        public int Id { get; set; }

        [Column("Side"), StringLength(4), Required(), IgnoreDataMember]
        public string SideString
        {
            get { return Side.ToString(); }
            private set { Side = value.ParseEnum<Lado>(); }
        }

        [NotMapped, Display(Name = "Lado"), Required(ErrorMessage = "O lado é obrigatório."), JsonConverter(typeof(StringEnumConverter))]
        public Lado Side { get; set; }

        [DataType(DataType.Currency), Display(Name = "Preço"), Required(ErrorMessage = "O preço é obrigatório."), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = false), UIHint("Currency")]
        public decimal Price { get; set; }

        [Display(Name = "Quantidade"), Required(ErrorMessage = "A quantidade é obrigatória.")]
        public int Quantity { get; set; }

        [Display(Name = "Símbolo"), StringLength(30)]
        public string Symbol { get; set; }
    }
}
