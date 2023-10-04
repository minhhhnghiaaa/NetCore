using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NetCore.Domain.Entities;

namespace NetCore.DataServices.Data;

public class AppDbContext : DbContext
{
    // define the db entities
    public virtual DbSet<Member> Members { get; set; }
    public virtual DbSet<Merchant> Merchants { get; set; }
    public virtual DbSet<User> Users { get; set; }
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // specify the relationships between db entities
        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasOne(d => d.Merchant)
                .WithMany(p => p.Members)
                .HasForeignKey(d => d.MerchantId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Members_Merchant");
        });
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // var httpContext = _httpContextAccessor.HttpContext;
        // var accessToken = httpContext.GetTokenAsync("access_token").Result;
    
        var entities = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified))
            .ToList();
    
        foreach (var entityEntry in entities)
        {
            if (entityEntry.Entity is BaseEntity baseEntity)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    baseEntity.CreatedDate = DateTime.UtcNow;
                    // baseEntity.CreateBy = Token.GetCurrentUserId(accessToken);
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    baseEntity.UpdatedDate = DateTime.UtcNow;
                    // baseEntity.UpdateBy = Token.GetCurrentUserId(accessToken);
                }
            }
        }
    
        return await base.SaveChangesAsync(cancellationToken);
    }
}