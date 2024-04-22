using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace SswApplication.Components.Pages
{
    public partial class CallJavaScriptInDotNet
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        public async Task ShowAlertWindow()
        {
            await JSRuntime.InvokeVoidAsync("draw");
        }
    }
}
