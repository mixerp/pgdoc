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
using MixERP.Net.Utilities.PgDoc.Helpers;

namespace MixERP.Net.Utilities.PgDoc.Generators
{
    internal static class AssetRunner
    {
        internal static void Run()
        {
            Console.WriteLine("Generating stylesheets and scripts.");
            
            FileHelper.WriteResourceToOutPutDirectory("MixERP.Net.Utilities.PgDoc.Configs.Template.Scripts.jquery.min.js",
                "Scripts/jquery.min.js");

            FileHelper.WriteResourceToOutPutDirectory("MixERP.Net.Utilities.PgDoc.Configs.Template.Scripts.prism.min.js",
                "Scripts/prism.min.js");
            
            FileHelper.WriteResourceToOutPutDirectory("MixERP.Net.Utilities.PgDoc.Configs.Template.Scripts.semantic.min.js",
                "Scripts/semantic.min.js");
            
            FileHelper.WriteResourceToOutPutDirectory("MixERP.Net.Utilities.PgDoc.Configs.Template.Stylesheets.mixerp.css",
                "Stylesheets/mixerp.css");
            
            FileHelper.WriteResourceToOutPutDirectory("MixERP.Net.Utilities.PgDoc.Configs.Template.Stylesheets.prism.min.css",
                "Stylesheets/prism.min.css");
            
            FileHelper.WriteResourceToOutPutDirectory("MixERP.Net.Utilities.PgDoc.Configs.Template.Stylesheets.semantic.min.css",
                "Stylesheets/semantic.min.css");
            
            FileHelper.WriteResourceToOutPutDirectory(
                "MixERP.Net.Utilities.PgDoc.Configs.Template.Stylesheets.themes.default.assets.fonts.icons.eot",
                "Stylesheets/themes/default/assets/fonts/icons.eot");
            
            FileHelper.WriteResourceToOutPutDirectory(
                "MixERP.Net.Utilities.PgDoc.Configs.Template.Stylesheets.themes.default.assets.fonts.icons.otf",
                "Stylesheets/themes/default/assets/fonts/icons.otf");
            
            FileHelper.WriteResourceToOutPutDirectory(
                "MixERP.Net.Utilities.PgDoc.Configs.Template.Stylesheets.themes.default.assets.fonts.icons.svg",
                "Stylesheets/themes/default/assets/fonts/icons.svg");
            
            FileHelper.WriteResourceToOutPutDirectory(
                "MixERP.Net.Utilities.PgDoc.Configs.Template.Stylesheets.themes.default.assets.fonts.icons.ttf",
                "Stylesheets/themes/default/assets/fonts/icons.ttf");
            
            FileHelper.WriteResourceToOutPutDirectory(
                "MixERP.Net.Utilities.PgDoc.Configs.Template.Stylesheets.themes.default.assets.fonts.icons.woff",
                "Stylesheets/themes/default/assets/fonts/icons.woff");
            
            FileHelper.WriteResourceToOutPutDirectory(
                "MixERP.Net.Utilities.PgDoc.Configs.Template.Stylesheets.themes.default.assets.images.flags.png",
                "Stylesheets/themes/default/assets/images/flags.png");
        }
    }
}