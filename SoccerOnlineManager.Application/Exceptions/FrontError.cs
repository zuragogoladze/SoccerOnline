using System.Runtime.Serialization;

namespace SoccerOnlineManager.Application.Exceptions
{
    public class FrontError
    {
        public FrontError(int statusCode, string error, object data)
        {
            StatusCode = statusCode;
            Error = error;
            ValidationResult = data;
        }

        /// <summary>
        /// Status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// შეცდომის ტექსტი რომელიც შეგიძლიათ გამოაჩინოთ კლიენტის მხარეს
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// ვალიდაციის შეცდომა
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public object ValidationResult { get; set; }
    }
}
