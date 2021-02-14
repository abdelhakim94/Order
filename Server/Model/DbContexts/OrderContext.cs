using System.Reflection;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Extensions;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Order.Server.Model
{
    public class OrderContext
        : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
        , IOrderContext
        , IConfigurationDbContext
        , IPersistedGrantDbContext
    {
        private readonly IOptions<OperationalStoreOptions> operationalStoreOptions;
        private readonly ConfigurationStoreOptions configurationStoreOptions;

        public OrderContext(
            DbContextOptions<OrderContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            ConfigurationStoreOptions configurationStoreOptions)
        : base(options)
        {
            this.operationalStoreOptions = operationalStoreOptions;
            this.configurationStoreOptions = configurationStoreOptions;
        }

        Task<int> IPersistedGrantDbContext.SaveChangesAsync() => base.SaveChangesAsync();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureClientContext(configurationStoreOptions);
            modelBuilder.ConfigureResourcesContext(configurationStoreOptions);

            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigurePersistedGrantContext(operationalStoreOptions.Value);

            modelBuilder
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
                .HasAnnotation("Relational:Collation", "English_United States.1252")
                .HasDefaultSchema("order_schema");

            // Override default table names of IdentityServer4 configuration store tables
            modelBuilder.Entity<ApiResource>(e => e.ToTable("is4_api_ressource", "order_schema"));
            modelBuilder.Entity<ApiScope>(e => e.ToTable("is4_api_scope", "order_schema"));
            modelBuilder.Entity<IdentityServer4.EntityFramework.Entities.Client>(e => e.ToTable("is4_client", "order_schema"));
            modelBuilder.Entity<IdentityResource>(e => e.ToTable("is4_identity_resource", "order_schema"));
            modelBuilder.Entity<ApiResourceClaim>(e => e.ToTable("is4_api_resource_claim", "order_schema"));
            modelBuilder.Entity<ApiResourceProperty>(e => e.ToTable("is4_api_resource_properties", "order_schema"));
            modelBuilder.Entity<ApiResourceScope>(e => e.ToTable("is4_api_resource_scope", "order_schema"));
            modelBuilder.Entity<ApiResourceSecret>(e => e.ToTable("is4_api_resource_secret", "order_schema"));
            modelBuilder.Entity<ApiScopeClaim>(e => e.ToTable("is4_api_scope_claim", "order_schema"));
            modelBuilder.Entity<ApiScopeProperty>(e => e.ToTable("is4_api_scope_property", "order_schema"));
            modelBuilder.Entity<ClientClaim>(e => e.ToTable("is4_client_claim", "order_schema"));
            modelBuilder.Entity<ClientCorsOrigin>(e => e.ToTable("is4_client_cors_origin", "order_schema"));
            modelBuilder.Entity<ClientGrantType>(e => e.ToTable("is4_client_grant_type", "order_schema"));
            modelBuilder.Entity<ClientIdPRestriction>(e => e.ToTable("is4_client_idp_restriction", "order_schema"));
            modelBuilder.Entity<ClientPostLogoutRedirectUri>(e => e.ToTable("is4_client_post_logout_redirection_uri", "order_schema"));
            modelBuilder.Entity<ClientProperty>(e => e.ToTable("is4_client_property", "order_schema"));
            modelBuilder.Entity<ClientRedirectUri>(e => e.ToTable("is4_client_redirect_uri", "order_schema"));
            modelBuilder.Entity<ClientScope>(e => e.ToTable("is4_client_scope", "order_schema"));
            modelBuilder.Entity<ClientSecret>(e => e.ToTable("is4_client_secret", "order_schema"));
            modelBuilder.Entity<IdentityResourceClaim>(e => e.ToTable("is4_identity_resource_claim", "order_schema"));
            modelBuilder.Entity<IdentityResourceProperty>(e => e.ToTable("is4_identity_resource_property", "order_schema"));

            // Override default table names of IdentityServer4 operation store tables
            modelBuilder.Entity<PersistedGrant>(e => e.ToTable("persisted_grant", "order_schema"));
            modelBuilder.Entity<DeviceFlowCodes>(e => e.ToTable("device_flow_code", "order_schema"));
        }

        // Operation store tables for IdentityServer4
        public DbSet<PersistedGrant> PersistedGrants { get; set; }
        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

        // Configuration store tables for IdentityServer4
        public DbSet<IdentityServer4.EntityFramework.Entities.Client> Clients { get; set; }
        public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
        public DbSet<IdentityResource> IdentityResources { get; set; }
        public DbSet<ApiResource> ApiResources { get; set; }
        public DbSet<ApiScope> ApiScopes { get; set; }
    }
}
