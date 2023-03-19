using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MixedLibrary.DataAccess
{
    public class ProtocolModel
    {
        [Key] // Autoincrement
        [Column(Order = 1)]
        public int Id { get; internal set; }

        [Required]
        [StringLength(30)]
        [Column(Order = 2)]
        public string Title { get; set; }

        [DefaultValue("")]
        [StringLength(100)]
        [Column(Order = 3)]
        public string Description { get; set; }


        public ProtocolModel(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}