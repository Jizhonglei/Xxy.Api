﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace IFramework.Ioc.Reflection
{
    public abstract class DAssemblyFinder : IAssemblyFinder
    {
        protected Func<Assembly, bool> DefaultPredicate { get; set; }

        public virtual IEnumerable<Assembly> FindAll()
        {
            //var path = AppDomain.CurrentDomain.RelativeSearchPath;
            //if (!Directory.Exists(path))
            //    path = AppDomain.CurrentDomain.BaseDirectory;

            var asses = AppDomain.CurrentDomain.GetAssemblies().ToList();

            //var asses = Directory.GetFiles(path, "*.*").Where(s => s.EndsWith(".dll") || s.EndsWith(".exe")).Select(Assembly.LoadFrom).ToList();
            return DefaultPredicate != null ? asses.Where(DefaultPredicate) : asses;
        }

        public IEnumerable<Assembly> Find(Func<Assembly, bool> expression)
        {
            return FindAll().Where(expression);
        }
    }
}