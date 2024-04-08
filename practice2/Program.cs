using System;
using System.Dynamic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.ComponentModel;
using System.Security.Principal;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;
using System.Net.NetworkInformation;

namespace practice2
{
    // Тип
    public class Type
    {
        [Key]
        public long id {get;set;}
        public string? name {get;set;}
        public virtual List<Property> properties {get;set;} = new();
    }

    // Районы
    public class District
    {
        [Key]
        public int id {get;set;}
        public string? name {get;set;}
        public virtual List<Property> properties {get;set;} = new();
    }

    // Материалы
    public class Material
    {
        [Key]
        public int id {get;set;}
        public string? name {get;set;}
        public virtual List<Property> properties {get;set;} = new();
    }

    // Объект недвижимости
    public class Property
    {
        [Key]
        public int id {get;set;}
        public int districtId {get;set;}
        public virtual District? district {get;set;}
        public string? address {get;set;}
        public int floor {get;set;}
        public int roomCount {get;set;}
        public int typeId {get;set;}
        public virtual Type? type {get;set;}
        public int saleStatus {get;set;}
        public float price {get;set;}
        public string? description {get;set;}
        public int materialId {get;set;}
        public virtual Material? material {get;set;}
        public float area {get;set;}
        [Column(TypeName =  "timestamp")]
        public DateTime onSaleSince {get;set;}
        public virtual List<Rating> ratings {get;set;} = new();
    }

    // Критерий оценки
    public class Criteria
    {
        [Key]
        public int id {get;set;}
        public string? name {get;set;}
        public virtual List<Rating> ratings {get;set;} = new();
    }

    // Оценка
    public class Rating
    {
        [Key]
        public int id {get;set;}
        public virtual Property? property {get;set;}
        [Column(TypeName =  "timestamp")]
        public DateTime ratedDate {get;set;}
        public virtual Criteria? criteria {get;set;}
        public int rating {get;set;}
    }

    // Риэлтор
    public class Realtor
    {
        [Key]
        public int id {get;set;}
        public string? lastName {get;set;}
        public string? firstName {get;set;}
        public string? patronym {get;set;}
        public string? phoneNumber {get;set;}
        public virtual List<Sale> sales {get;set;} = new();
    }

    // Продажа
    public class Sale
    {
        [Key]
        public int id {get;set;}
        public virtual Property? property {get;set;}
        [Column(TypeName = "timestamp")]
        public DateTime saleDate {get;set;}
        public virtual Realtor? realtor {get;set;}
        public float price {get;set;}
    }

