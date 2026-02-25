namespace Vale.Tops.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.CompilerServices;

    [Table("Ore")]
    public partial class Ore : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        public Ore()
        {

        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public virtual long Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Family { get; set; }
        public string Process { get; set; }
        public DateTime dh { get; set; }
        public float Humidity { get; set; }
        public float DensityAvg { get; set; }
        public float DensityLoose { get; set; }
        public float DensityCompacted { get; set; }
        public float Angle { get; set; }
       
    }
}
