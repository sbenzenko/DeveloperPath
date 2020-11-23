using DeveloperPath.Application.Common.Interfaces;
using System;

namespace DeveloperPath.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
