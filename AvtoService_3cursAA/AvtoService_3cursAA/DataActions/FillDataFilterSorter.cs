﻿using Microsoft.EntityFrameworkCore;
using AvtoService_3cursAA.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace AvtoService_3cursAA.ActionsForEmployee
{
    /// <summary>
    /// Для фильтрации и сортировки сотрдуников
    /// </summary>
    public class FillDataFilterSorter
    {
        private readonly static Avtoservice3cursAaContext dbContext = new Avtoservice3cursAaContext();
        private readonly static List<object> _filterList = new List<object>
        {
            "Без фильтрации",
            new Separator { Margin = new Thickness(0, 5, 0, 5), Width = 150 }
        };
        private readonly static List<object> _sorterList = new List<object>
        {
            "По дате добавления",
            new Separator { Margin = new Thickness(0, 5, 0, 5), Width = 150 }
        };

        private readonly static List<object> _clientsList = new List<object>
        {
            "Выберите клиента",
            new Separator { Margin = new Thickness(0, 5, 0, 5), Width = 150 }
        };
        private readonly static List<object> _carsList = new List<object>
        {
            "Выберите машину",
            new Separator { Margin = new Thickness(0, 5, 0, 5), Width = 150 }
        };
        private readonly static List<object> _typeOfRepairList = new List<object>
        {
            "Выберите тип ремонта",
            new Separator { Margin = new Thickness(0, 5, 0, 5), Width = 150 }
        };

        public static List<object> FillListClients()
        {
            var list = new List<object>(_clientsList);

            foreach (var item in dbContext.Clients)
            {
                list.Add(item.FullName);
            }
            return list;
        }
        public static List<object> FillListCars(Client client)
        {
            var list = new List<object>(_carsList);
            var listCars = client.CarList;

            foreach (var car in listCars)
            {
                list.Add(car);
            }
            return list;
        }
        public static List<object> FillTypeOfRepairList()
        {
            var list = new List<object>(_typeOfRepairList);
            foreach (var item in dbContext.Typeofrepairs)
            {
                list.Add(item.Name);
            }
            return list;
        }

        public static List<object> FillFilterEmployee()
        {
            var filterList = new List<object>(_filterList);
            foreach (var role in dbContext.Roles)
            {
                filterList.Add(role.Name);
            }
            return filterList;
        }
        public static List<object> FillSorterEmployee()
        {
            var sorterList = new List<object>(_sorterList);
            var strings = new List<object>
            {
                "По имени",
                "По фамилии",
                "По отчеству",
                "По дате рождения",
                "По стажу работы",
                "По паспорту",
                "По номеру телефона",
                "По логино",
                "По паролю"
            };
            sorterList.AddRange(strings);
            return sorterList;
        }
        public static List<object> FillSorterClient()
        {
            var sorterList = new List<object>(_sorterList);
            var strings = new List<object>
            {
                "По имени",
                "По фамилии",
                "По отчеству",
                "По дате рождения",
                "По номеру телефона"
            };
            sorterList.AddRange(strings);
            return sorterList;
        }
        public static List<object> FillSorterPrices()
        {
            var sorterList = new List<object>(_sorterList);
            var strings = new List<object>
            {
                "По имени",
                "По цене"
            };
            sorterList.AddRange(strings);
            return sorterList;
        }

        public static List<object> FillSorterDetails()
        {
            var sorterList = new List<object>(_sorterList);
            var strings = new List<object>
            {
                "По имени",
                "По цене",
                "По количеству"
            };
            sorterList.AddRange(strings);
            return sorterList;
        }

        public static List<object> FillSorterCars()
        {
            var sorterList = new List<object>(_sorterList);
            var strings = new List<object>
            {
                "По бренду",
                "По модели",
                "По стране производства",
                "По году производства"
            };
            sorterList.AddRange(strings);
            return sorterList;
        }

        public static List<object> FillSorterDetailsCount()
        {
            var filterList = new List<object>(_filterList);
            var strings = new List<object>
            {
                "Имеется на слкаде",
                "Нет на складе"
            };
            filterList.AddRange(strings);
            return filterList;
        }
    }
}
