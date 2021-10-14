﻿#pragma checksum "..\..\..\Forms\MessengerWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "DF9465A6D707790BB939B711D02E72418C62822488E37AA292395FA6ECB4598B"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using LandConquest.Forms;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace LandConquest.Forms {
    
    
    /// <summary>
    /// MessengerWindow
    /// </summary>
    public partial class MessengerWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\..\Forms\MessengerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ListViewDialogs;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\Forms\MessengerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextBoxDialog;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Forms\MessengerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonSendMessage;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\Forms\MessengerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonClose;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\Forms\MessengerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextBoxNewMessage;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\Forms\MessengerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonCreateNewDialog;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\Forms\MessengerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonDeleteDialog;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\Forms\MessengerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonRefreshDialog;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/LandConquest;component/forms/messengerwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Forms\MessengerWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ListViewDialogs = ((System.Windows.Controls.ListView)(target));
            
            #line 16 "..\..\..\Forms\MessengerWindow.xaml"
            this.ListViewDialogs.Loaded += new System.Windows.RoutedEventHandler(this.ListViewDialogs_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TextBoxDialog = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.ButtonSendMessage = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\..\Forms\MessengerWindow.xaml"
            this.ButtonSendMessage.Click += new System.Windows.RoutedEventHandler(this.ButtonSendMessage_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ButtonClose = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\Forms\MessengerWindow.xaml"
            this.ButtonClose.Click += new System.Windows.RoutedEventHandler(this.ButtonClose_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.TextBoxNewMessage = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.ButtonCreateNewDialog = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\..\Forms\MessengerWindow.xaml"
            this.ButtonCreateNewDialog.Click += new System.Windows.RoutedEventHandler(this.ButtonCreateNewDialog_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.ButtonDeleteDialog = ((System.Windows.Controls.Button)(target));
            return;
            case 8:
            this.ButtonRefreshDialog = ((System.Windows.Controls.Button)(target));
            
            #line 49 "..\..\..\Forms\MessengerWindow.xaml"
            this.ButtonRefreshDialog.Click += new System.Windows.RoutedEventHandler(this.ButtonRefreshDialog_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

