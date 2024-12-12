using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace TestCommunityToolkit._2_Observable
{
    public partial class CommentVO : ObservableValidator
    {
        [ObservableProperty]
        [NotifyDataErrorInfo ]
        [Required]
        [MinLength(2)]
        [MaxLength(8)]
        private string _author;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [MinLength(1)]
        [EmailAddress]
        private string _email;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [MinLength(1)]
        [CustomValidation(typeof(CommentVO), nameof(ValidateText))]
        private string _text;
        
        [RelayCommand]
        private void SubmitComment()
        {
            this.ValidateAllProperties();
            if (this.HasErrors)
            {
                string errors = string.Join(Environment.NewLine, this.GetErrors().Select(vr => vr.ErrorMessage));
                MessageBox.Show(errors, "Validation errors", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CommentMessage commentMessage = WeakReferenceMessenger.Default.Send(new CommentMessage(this));
            bool success = commentMessage.Response.success;
            if (!success)
            {
                MessageBox.Show(commentMessage.Response.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Window? window = Application.Current.Windows?.Cast<Window>()?.FirstOrDefault(w => w.DataContext == this);
            if (window is not null)
            {
                window.DialogResult = true;
                window.Close();
            }
        }

        public static ValidationResult ValidateText(string value, ValidationContext context)
        {
            if (value?.Contains("error") == true)
            {
                return new ValidationResult("Text cannot contain the word 'error'");
            }
            return ValidationResult.Success;
        }
    }
}
