using BuildingCompany.Connection;
using BuildingCompany.Model.ModelBases;
using System;
using System.Text.RegularExpressions;

namespace BuildingCompany.Model.Entities
{
    public class HouseModel : ModelBase
    {
        private House _house;

        public int ID => _house.ID;
        public string Name
        {
            get => _house.Name;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Название не может быть пустым");
                if (Regex.IsMatch(value.Trim(), @"[^\w\d\- ]"))
                    throw new ArgumentException("Название может содержать только буквы, цифры, пробел и дефис");

                _house.Name = value.Trim();
                OnPropertyChanged();
            }
        }
        public decimal Area
        {
            get => _house.Area;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Площадь не может быть меньше 0");
                if (value > 9999.99M)
                    throw new ArgumentOutOfRangeException("Слишком большое значение");
                _house.Area = value;
                OnPropertyChanged();
            }
        }
        public decimal Floors
        {
            get => _house.Floors;
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("Этажность не может быть меньше 1");
                if (value > 99)
                    throw new ArgumentOutOfRangeException("Слишком большое значение");

                _house.Floors = value;
                OnPropertyChanged();
            }
        }
        public decimal Cost
        {
            get => _house.Cost;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Цена не может быть отрицательной");
                if (value > 9999999999.99M)
                    throw new ArgumentOutOfRangeException("Слишком большое значение");
                _house.Cost = value;
                OnPropertyChanged();
            }
        }
        public byte[] Photo
        {
            get => _house.Photo ?? SystemImage.GetImageByName("noimage");
            set
            {
                _house.Photo = value;
                OnPropertyChanged();
            }
        }
        public bool Deleted => _house.IsDeleted;

        public void Delete()
        {
            _house.IsDeleted = true;
            OnPropertyChanged(nameof(Deleted));
        }
    }
}
