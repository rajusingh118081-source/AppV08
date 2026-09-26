using App.Common.GenericResponse;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.Application.DTOs.QuickBookOnlineDTO
{
    public class QuickBooksTokenDTO: ViewBaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string RealmId { get; set; } = "";

        [Required]
        public string AccessToken { get; set; } = "";

        [Required]
        public string RefreshToken { get; set; } = "";

        public DateTime AccessTokenExpiresAt { get; set; }

        public DateTime RefreshTokenExpiresAt { get; set; }

        [MaxLength(50)]
        public string TokenType { get; set; } = "Bearer";
    }
}
