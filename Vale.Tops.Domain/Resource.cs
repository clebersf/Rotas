namespace  Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Resource")]
    public partial class Resource : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Resource()
        {
            
        }
        [Key]
        public virtual Guid Id { get; set; }
        public virtual long? TypeId { get; set; }
        public virtual Type Type { get; set; }
        public DateTime dhInitial { get; set; }
        public DateTime dhCommunicated { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public bool Completed { get; set; }
        public bool Recused { get; set; }
        public string Response { get; set; }
        public string Requester { get; set; }
        public string Location { get; set; }
        public bool Finish { get; set; }
    }
}
