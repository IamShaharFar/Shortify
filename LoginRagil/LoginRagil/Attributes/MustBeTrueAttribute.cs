using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ControllerContext = Microsoft.AspNetCore.Mvc.ControllerContext;
using ModelMetadata = Microsoft.AspNetCore.Mvc.ModelBinding.ModelMetadata;

namespace LoginRagil.Attributes
{
    public class MustBeTrueAttribute : ValidationAttribute, IClientValidatable
    {
        public override bool IsValid(object value)
        {
            return value is bool && (bool)value;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be checked.";
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ValidationType = "mustbetrue",
                ErrorMessage = FormatErrorMessage(metadata.DisplayName)
            };
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(System.Web.Mvc.ModelMetadata metadata, System.Web.Mvc.ControllerContext context)
        {
            throw new NotImplementedException();
        }
    }

}
