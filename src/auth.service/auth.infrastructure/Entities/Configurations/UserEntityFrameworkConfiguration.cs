using auth.infrastructure.entites;
using Microsoft.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace auth.infrastructure.entites.configurations; 
public class UserEntityFrameworkConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users"); 

        builder.HasKey(u => u.Id);

        builder.Property(u => u.CreateAt)
            .IsRequired();

        builder.Property(u => u.UpdateAt)
            .IsRequired(); 

        builder.Property(u => u.Email)
            .HasMaxLength(255)
            .IsRequired();  

        builder.Property(u => u.Name) 
            .IsRequired(); 

        builder.Property(u => u.PasswordHash) 
            .IsRequired(); 

        builder.Property(u => u.UserRole) 
            .IsRequired(); 

        builder.Property(u => u.Active) 
            .HasDefaultValue(true)
            .IsRequired(); 
    }
}