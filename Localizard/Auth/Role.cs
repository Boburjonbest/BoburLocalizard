﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Localizard.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
