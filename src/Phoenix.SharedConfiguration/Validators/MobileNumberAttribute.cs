using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Phoenix.SharedConfiguration.Validators
{
    public class MobileNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object mobileNumberObject)
        {
            if (mobileNumberObject == null) return true;
            var mobileNumber = mobileNumberObject.ToString().TrimStart('0');
            var number = $"0{mobileNumber}";
            var pattern = new Regex(@"^(09)([0-9]{9})$");
            return pattern.IsMatch(number);
        }

    }
}
