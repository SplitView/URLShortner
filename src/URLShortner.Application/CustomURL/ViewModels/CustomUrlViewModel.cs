﻿using Microsoft.AspNetCore.Http;

using System;


namespace URLShortner.Application.CustomURL.ViewModels
{
    public class CustomUrlViewModel
    {
        public string Id { get; set; }
        public string OriginalURL { get; set; }
        public string UniqueKey { get; set; }
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// Url that redirects to the original url
        /// </summary>
        public string RedirectURL { get; set; }

        public static CustomUrlViewModel FromModel(Domain.Entities.CustomURL customURL, IHttpContextAccessor httpContextAccessor)
        {
            return new CustomUrlViewModel
            {
                Id = customURL.Id,
                ExpiryDate = customURL.ExpiryDate,
                UniqueKey = customURL.UniqueKey,
                OriginalURL = customURL.OriginalURL,
                RedirectURL = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}/{customURL.UniqueKey}"
            };
        }
    }
}
