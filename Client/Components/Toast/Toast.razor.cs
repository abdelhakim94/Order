using System;
using System.Timers;
using Microsoft.AspNetCore.Components;

namespace Order.Client.Components
{
    public partial class Toast : ComponentBase, IDisposable
    {
        private bool show { get; set; }
        private string shouldShow { get => show ? "show" : null; }
        private string content { get; set; }

        private bool isError { get; set; }
        private string getIcon { get => isError ? "icons/ko.png" : "icons/ok.png"; }
        private string messageType { get => isError ? "error" : "success"; }

        private Timer timer { get; set; }

        protected override void OnInitialized()
        {
            timer = new Timer(3000);
            timer.Elapsed += OnTimedEvent;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Close();
        }

        private void ResetTimer()
        {
            timer.Stop();
            timer.Start();
        }

        public void ShowSuccess(string content)
        {
            show = true;
            isError = false;
            this.content = content;
            StateHasChanged();
            ResetTimer();
        }

        public void ShowError(string content)
        {
            show = true;
            isError = true;
            this.content = content;
            StateHasChanged();
            ResetTimer();
        }

        public void Close()
        {
            show = false;
            StateHasChanged();
        }

        void IDisposable.Dispose()
        {
            timer.Dispose();
        }
    }
}
