using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation.Results;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace AuthMaster.Application.Helpers
{
    public class ValidationFormat
    {
        public static object FormatErrors(ValidationResult result)
        {
            return result.Errors
                .GroupBy(e => JsonNamingPolicy.CamelCase.ConvertName(e.PropertyName))
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
        }
    }
}
