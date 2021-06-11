using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ReportingReflection
{
    public class CSVGenerator<T>
    {
        private IEnumerable<T> _data;
        private string _filename;
        private Type _type;

        public CSVGenerator(IEnumerable<T> data, string filename)
        {
            _data = data;
            _filename = filename;

            _type = typeof(T);
        }

        public void Generate()
        {
            var rows = new List<string>();

            rows.Add(CreateHeader());

            foreach (var item in _data)
                rows.Add(CreateRow(item));

            File.WriteAllLines($"{_filename}.csv", rows, Encoding.UTF8);
        }

        private string CreateHeader()
        {
            var properties = _type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var orderedProps = properties.OrderBy(p => p.GetCustomAttribute<ReportItemAttribute>().ColumnOrder);

            var bob = new StringBuilder();

            foreach (var prop in orderedProps)
            {
                var attr = prop.GetCustomAttribute<ReportItemAttribute>();

                bob.Append(attr.Heading ?? prop.Name).Append(",");
            }

            return bob.ToString()[..^1];
        }

        private string CreateRow(T item)
        {
            var properties = _type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var orderedProps = properties.OrderBy(p => p.GetCustomAttribute<ReportItemAttribute>().ColumnOrder);

            var bob = new StringBuilder();

            foreach (var prop in orderedProps)
            {
                bob.Append(CreateItem(prop, item)).Append(",");
            }

            return bob.ToString()[..^1];
        }

        private string CreateItem(PropertyInfo prop, T item)
        {
            var attr = prop.GetCustomAttribute<ReportItemAttribute>();

            return string.Format($"{{0:{attr.Format}}}{attr.Units}", prop.GetValue(item));
        }
    }

}
