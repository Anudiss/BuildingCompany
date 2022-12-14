using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace BuildingCompany.Components
{
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_IncreaseButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_DecreaseButton", Type = typeof(RepeatButton))]
    public class NumericalBox : Control
    {
        protected TextInput TextBox;
        protected RepeatButton IncreaseButton;
        protected RepeatButton DecreaseButton;

        public bool HasError => TextBox.HasError;

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(NumericalBox));



        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, Math.Min(UpperSide, Math.Max(LowerSide, value))); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(NumericalBox), new PropertyMetadata(0, OnValueChanged, CoerceValue));

        public static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs value)
        {
            ((NumericalBox)obj).TextBox.Text = $"{value.NewValue}";
        }

        public static object CoerceValue(DependencyObject obj, Object baseValue)
        {
            
            return (int)baseValue;
        }

        public int LowerSide
        {
            get { return (int)GetValue(LowerSideProperty); }
            set { SetValue(LowerSideProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LowerSide.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LowerSideProperty =
            DependencyProperty.Register("LowerSide", typeof(int), typeof(NumericalBox), new PropertyMetadata(0));


        public int UpperSide
        {
            get { return (int)GetValue(UpperSideProperty); }
            set { SetValue(UpperSideProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UpperSide.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpperSideProperty =
            DependencyProperty.Register("UpperSide", typeof(int), typeof(NumericalBox), new PropertyMetadata(int.MaxValue));

        protected readonly RoutedUICommand IncreaseCommand =
            new RoutedUICommand("IncreaseCommand", "IncreaseCommand", typeof(NumericalBox));
        protected readonly RoutedUICommand DecreaseCommand =
            new RoutedUICommand("DecreaseCommand", "DecreaseCommand", typeof(NumericalBox));

        static NumericalBox() =>
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericalBox), new FrameworkPropertyMetadata(typeof(NumericalBox)));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            AttachVisualTree();
            AttactCommands();
        }

        private void AttachVisualTree()
        {
            AttacTextBox();
            AttachIncreaseButton();
            AttachDecreaseButton();
        }

        private void AttactCommands()
        {
            CommandBindings.Add(new CommandBinding(IncreaseCommand, (sender, arg) =>
            {
                Value++;
                TextBox.Text = $"{Value}";
            }));
            CommandBindings.Add(new CommandBinding(DecreaseCommand, (sender, arg) =>
            {
                Value--;
                TextBox.Text = $"{Value}";
            }));
        }

        private void AttachIncreaseButton()
        {
            IncreaseButton = GetTemplateChild("PART_IncreaseButton") as RepeatButton;
            if (IncreaseButton == null)
                throw new NullReferenceException("PART_IncreaseButton не найден");

            IncreaseButton.Command = IncreaseCommand;
        }

        private void AttachDecreaseButton()
        {
            DecreaseButton = GetTemplateChild("PART_DecreaseButton") as RepeatButton;
            if (DecreaseButton == null)
                throw new NullReferenceException("PART_DecreaseButton не найден");

            DecreaseButton.Command = DecreaseCommand;
        }

        private void AttacTextBox()
        {
            TextBox = GetTemplateChild("PART_TextBox") as TextInput;
            if (TextBox == null)
                throw new NullReferenceException("PART_TextBox не найден");
            TextBox.Label = Label;

            TextBox.PreviewTextInput += (sender, e) =>
            {
                e.Handled = Regex.IsMatch(e.Text, @"[^\d]");
            };

            TextBox.TextChanged += (sender, e) =>
            {
                Value = int.Parse(TextBox.Text);
            };

            Value = Value;

            TextBox.Text = $"{Value}";
        }
    }
}