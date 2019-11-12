using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebAPI.Dtos
{
    public class LoteDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage="O Nome deve ser preenchido.")]
        public string Nome { get; set; }

        [Required(ErrorMessage="O preço deve ser preenchido.")]
        public double Preco { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }

        [Range(1, 120000, ErrorMessage="Quantidade deve ser entre 1 e 120000.")]
        public int Quantidade { get; set; }
    }
}