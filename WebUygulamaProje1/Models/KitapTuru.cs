using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebUygulamaProje1.Models
{
    public class KitapTuru
    {
        [Key]  // primary key  ( "Id" yazılması yeterli -> [Key] yazılmış varsayılıyor)
        public int Id { get; set; }
        [Required(ErrorMessage = "Kitap Türü Adı boş bırakılamaz")]  // not null
        [MaxLength(25)]
        [DisplayName("Kitap Türü Adı")]
        public string Ad { get; set; }

    }
}
