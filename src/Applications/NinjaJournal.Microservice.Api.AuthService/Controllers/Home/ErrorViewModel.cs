﻿using IdentityServer4.Models;

namespace NinjaJournal.Microservice.Api.AuthService.Controllers.Home
{
    public class ErrorViewModel
    {
        public ErrorViewModel()
        {
        }

        public ErrorViewModel(string error)
        {
            Error = new ErrorMessage { Error = error };
        }

        public ErrorMessage Error { get; set; }
    }
}