    public class ApplicationContext : DbContext
    {
        public DbSet<Type> types {get;set;} = null!;
        public DbSet<District> districts {get;set;} = null!;
        public DbSet<Material> materials {get;set;} = null!;
        public DbSet<Property> properties {get;set;} = null!;
        public DbSet<Criteria> criteria {get;set;} = null!;
        public DbSet<Rating> ratings {get;set;} = null!;
        public DbSet<Realtor> realtors {get;set;} = null!;
        public DbSet<Sale> sales {get;set;} = null!;

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
            .UseLazyLoadingProxies()
            .UseNpgsql("Host=localhost;Port=5432;Database=housing;Username=voldezhur;Password=291199");
            base.OnConfiguring(optionsBuilder);
        }
    }

    internal class Program
    {
        // Функция для заполнения базы данных информацией
        public static void fillDatabase()
        {
            using (ApplicationContext db = new ApplicationContext())
            {   
                District district1 = new District {id = 1, name = "Район1"};
                District district2 = new District {id = 2, name = "Район2"};
                District district3 = new District {id = 3, name = "Район3"};
                District district4 = new District {id = 4, name = "Район4"};
                District district5 = new District {id = 5, name = "Район5"};


                db.districts.AddRange(district1, district2, district3, district4, district5);

                Type typeApartment = new Type {id = 1, name = "Квартира"};
                Type typeHouse = new Type {id = 2, name = "Дом"};

                db.types.AddRange(typeApartment, typeHouse);

                Material material1 = new Material {id = 1, name = "Кирпич"};
                Material material2 = new Material {id = 2, name = "Дерево"};
                Material material3 = new Material {id = 3, name = "Сталь"};
                Material material4 = new Material {id = 4, name = "Камень"};
                Material material5 = new Material {id = 5, name = "Бетон"};

                db.materials.AddRange(material1, material2, material3, material4,material5);

                Property propertyApartment1 = new Property {id = 1, district = district1, address = "Бутовская улица дом 2", floor = 13, roomCount = 2, type = typeApartment, saleStatus = 1, price = 15000000, description = "Двушка в Бирюлево", material = material1, area = 100, onSaleSince = new DateTime(2023, 1, 15)};
                Property propertyApartment2 = new Property {id = 2, district = district2, address = "Ленинский проспект дом 55", floor = 7, roomCount = 3, type = typeApartment, saleStatus = 1, price = 22000000, description = "Трешка с видом на МГУ", material = material3, area = 120, onSaleSince = new DateTime(2023, 3, 8)};
                Property propertyApartment3 = new Property {id = 3, district = district3, address = "проспект Вернадского 85", floor = 10, roomCount = 1, type = typeApartment, saleStatus = 1, price = 10000000, description = "Однушка с ремонтом", material = material3, area = 60, onSaleSince = new DateTime(2023, 6, 20)};
                Property propertyApartment4 = new Property {id = 4, district = district4, address = "улица Мичурина 30", floor = 5, roomCount = 2, type = typeApartment, saleStatus = 1, price = 17000000, description = "Квартира в центре города", material = material5, area = 90, onSaleSince = new DateTime(2023, 9, 30)};
                Property propertyApartment5 = new Property {id = 5, district = district5, address = "улица Баумана 10", floor = 4, roomCount = 3, type = typeApartment, saleStatus = 1, price = 20000000, description = "Трехкомнатная квартира с балконом", material = material5, area = 110, onSaleSince = new DateTime(2024, 3, 17)};
                Property propertyApartment6 = new Property {id = 6, district = district1, address = "улица Лесная 20", floor = 6, roomCount = 2, type = typeApartment, saleStatus = 1, price = 16000000, description = "Уютная квартира со всеми удобствами", material = material3, area = 95, onSaleSince = new DateTime(2024, 6, 20)};
                Property propertyApartment7 = new Property {id = 7, district = district2, address = "проспект Строителей 40", floor = 8, roomCount = 3, type = typeApartment, saleStatus = 1, price = 21000000, description = "Трехкомнатная квартира с ремонтом", material = material1, area = 120, onSaleSince = new DateTime(2024, 10, 3)};
                Property propertyApartment8 = new Property {id = 8, district = district3, address = "улица Жуковского 25", floor = 9, roomCount = 2, type = typeApartment, saleStatus = 1, price = 18000000, description = "Двушка в новом жилом комплексе", material = material1, area = 85, onSaleSince = new DateTime(2024, 12, 20)};
                Property propertyApartment9 = new Property {id = 9, district = district1, address = "улица Арбат", floor = 2, roomCount = 3, type = typeApartment, saleStatus = 1, price = 50000000, description = "Трешка на Арбате", material = material1, area = 200, onSaleSince = new DateTime(2024, 12, 20)};
                Property propertyApartment10 = new Property {id = 10, district = district5, address = "Пятницкое шоссе", floor = 2, roomCount = 2, type = typeApartment, saleStatus = 1, price = 20000000, description = "Двушка в новом жилом комплексе", material = material1, area = 120, onSaleSince = new DateTime(2024, 12, 30)};

                Property propertyHouse1 = new Property {id = 11, district = district4, address = "Профсоюзная улица 126", floor = 2, roomCount = 5, type = typeHouse, saleStatus = 1, price = 35000000, description = "Коттедж с участком", material = material1, area = 250, onSaleSince = new DateTime(2025, 3, 17)};
                Property propertyHouse2 = new Property {id = 12, district = district5, address = "улица Ломоносова 50", floor = 3, roomCount = 4, type = typeHouse, saleStatus = 1, price = 28000000, description = "Дом на окраине города", material = material2, area = 180, onSaleSince = new DateTime(2025, 3, 17)};
                Property propertyHouse3 = new Property {id = 13, district = district1, address = "улица Гагарина 12", floor = 1, roomCount = 6, type = typeHouse, saleStatus = 1, price = 45000000, description = "Шикарный особняк рядом с парком", material = material3, area = 350, onSaleSince = new DateTime(2025, 3, 20)};
                Property propertyHouse4 = new Property {id = 14, district = district2, address = "проспект Мира 75", floor = 2, roomCount = 4, type = typeHouse, saleStatus = 1, price = 30000000, description = "Уютный дом с садом", material = material4, area = 200, onSaleSince = new DateTime(2025, 6, 30)};
                Property propertyHouse5 = new Property {id = 15, district = district3, address = "переулок Льва Толстого 5", floor = 2, roomCount = 5, type = typeHouse, saleStatus = 1, price = 35000000, description = "Дворец в центре города", material = material4, area = 300, onSaleSince = new DateTime(2025, 6, 30)};
                Property propertyHouse6 = new Property {id = 16, district = district4, address = "переулок Пушкина 15", floor = 3, roomCount = 4, type = typeHouse, saleStatus = 1, price = 29000000, description = "Дом в экологически чистом районе", material = material2, area = 210, onSaleSince = new DateTime(2025, 8, 14)};
                Property propertyHouse7 = new Property {id = 17, district = district5, address = "улица Революции 7", floor = 2, roomCount = 6, type = typeHouse, saleStatus = 1, price = 40000000, description = "Исторический особняк с видом на реку", material = material1, area = 320, onSaleSince = new DateTime(2025, 9, 17)};
                
                db.properties.AddRange(propertyApartment1, propertyApartment2, propertyApartment3, propertyApartment4, propertyApartment5, propertyApartment6,  propertyApartment7, propertyApartment8, propertyApartment9, propertyApartment10, propertyHouse1, propertyHouse2, propertyHouse3, propertyHouse4, propertyHouse5, propertyHouse6, propertyHouse7);

                Criteria criteria1 = new Criteria {id = 1, name = "Отделка"};
                Criteria criteria2 = new Criteria {id = 2, name = "Вид из окон"};
                Criteria criteria3 = new Criteria {id = 3, name = "Соседи"};
                Criteria criteria4 = new Criteria {id = 4, name = "Местность"};
                Criteria criteria5 = new Criteria {id = 5, name = "Безопасность"};

                db.criteria.AddRange(criteria1, criteria2, criteria3, criteria4);

                // Рейтинг
                Rating rating1_1 = new Rating {id = 1, criteria = criteria1, property = propertyApartment1, ratedDate = new DateTime(2024, 03, 28), rating = 3};
                Rating rating1_2 = new Rating {id = 2, criteria = criteria2, property = propertyApartment1, ratedDate = new DateTime(2024, 03, 28), rating = 2};
                Rating rating1_3 = new Rating {id = 3, criteria = criteria3, property = propertyApartment1, ratedDate = new DateTime(2024, 03, 28), rating = 5};
                Rating rating1_4 = new Rating {id = 4, criteria = criteria4, property = propertyApartment1, ratedDate = new DateTime(2024, 03, 28), rating = 5};
                Rating rating1_5 = new Rating {id = 5, criteria = criteria5, property = propertyApartment1, ratedDate = new DateTime(2024, 03, 28), rating = 3};


                Rating rating2_1 = new Rating {id = 6, criteria = criteria1, property = propertyApartment2, ratedDate = new DateTime(2024, 03, 28), rating = 4};
                Rating rating2_2 = new Rating {id = 7, criteria = criteria2, property = propertyApartment2, ratedDate = new DateTime(2024, 03, 28), rating = 3};
                Rating rating2_3 = new Rating {id = 8, criteria = criteria3, property = propertyApartment2, ratedDate = new DateTime(2024, 03, 28), rating = 4};
                Rating rating2_4 = new Rating {id = 9, criteria = criteria4, property = propertyApartment2, ratedDate = new DateTime(2024, 03, 28), rating = 4};
                Rating rating2_5 = new Rating {id = 10, criteria = criteria5, property = propertyApartment2, ratedDate = new DateTime(2024, 03, 28), rating = 5};

                Rating rating3_1 = new Rating {id = 11, criteria = criteria1, property = propertyApartment3, ratedDate = new DateTime(2024, 03, 28), rating = 4};
                Rating rating3_2 = new Rating {id = 12, criteria = criteria2, property = propertyApartment3, ratedDate = new DateTime(2024, 03, 28), rating = 3};
                Rating rating3_3 = new Rating {id = 13, criteria = criteria3, property = propertyApartment3, ratedDate = new DateTime(2024, 03, 28), rating = 5};
                Rating rating3_4 = new Rating {id = 14, criteria = criteria4, property = propertyApartment3, ratedDate = new DateTime(2024, 03, 28), rating = 4};
                Rating rating3_5 = new Rating {id = 15, criteria = criteria5, property = propertyApartment3, ratedDate = new DateTime(2024, 03, 28), rating = 1};

                Rating rating5_1 = new Rating {id = 16, criteria = criteria1, property = propertyApartment5, ratedDate = new DateTime(2024, 03, 28), rating = 5};
                Rating rating5_2 = new Rating {id = 17, criteria = criteria2, property = propertyApartment5, ratedDate = new DateTime(2024, 03, 28), rating = 4};
                Rating rating5_3 = new Rating {id = 18, criteria = criteria3, property = propertyApartment5, ratedDate = new DateTime(2024, 03, 28), rating = 5};
                Rating rating5_4 = new Rating {id = 19, criteria = criteria4, property = propertyApartment5, ratedDate = new DateTime(2024, 03, 28), rating = 5};
                Rating rating5_5 = new Rating {id = 20, criteria = criteria5, property = propertyApartment5, ratedDate = new DateTime(2024, 03, 28), rating = 4};

                Rating rating10_1 = new Rating {id = 21, criteria = criteria1, property = propertyHouse1, ratedDate = new DateTime(2024, 03, 28), rating = 5};
                Rating rating10_2 = new Rating {id = 22, criteria = criteria2, property = propertyHouse1, ratedDate = new DateTime(2024, 03, 28), rating = 4};
                Rating rating10_3 = new Rating {id = 23, criteria = criteria3, property = propertyHouse1, ratedDate = new DateTime(2024, 03, 28), rating = 3};
                Rating rating10_4 = new Rating {id = 24, criteria = criteria4, property = propertyHouse1, ratedDate = new DateTime(2024, 03, 28), rating = 2};
                Rating rating10_5 = new Rating {id = 25, criteria = criteria5, property = propertyHouse1, ratedDate = new DateTime(2024, 03, 28), rating = 5};

                db.ratings.AddRange(rating1_1, rating1_2, rating1_3, rating1_4, rating1_5);
                db.ratings.AddRange(rating2_1, rating2_2, rating2_3, rating2_4, rating2_5);
                db.ratings.AddRange(rating3_1, rating3_2, rating3_3, rating3_4, rating3_5);
                db.ratings.AddRange(rating5_1, rating5_2, rating5_3, rating5_4, rating5_5);
                db.ratings.AddRange(rating10_1, rating10_2, rating10_3, rating10_4, rating10_5);

                Realtor realtor1 = new Realtor {id = 1, firstName = "Василий", lastName = "Пупкин", patronym = "Васильевич", phoneNumber = "8-800-555-35-35"};
                Realtor realtor2 = new Realtor {id = 2, firstName = "Екатерина", lastName = "Иванова", patronym = "Александровна", phoneNumber = "8-800-555-40-40"};
                Realtor realtor3 = new Realtor {id = 3, firstName = "Александр", lastName = "Сидоров", patronym = "Иванович", phoneNumber = "8-800-555-45-45"};
                Realtor realtor4 = new Realtor {id = 4, firstName = "Мария", lastName = "Петрова", patronym = "Сергеевна", phoneNumber = "8-800-555-50-50"};
                Realtor realtor5 = new Realtor {id = 5, firstName = "Иван", lastName = "Федоров", patronym = "Алексеевич", phoneNumber = "8-800-555-55-55"};
                Realtor realtor6 = new Realtor {id = 6, firstName = "Елена", lastName = "Никитина", patronym = "Игоревна", phoneNumber = "8-800-555-60-60"};
                Realtor realtor7 = new Realtor {id = 7, firstName = "Дмитрий", lastName = "Козлов", patronym = "Петрович", phoneNumber = "8-800-555-65-65"};
                Realtor realtor8 = new Realtor {id = 8, firstName = "Ольга", lastName = "Морозова", patronym = "Андреевна", phoneNumber = "8-800-555-70-70"};
                Realtor realtor9 = new Realtor {id = 9, firstName = "Павел", lastName = "Семенов", patronym = "Олегович", phoneNumber = "8-800-555-75-75"};
                Realtor realtor10 = new Realtor {id = 10, firstName = "Татьяна", lastName = "Григорьева", patronym = "Владимировна", phoneNumber = "8-800-555-80-80"};

                db.realtors.AddRange(realtor1, realtor2, realtor3, realtor4, realtor5, realtor6, realtor7, realtor8, realtor9, realtor10);

                Sale sale1 = new Sale {id = 1, price = 30000000, property = propertyApartment1, realtor = realtor1, saleDate = new DateTime(2023, 3, 10)};
                Sale sale2 = new Sale {id = 2, price = 25000000, property = propertyApartment2, realtor = realtor1, saleDate = new DateTime(2023, 4, 11)};
                Sale sale3 = new Sale {id = 3, price = 10000000, property = propertyHouse1, realtor = realtor2, saleDate = new DateTime(2024, 1, 2)};
                Sale sale4 = new Sale {id = 4, price = 15000000, property = propertyApartment1, realtor = realtor3, saleDate = new DateTime(2024, 5, 15)};
                Sale sale5 = new Sale {id = 5, price = 40000000, property = propertyApartment4, realtor = realtor3, saleDate = new DateTime(2024, 5, 17)};
                Sale sale6 = new Sale {id = 6, price = 37000000, property = propertyApartment8, realtor = realtor3, saleDate = new DateTime(2024, 8, 20)};
                Sale sale7 = new Sale {id = 7, price = 32000000, property = propertyHouse3, realtor = realtor4, saleDate = new DateTime(2025, 1, 15)};
                Sale sale8 = new Sale {id = 8, price = 40000000, property = propertyHouse7, realtor = realtor1, saleDate = new DateTime(2025, 1, 18)};
                Sale sale9 = new Sale {id = 9, price = 29000000, property = propertyHouse4, realtor = realtor5, saleDate = new DateTime(2025, 4, 30)};
                Sale sale10 = new Sale {id = 10, price = 21000000, property = propertyApartment10, realtor = realtor4, saleDate = new DateTime(2025, 9, 20)};

                db.sales.AddRange(sale1, sale2, sale3, sale4, sale5, sale6, sale7);

                try 
                {
                    db.SaveChanges();
                }

                catch (Exception ex)
                {
                    Console.Write("\n\n\n========\nПроизошла ошибка при попытке записи в базу данных\nТекст ошибки:\n");
                    Console.Write(ex);
                    Console.Write("\n========\n\n\n");
                }
            }
        }

        public static void outputProperties()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                // Outputting data  from database into the console
                var propertiesList = db.properties.ToList();
                Console.WriteLine("Список недвижимости:");

                foreach (var property in propertiesList)
                {
                    Console.WriteLine($"\nОписание: {property.description}, Тип: {property.type!.name}");
                    Console.WriteLine("Рейтинг:");

                    foreach (var rating in property.ratings)
                    {
                        var criteriaName = "";

                        if (rating.criteria != null)
                        {
                            criteriaName = rating.criteria.name;
                        }

                        Console.WriteLine($"{criteriaName} - {rating.rating}");
                    }
                }
            }
        }

        public static void query1(string selectedDistrictName = "Район1", float lowPrice = 10000000, float highPrice = 40000000)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine($"Объекты в районе {selectedDistrictName} со стоимостью от {lowPrice} и до {highPrice}\n");

                var propertiesInRange = from property in db.properties.ToList()
                    where property.district!.name == selectedDistrictName &&
                        property.price > lowPrice && property.price < highPrice
                    select property;

                foreach (var property in propertiesInRange.ToList())
                {
                    Console.WriteLine($"id недвижимости: {property.id}");
                    Console.WriteLine($"Адрес: {property.address}");
                    Console.WriteLine($"Площадь: {property.area}");
                    Console.WriteLine($"Этаж: {property.floor}\n");
                }
            }
        }
        public static void query2()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine("\n\n\n==========");
                Console.WriteLine("Запрос 2");

                Console.WriteLine("Фамилии риэлторов, продавших двухкомнатные квартиры\n");

                var realtorsTwoRoomApps = from realtor in db.realtors.ToList()
                    where realtor.sales.Any(s => s.property!.roomCount == 2)
                    select realtor;

                foreach (var realtor in realtorsTwoRoomApps.ToList())
                {
                    Console.WriteLine($"ФИО риэлтора: {realtor.lastName} {realtor.patronym} {realtor.firstName}");
                }
            }
        }
        public static void query3(string selectedDistrictName = "Район1")
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine($"Общая стоимость двухкомнатных объектов недвижимости в районе {selectedDistrictName}");

                var propertiesInDistrict = from property in db.properties.ToList()
                    where property.district!.name == selectedDistrictName &&
                        property.roomCount == 2
                    select property;

                double summedPriceInDistrict = propertiesInDistrict.Sum(x => x.price);

                Console.WriteLine(summedPriceInDistrict);   
            }
        }
        public static void query4(string selectedRealtorName = "Пупкин")
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine($"Максимальная и минимальная стоимость объектов, проданных риэлтором {selectedRealtorName}\n");

                var realtorMinMaxPrices = from realtor in db.realtors.ToList()
                    where realtor.lastName == selectedRealtorName
                    select new {maxSale = realtor.sales.Max(x => x.price), minSale = realtor.sales.Min(x => x.price)};
                
                foreach (var realtor in realtorMinMaxPrices)
                {
                    Console.WriteLine($"Максимальная продажа: {realtor.maxSale}");
                    Console.WriteLine($"Минимальная продажа: {realtor.minSale}");
                }
            }
        }
        public static void query5(string selectedRealtorName = "Пупкин", string selectedPropertyType = "Квартира")
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine($"Средняя оценка безопасности недвижимости типа {selectedPropertyType}, проданной риэлтором {selectedRealtorName}");

                var averageSafetyByRealtor = (from sale in db.sales.ToList()
                    where sale.realtor!.lastName == selectedRealtorName &&
                        sale.property!.type!.name == selectedPropertyType
                    select sale.property!.ratings[4].rating).Average();

                Console.WriteLine(averageSafetyByRealtor);
            }
        }
        public static void query6(int selectedFloor = 2)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine($"Количество квартир на этаже {selectedFloor} по районам\n");

                var selectedFloorAppsByDistrict = from property in db.properties.ToList()
                    where property.floor == 2 &&
                        property.type!.name == "Квартира"
                    group property by property.district;

                foreach (var district in selectedFloorAppsByDistrict)
                {
                    Console.WriteLine($"Район - {district.Key.name}, Квартир - {district.Count()}");
                }
            }
        }
        public static void query7(string selectedPropertyType = "Квартира")
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine($"Количество недвижимости типа {selectedPropertyType}, проданные риэлторами\n");

                var propertiesSoldByRealtors = from sale in db.sales.ToList()
                    where sale.property!.type!.name == selectedPropertyType
                    group sale by sale.realtor!.lastName;

                foreach (var realtorProperties in propertiesSoldByRealtors)
                {
                    Console.WriteLine($"Риэлтор - {realtorProperties.Key}:\nПродано недвижимости типа {selectedPropertyType}: {realtorProperties.Count()}");
                }
            }
        }
        public static void query8()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine("Три самых дорогих объекта недвижимости по районам\n");

                var mostExpensiveProperties = from property in db.properties.ToList()
                    orderby property.price descending
                    group property by property.district!.name;
                    
                foreach (var propertiesByDistrict in mostExpensiveProperties)
                {
                    Console.WriteLine($"Район - {propertiesByDistrict.Key}, Самые дорогие объекты:");
                    Console.WriteLine("--------");

                    var expensiveProperties = propertiesByDistrict.Take(3);

                    foreach (var property in expensiveProperties)
                    {
                        Console.WriteLine($"Описание - {property.description}");
                        Console.WriteLine($"Цена - {property.price}\n");
                    }

                    Console.WriteLine();
                }
            }
        }
        public static void query9(string realtorLastName = "Пупкин", string realtorName = "Василий", string realtorPatronym = "Васильевич")
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine($"Годы, в которые {realtorLastName} {realtorName} {realtorPatronym} продал больше 2 объектов недвижимости\n");

                var successfulYearsForRealtor = from sale in db.sales.ToList()
                    where sale.realtor!.lastName == realtorLastName &&
                    sale.realtor!.firstName == realtorName &&
                    sale.realtor!.patronym == realtorPatronym
                    group sale by sale.saleDate.Year;

                foreach (var year in successfulYearsForRealtor)
                {
                    if (year.Count() > 2)
                    {
                        Console.WriteLine($"Год: {year.Key}");

                        foreach (var sale in year)
                        {
                            Console.WriteLine(sale.saleDate);
                        }
                    }
                }
            }
        }
        public static void query10()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine("Годы, в которые было выставлено на продажу 2 или 3 объекта недвижимости\n");

                var yearsWith2or3Properties = from sale in db.sales.ToList()
                    group sale by sale.saleDate.Year into g
                    where g.Count() >= 2 &&
                    g.Count() <= 3
                    select g;
                    

                foreach (var saleYear in yearsWith2or3Properties)
                {
                    Console.WriteLine(saleYear.Key);
                }
            }
        }
        public static void query11()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine("Объекты недвижимости, у которых разница между заявленной и продажной ценой не более 20%\n");

                var propertiesSoldWithin20Percent = from sale in db.sales.ToList()
                    where Math.Abs(sale.price - sale.property!.price) / sale.price <= 0.2
                    select new {sale.property, sale.price, percentage = Math.Abs(sale.price - sale.property!.price) / sale.price};

                foreach (var x in propertiesSoldWithin20Percent)
                {
                    Console.WriteLine($"Адрес - {x.property.address}");
                    Console.WriteLine($"Район - {x.property.district!.name}");
                    Console.WriteLine($"Заявленная цена\t- {x.property.price}");
                    Console.WriteLine($"Продажная цена\t- {x.price}");
                    Console.WriteLine($"Разница\t\t- {x.percentage * 100} процентов\n");
                }
            }
        }
        public static void query12()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine("Адреса недвижимости, где стоимость 1 м^2 меньше средней по району\n");

                var cheapProperties = from property in db.properties.ToList()
                    where property.district!.properties.Average(x => x.price) > property.price
                    select property;
                
                foreach (var property in cheapProperties)
                {
                    Console.WriteLine(property.address);
                }
            }
        }
        public static void query13(int year = 2024)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine($"Риэлторы, ничего не продавшие в {year} году\n");

                var unsuccessfulRealtors = from realtor in db.realtors.ToList()
                    where realtor.sales.Count(x => x.saleDate.Year == 2024) == 0
                    select new { realtor.lastName, realtor.firstName, realtor.patronym };

                foreach (var realtor in unsuccessfulRealtors)
                {
                    Console.Write(realtor.lastName + ' ');
                    Console.Write(realtor.firstName + ' ');
                    Console.Write(realtor.patronym + '\n');
                }
            }
        }
        public static void query14()
        {

        }
        public static void query15(string propertyAddress = "Бутовская улица дом 2")
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine($"Средняя оценка по каждому критерию для объекта недвижимости по адресу {propertyAddress}\n");

                var ratingsForProperty = from rating in db.ratings.ToList()
                    where rating.property!.address == propertyAddress
                    group rating by rating.criteria into g
                    select new { criteria = g.Key.name, rating = g.Average(x => x.rating * 20) };

                foreach (var rating in ratingsForProperty)
                {
                    Console.Write($"{rating.criteria} - {rating.rating}% - ");

                    if (rating.rating < 60)
                    {
                        Console.WriteLine("Неудовлетворительно");
                    }
                    else
                    if (rating.rating >= 60 && rating.rating < 70)
                    {
                        Console.WriteLine("Удовлетворительно");
                    }
                    else
                    if (rating.rating >= 70 && rating.rating < 80)
                    {
                        Console.WriteLine("Хорошо");
                    }
                    else
                    if (rating.rating >= 80 && rating.rating < 90)
                    {
                        Console.WriteLine("Очень хорошо");
                    }
                    else
                    if (rating.rating >= 90)
                    {
                        Console.WriteLine("Превосходно");
                    };
                }
            }
        }

        static void Main(string[] args)
        {
            // Инициализация БД
            // fillDatabase();

            // Запросы
            // query1();
            // query2();
            // query3();
            // query4();
            // query5();
            // query6();
            // query7();
            // query8();
            // query9();
            // query10();
            // query11();
            // query12();
            // query13();
            // query14()
            query15();
        }
    }
}