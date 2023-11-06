using Microsoft.AspNetCore.Http;
using ProductCatalog.Domain.Models.Templates;
using ProductCatalog.Utility.Helpers;
using ProductCatalog.Utility.Proxies;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalog.Domain.Models.Authentication
{
    [Table("user")]
    public class User : BaseEntity
    {
        [Required, Column("login", TypeName = "NVARCHAR(100)")]
        public string encryptedLogin { get; set; } = null!;

        [NotMapped]
        public string Login
        {
            get { return encryptedLogin == null ? default : ProxyUtitlity<EncryptionUtility>.GetUtility().Decrypt(encryptedLogin); }
            set { encryptedLogin = ProxyUtitlity<EncryptionUtility>.GetUtility().Encrypt(value); }
        }

        [Required, Column("password", TypeName = "NVARCHAR(100)")]
        public string encryptedPassword { get; set; } = null!;

        [NotMapped]
        public string Password
        {
            get { return encryptedPassword == null ? default : ProxyUtitlity<EncryptionUtility>.GetUtility().Decrypt(encryptedPassword); }
            set { encryptedPassword = ProxyUtitlity<EncryptionUtility>.GetUtility().Encrypt(value); }
        }

        [Column("isLocked", TypeName = "BIT")]
        public bool IsLocked { get; set; }
        [Required, ForeignKey("roleId")]
        public virtual UserRole Role { get; set; } = null!;
        [NotMapped]
        public Guid? RoleId { get; set; }
    }
}
