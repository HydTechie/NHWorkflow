
using NHWorkflow.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;

namespace NHWorkflow.Data
{
    public class NHWorkflowContext : DbContext, IDbContext
    {
        public NHWorkflowContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(NHWorkflowEntityTypeConfiguration<>));

            foreach(var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            //...or do it manually below. For example,
            //modelBuilder.Configurations.Add(new LanguageMap());

            base.OnModelCreating(modelBuilder);
        }

        protected override bool ShouldValidateEntity(DbEntityEntry entityEntry)
        {
            return base.ShouldValidateEntity(entityEntry);
        }
        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            return base.ValidateEntity(entityEntry, items);
        }

        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                Set<TEntity>().Attach(entity);
                return entity;
            }

            return alreadyAttached;
        }

        public bool ProxyCreationEnabled {

            get
            {
                return Configuration.ProxyCreationEnabled;
            }
            set
            {
                Configuration.ProxyCreationEnabled = value; 
            }
        }

        public bool AutoDetectChangesEnabled {
            get
            {
                return Configuration.AutoDetectChangesEnabled;
            }
            set
            {
                Configuration.AutoDetectChangesEnabled = value;
            }
        }

        public void Detach(object entity)
        {
            if (entity == null)
                 throw new ArgumentNullException(nameof(entity));
            ((IObjectContextAdapter)this).ObjectContext.Detach(entity);
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //store previous timeout
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            var transactionalBehavior = doNotEnsureTransaction
                ? TransactionalBehavior.DoNotEnsureTransaction
                : TransactionalBehavior.EnsureTransaction;
            var result = Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);

            if (timeout.HasValue)
            {
                //Set previous timeout back
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }

            //return result
            return result;
        }

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            //add parameters to command
            if (parameters != null && parameters.Length > 0)
            {
                for (var i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //output parameter
                        commandText += " output";
                    }
                }
            }

            var result = Database.SqlQuery<TEntity>(commandText, parameters).ToList();

            //performance hack applied as described here - https://www.nopcommerce.com/boards/t/25483/fix-very-important-speed-improvement.aspx
            var acd = Configuration.AutoDetectChangesEnabled;
            try
            {
                Configuration.AutoDetectChangesEnabled = false;

                for (var i = 0; i < result.Count; i++)
                    result[i] = AttachEntityToContext(result[i]);
            }
            finally
            {
                Configuration.AutoDetectChangesEnabled = acd;
            }

            return result;
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return Database.SqlQuery<TElement>(sql, parameters);
        }

        IDbSet<TEntity> IDbContext.Set<TEntity>()
        {
            throw new NotImplementedException();
        }
    }

    public interface IDbContext
    {

        IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;
        int SaveChanges();
        IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters)
            where TEntity : BaseEntity, new();

        IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters);

        int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction=false, int? timeout = null, params object[] parameters);

        void Detach(object Entity);

        bool ProxyCreationEnabled { get; set; }
        bool AutoDetectChangesEnabled { get; set; }
     }

    public abstract class NHWorkflowEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : class
    {
        protected NHWorkflowEntityTypeConfiguration()
        {
            PostInitialize();
        } 
        // override in partial custom classes 
        protected virtual void PostInitialize()
        {

        }
    }

    public partial class MemberMap : NHWorkflowEntityTypeConfiguration<MemberEntity>
    {
        public MemberMap()
        {
            this.ToTable("Member");
            this.HasKey(x => x.MemberId);

            this.Property(x => x.FirstName).HasMaxLength(50).IsRequired();
            this.Property(x => x.LastName).HasMaxLength(50).IsRequired();
            this.Property(x => x.DOB).IsRequired();
        }
    }

    public partial class LoginMap : NHWorkflowEntityTypeConfiguration<Login>
    {
        public LoginMap()
        {
            this.ToTable("Login");
            this.HasKey(x => x.LoginId);

            //this.Property(x => x.Username).HasMaxLength(50).IsRequired();
            //this.Property(x => x.PasswordHash).HasMaxLength(50).IsRequired();
        }
    }
    public partial class BenefitCategoryMap : NHWorkflowEntityTypeConfiguration<BenefitCategory>
    {
        public BenefitCategoryMap()
        {
            this.ToTable("BenefitCategory");
            this.HasKey(x => x.CategoryId);
                // A, B, C... etc
            this.Property(x => x.CategoryType).HasMaxLength(1);
        }
    }

    public partial class ApprovalQueueMap : NHWorkflowEntityTypeConfiguration<ApprovalQueue>
    {
        public ApprovalQueueMap()
        {
            this.ToTable("ApprovalQueue");
            this.HasKey(x => x.ApprovalId);
            // A, B, C... etc
            this.Property(x => x.ApprovalStatus).;
        }
    }



}
