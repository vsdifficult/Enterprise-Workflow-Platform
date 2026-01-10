using Microsoft.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace users.infastructure.entities.configurations; 

public class TaskEntityConfiguration: IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.ToTable("tasks"); 

        builder.HasKey(t => t.Id); 

        builder.Property(t => t.UserId)
            .IsRequired(); 

        builder.Property(t => t.CreateAt) 
            .IsRequired(); 

        builder.Property(t => t.UpdateAt)
            .IsRequired(); 

        builder.Property(t => t.Name)
            .IsRequired(); 

        builder.Property(t => t.Description); 

        builder.Property(t => t.Status) 
            .IsRequired(); 
    }
}