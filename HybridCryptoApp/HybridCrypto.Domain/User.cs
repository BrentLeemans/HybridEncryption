using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HybridCrypto.Domain
{
    public class User : IdentityUser
    {
        public string Nickname { get; set; }

        public string URL { get; set; }

        public IEnumerable<Message> Messages { get; set; }

        [IgnoreDataMember]
        public override string UserName { get => base.UserName; set => base.UserName = value; }
        [IgnoreDataMember]
        public override string SecurityStamp { get => base.SecurityStamp; set => base.SecurityStamp = value; }
        [IgnoreDataMember]
        public override bool PhoneNumberConfirmed { get => base.PhoneNumberConfirmed; set => base.PhoneNumberConfirmed = value; }
        [IgnoreDataMember]
        public override bool TwoFactorEnabled { get => base.TwoFactorEnabled; set => base.TwoFactorEnabled = value; }
        [IgnoreDataMember]
        public override int AccessFailedCount { get => base.AccessFailedCount; set => base.AccessFailedCount = value; }
        [IgnoreDataMember]
        public override string PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }
        [IgnoreDataMember]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }
        [IgnoreDataMember]
        public override string ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }
        [IgnoreDataMember]
        public override bool EmailConfirmed { get => base.EmailConfirmed; set => base.EmailConfirmed = value; }
        [IgnoreDataMember]
        public override bool LockoutEnabled { get => base.LockoutEnabled; set => base.LockoutEnabled = value; }
        [IgnoreDataMember]
        public override string NormalizedEmail { get => base.NormalizedEmail; set => base.NormalizedEmail = value; }
        [IgnoreDataMember]
        public override DateTimeOffset? LockoutEnd { get => base.LockoutEnd; set => base.LockoutEnd = value; }
        [IgnoreDataMember]
        public override string NormalizedUserName { get => base.NormalizedUserName; set => base.NormalizedUserName = value; }

        public string Key { get; set; }

        public Guid Guid { get; set; }

    }
}
