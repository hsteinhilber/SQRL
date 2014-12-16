using System;
using System.Reflection;

namespace SQRL
{

    static class AssemblyInformation {
        private static Assembly _currentAssembly;
        private static Assembly CurrentAssembly {
            get {
                if (_currentAssembly != null) return _currentAssembly;
                return _currentAssembly = typeof(AssemblyInformation).GetTypeInfo().Assembly;
            }
        }

        private static T GetAttribute<T>() where T : Attribute {
            return CurrentAssembly.GetCustomAttribute<T>();
        }

        private static Version _version;
        public static Version Version {
            get {
                if (_version != null) return _version;
                return _version = CurrentAssembly.GetName().Version;
            }
        }

        private static string _fileVersion;
        public static string FileVersion {
            get {
                if (_fileVersion != null) return _title;
                var fileVersionAttr = GetAttribute<AssemblyFileVersionAttribute>();
                if (fileVersionAttr != null)
                    return _fileVersion = fileVersionAttr.Version.ToString();
                return Version.ToString();
            }
        }

        private static string _infoVersion;
        public static string InformationalVersion {
            get {
                if (_infoVersion != null) return _title;
                var infoVersionAttr = GetAttribute<AssemblyInformationalVersionAttribute>();
                if (infoVersionAttr != null && infoVersionAttr.InformationalVersion.Length > 0)
                    return _fileVersion = infoVersionAttr.InformationalVersion;
                return Version.ToString();
            }
        }

        private static string _title;
        public static string Title {
            get {
                if (_title != null) return _title;
                var titleAttr = GetAttribute<AssemblyTitleAttribute>();
                if (titleAttr != null && titleAttr.Title.Length > 0)
                    return _title = titleAttr.Title;
                return _title = CurrentAssembly.FullName;
            }
        }

        private static string _product;
        public static string Product {
            get {
                if (_product != null) return _product;
                var productAttr = GetAttribute<AssemblyProductAttribute>();
                if (productAttr != null && productAttr.Product.Length > 0)
                    return _product = productAttr.Product;
                return _product = Title;
            }
        }

        private static string _description;
        public static string Description {
            get {
                if (_description != null) return _description;
                var descAttr = GetAttribute<AssemblyDescriptionAttribute>();
                if (descAttr != null && descAttr.Description.Length > 0)
                    return _description = descAttr.Description;
                return _description = Title;
            }
        }

        private static string _company;
        public static string Company {
            get {
                if (_company != null) return _company;
                var companyAttr = GetAttribute<AssemblyCompanyAttribute>();
                if (companyAttr != null && companyAttr.Company.Length > 0)
                    return _company = companyAttr.Company;
                return _company = string.Empty;
            }
        }

        private static string _copyright;
        public static string Copyright {
            get {
                if (_copyright != null) return _copyright;
                var copyrightAttr = GetAttribute<AssemblyCopyrightAttribute>();
                if (copyrightAttr != null && copyrightAttr.Copyright.Length > 0)
                    return _copyright = copyrightAttr.Copyright;
                return _copyright = string.Format("Copyright Â© {0}", DateTime.Now.Year);
            }
        }

        private static string _trademark;
        public static string Trademark {
            get {
                if (_trademark != null) return _trademark;
                var trademarkAttr = GetAttribute<AssemblyTrademarkAttribute>();
                if (trademarkAttr != null && trademarkAttr.Trademark.Length > 0)
                    return _trademark = trademarkAttr.Trademark;
                return _trademark = string.Format("Trademark â„¢ {0}", DateTime.Now.Year);
            }
        }
    }
}
