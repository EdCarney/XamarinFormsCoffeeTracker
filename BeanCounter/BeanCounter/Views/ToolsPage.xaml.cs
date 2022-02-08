using System;
using System.ComponentModel;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BeanCounter.Views
{
    public partial class ToolsPage : ContentPage
    {
        private readonly string[] unitPickerOpts = new string[] {"oz", "fl oz", "grams"};
        private bool updateLock = false;
        private const double waterPerCoffeeGrams = 17;

        public ToolsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            PopulateUnitOptions();
        }

        private void PopulateUnitOptions()
        {
            CoffeeUnitPicker.ItemsSource = unitPickerOpts;
            WaterUnitPicker.ItemsSource = unitPickerOpts;

            CoffeeUnitPicker.SelectedIndex = 0;
            WaterUnitPicker.SelectedIndex = 0;
        }

        private void WaterAmountChanged(object sender, PropertyChangedEventArgs e)
        {
            if (updateLock)
            {
                return;
            }
            RecalculateCoffeeForWater();
        }

        private void CoffeeAmountChanged(object sender, PropertyChangedEventArgs e)
        {
            if (updateLock)
            {
                return;
            }
            RecalculateWaterForCoffee();
        }

        void WaterUnitPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            RecalculateWaterForCoffee();
        }

        void CoffeeUnitPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            RecalculateCoffeeForWater();
        }

        private void RecalculateCoffeeForWater()
        {
            updateLock = true;
            if (double.TryParse(WaterAmountEntry.Text, out double waterAmount))
            {
                double conversionRatio = GetUnitConversionCoffeeForWater();
                double coffeeAmount = (waterAmount / waterPerCoffeeGrams) * conversionRatio;
                CoffeeAmountEntry.Text = coffeeAmount.ToString("F2");
            }
            else
            {
                CoffeeAmountEntry.Text = string.Empty;
            }
            updateLock = false;
        }

        private void RecalculateWaterForCoffee()
        {
            updateLock = true;
            if (double.TryParse(CoffeeAmountEntry.Text, out double coffeeAmount))
            {
                double conversionRatio = GetUnitConversionWaterForCoffee();
                double waterAmount = coffeeAmount * waterPerCoffeeGrams * conversionRatio;
                WaterAmountEntry.Text = waterAmount.ToString("F2");
            }
            else
            {
                WaterAmountEntry.Text = string.Empty;
            }
            updateLock = false;
        }

        private double GetUnitConversionCoffeeForWater()
        {
            string coffeeUnit = unitPickerOpts[CoffeeUnitPicker.SelectedIndex];
            string waterUnit = unitPickerOpts[WaterUnitPicker.SelectedIndex];
            return GetUnitToUnitRatio(waterUnit, coffeeUnit);
        }

        private double GetUnitConversionWaterForCoffee()
        {
            string coffeeUnit = unitPickerOpts[CoffeeUnitPicker.SelectedIndex];
            string waterUnit = unitPickerOpts[WaterUnitPicker.SelectedIndex];
            return GetUnitToUnitRatio(coffeeUnit, waterUnit);
        }

        private double GetUnitToUnitRatio(string fromUnit, string toUnit)
        {
            const double gramsInOz = 28.3495;
            const double gramsInFlOz = 29.57;
            const double ozInFlOz = 1.04;

            if (fromUnit == toUnit)
            {
                return 1;
            }
            if (fromUnit == "oz" && toUnit == "grams")
            {
                return gramsInOz;
            }
            if (fromUnit == "grams" && toUnit == "oz")
            {
                return 1 / gramsInOz;
            }
            if (fromUnit == "fl oz" && toUnit == "grams")
            {
                return gramsInFlOz;
            }
            if (fromUnit == "grams" && toUnit == "fl oz")
            {
                return 1 / gramsInFlOz;
            }
            if (fromUnit == "fl oz" && toUnit == "oz")
            {
                return ozInFlOz;
            }
            if (fromUnit == "oz" && toUnit == "fl oz")
            {
                return 1 / ozInFlOz;
            }

            return 0;
        }

        void OnClearButtonClicked(object sender, EventArgs e)
        {
            CoffeeAmountEntry.Text = string.Empty;
            WaterAmountEntry.Text = string.Empty;
        }
    }
}
