namespace Dynamic.EF
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class Entity
    {
        public Entity()
        {
            CreatedOn = DateTimeOffset.Now;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
    }
}
