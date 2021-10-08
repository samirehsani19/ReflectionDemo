using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReflectionDemo
{
    class Program
    {
        #region Main method call all methods
        static void Main(string[] args)
        {

            GetAll();
            GetSingle();
            GetClassName();
            ChangeFieldValue();

            Console.ReadKey();
        }
        #endregion

        #region Change a static field value
        private static void ChangeFieldValue()
        {
            Type type = typeof(Customer);
            FieldInfo property = type.GetField("Experience");
            property.SetValue(null, 10);
            Console.WriteLine($"Experience: {property.GetValue(Customer.Experience)}"); // Output: Experience: 10
        }
        #endregion

        #region Get a class name by customAttribute and reflection
        private static void GetClassName()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            IEnumerable<Type> types = assembly.GetTypes().Where(t => t.GetCustomAttributes<MyClassAttribute>().Count() > 0);
            foreach (var type in types)
            {
                Console.WriteLine($"Type: {type.Name}");  //Output: Customer, because only this use MyClassAttribute
            }
        }
        #endregion

        #region Get single property, field and method
        public static void GetSingle()
        {

            Customer customer = new Customer { Name = "Jahn", Age = 40};
            Customer.Experience = 7;

            Type customerType = typeof(Customer);

            PropertyInfo nameProperty = customerType.GetProperty("Name");
            Console.WriteLine($"Name: {nameProperty.GetValue(customer)}"); //Output: Name: Jahn

            FieldInfo experienceField = customerType.GetField("Experience");
            Console.WriteLine($"Experience: {experienceField.GetValue(customer)}");  // Output: Experience: 7

            MethodInfo methodInfo = customerType.GetMethod("GetFullName");
            string[] fullName = new string[] { "Samir", "Ehsani" };

            object result = methodInfo.Invoke(customer, fullName);
            Console.WriteLine($"FullName: {result}"); //OutPut: FirstName: Samir LastName: Ehsani
        }
        #endregion

        #region Get all properties, fields, methods and classes
        public static void GetAll()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();

            // Loop through all types
            foreach (var type in types)
            {
                //Output Program, Customer, MyClassAttribute
                Console.WriteLine($"Type: {type.Name}");

                // get all properties
                PropertyInfo[] props = type.GetProperties();
                foreach (var prop in props)
                {
                    //Output: Type: System.String Property: Name, System.Int32 Property: Age
                    Console.WriteLine($"Type: {prop.PropertyType} Property: {prop.Name}");
                }

                // Get all fields
                FieldInfo[] fields = type.GetFields();
                foreach (var field in fields)
                {
                    //Type: System.Int32 Field:Experience
                    Console.WriteLine($"Type: {field.FieldType} Field: {field.Name}");
                }

                // Get all methods
                MethodInfo[] methods = type.GetMethods();
                foreach (var method in methods)
                {
                    Console.WriteLine($"Method: {method.Name}");
                }

            }
        }
        #endregion

    }

    #region Customer class with custom attribute
    [MyClass]
    public class Customer
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public static int Experience;
        public string GetFullName(string firstName, string lastName)
        {
            return $"FirstName: {firstName} LastName:{lastName}";
        }
    }
    #endregion

    #region Custom attribute
    //Use this attribute only on Classes
    [AttributeUsage(AttributeTargets.Class)]
    public class MyClassAttribute : Attribute { }
    #endregion
}
