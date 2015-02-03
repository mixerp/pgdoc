/********************************************************************************
Copyright (C) Binod Nepal, Mix Open Foundation (http://mixof.org).

This file is part of MixERP.

MixERP is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MixERP is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MixERP.  If not, see <http://www.gnu.org/licenses/>.
***********************************************************************************/
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MixERP.Net.Utilities.PgDoc.Helpers
{
    internal static class DependencyHandler
    {
        internal static Assembly ResolveEventHandler(Object sender, ResolveEventArgs args)
        {
            String dllName = new AssemblyName(args.Name).Name + ".dll";

            Assembly assem = Assembly.GetExecutingAssembly();

            String resourceName = assem.GetManifestResourceNames().FirstOrDefault(rn => rn.EndsWith(dllName));

            if (resourceName == null) return null;

            using (Stream stream = assem.GetManifestResourceStream(resourceName))
            {
                if (stream == null) return null;
                
                Byte[] assemblyData = new Byte[stream.Length];

                stream.Read(assemblyData, 0, assemblyData.Length);

                return Assembly.Load(assemblyData);
            }
        }
    }
}