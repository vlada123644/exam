using System;
using System.Threading.Tasks;
using System.Windows;
using TradeAutomation.ViewModels;

namespace TradeAutomation.Services
{
    public interface IDialogService
    {
        Task<bool> ShowDialogAsync(ViewModelBase viewModel);
        Task ShowMessageAsync(string title, string message);
        Task<bool> ShowConfirmationAsync(string title, string message);
    }

    public class DialogService : IDialogService
    {
        public async Task<bool> ShowDialogAsync(ViewModelBase viewModel)
        {
            var tcs = new TaskCompletionSource<bool>();

            if (viewModel is ProductEditViewModel editVM)
            {
                editVM.DialogClosed += (s, result) =>
                {
                    tcs.SetResult(result);
                };
            }

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                // ВРЕМЕННО: без MaterialDesignThemes
                MessageBox.Show("Диалог временно отключен", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                tcs.SetResult(true);
            });

            return await tcs.Task;
        }

        public async Task ShowMessageAsync(string title, string message)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        public async Task<bool> ShowConfirmationAsync(string title, string message)
        {
            var tcs = new TaskCompletionSource<bool>();

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var result = MessageBox.Show(message, title,
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                tcs.SetResult(result == MessageBoxResult.Yes);
            });

            return await tcs.Task;
        }
    }
} 