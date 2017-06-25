using Orchard.Localization;
using Orchard.UI.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pluralsight.Movies
{
    public class MovieAdminMenu : INavigationProvider
    {
        public Localizer T { get; set; }

        public string MenuName => "admin";

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T("Movies"), "5", BuildMenu);
        }

        private void BuildMenu(NavigationItemBuilder menu)
        {
	        menu.Add(T("List"), "1.0", item =>
		        item.Action("List", "Admin", new { area = "Contents", id = "Movie" }));

	        menu.Add(T("New Movie"), "2.0", item =>
		        item.Action("Create", "Admin", new { area = "Contents", id = "Movie" }));

            // Admin/Actors
	        menu.Add(T("Actors"), "3.0", item =>
		        item.Action("Index", "ActorsAdmin", new { area = "Pluralsight.Movies" }));
        }
    }
}