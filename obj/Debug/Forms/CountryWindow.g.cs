﻿#pragma checksum "..\..\..\Forms\CountryWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "B3B83D6CE685BD4D50E6941E645E8B21D819CA0D11D70F72EB5AF6253FEAA81B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using LandConquest.Forms;
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
    /// CountryWindow
    /// </summary>
    public partial class CountryWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\Forms\CountryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MainGrid;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\Forms\CountryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label CountryNameLbl;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\Forms\CountryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label RulerNameLbl;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\Forms\CountryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image RulerImg;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\Forms\CountryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CbAct;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\Forms\CountryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CbLandToTransfer;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\Forms\CountryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CbCountryToTransfer;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Forms\CountryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button IssueALaw;
        
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
            System.Uri resourceLocater = new System.Uri("/LandConquest;component/forms/countrywindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Forms\CountryWindow.xaml"
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
            
            #line 8 "..\..\..\Forms\CountryWindow.xaml"
            ((LandConquest.Forms.CountryWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.MainGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.CountryNameLbl = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.RulerNameLbl = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.RulerImg = ((System.Windows.Controls.Image)(target));
            return;
            case 6:
            this.CbAct = ((System.Windows.Controls.ComboBox)(target));
            
            #line 19 "..\..\..\Forms\CountryWindow.xaml"
            this.CbAct.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CbAct_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.CbLandToTransfer = ((System.Windows.Controls.ComboBox)(target));
            
            #line 23 "..\..\..\Forms\CountryWindow.xaml"
            this.CbLandToTransfer.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CbLandToTransfer_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.CbCountryToTransfer = ((System.Windows.Controls.ComboBox)(target));
            
            #line 24 "..\..\..\Forms\CountryWindow.xaml"
            this.CbCountryToTransfer.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CbCountryToTransfer_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.IssueALaw = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\..\Forms\CountryWindow.xaml"
            this.IssueALaw.Click += new System.Windows.RoutedEventHandler(this.IssueALaw_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

